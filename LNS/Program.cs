using System;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LNS
{
    class Program
    {

        static void Main(string[] args)
        {
            // Get problem from text file
            ReadProblem reader = new ReadProblem("TestProblem.txt");
            Problem problem = reader.GetProblem();

            // Initial routing plan with one route per visit
            List<Route> initialRoutes = new List<Route>();
            foreach (Visit visit in problem.GetVisits())
            {
                initialRoutes.Add(new Route(new List<Visit> { visit }, problem.GetCapacity()));
            }
            RoutingPlan initialPlan = new RoutingPlan(initialRoutes, problem);

            // Print Initial Plan
            initialPlan.PrintRoutingPlan();

            // Print distances between visits
            /*
            foreach (Visit visit1 in problem.GetVisits())
            {
                foreach (Visit visit2 in problem.GetVisits().GetRange(0, 30))
                {
                    int dist = (int)problem.GetVisitDistance(visit1, visit2);
                    if (dist < 10)
                    {
                        Console.Write("  " + dist + "  ");
                    }
                    else if (dist < 100)
                    {
                        Console.Write("  " + dist + " ");
                    }
                    else if (dist < 1000)
                    {
                        Console.Write("  " + dist + "");
                    }
                }
                Console.WriteLine();
            }
            */

            int maxIterations = 1000;
            int discrepancies = 5; // discrep
            int attempts = 250; // a
            int determinism = 5; // D

            LargeNeighbourhoodSearch LNS = new LargeNeighbourhoodSearch(initialPlan, 
                maxIterations, discrepancies, attempts, determinism);
            RoutingPlan plan = LNS.Search();
            plan.PrintRoutingPlan();
        }
    }
    
    
    
    
    
    
    
    /*
    class Program
    {
        static void Main(string[] args)
        {
            // Initial RoutingPlan
            RoutingPlan bestPlan = new RoutingPlan(); // TODO: MAKE EXAMPLE?


            // Create timer for the while loop
            Timer timer = new Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
            bool running = true;
            void timer_Elapsed(object sender, ElapsedEventArgs e)
            {
                running = false;
            }


            // Run LNS while the timer is not elapsed
            while (running)
            {
                // Loop over the amount of costumers per neighbourhood
                for(int n = 1; n < 100; n++)
                {
                    // TODO: MAYBE MAKE COPY OF bestPlan SO POINTER DONT BOTHER ME
                    Tuple<RoutingPlan, List<Visit>> removedPlan = RemoveVisits(bestPlan, n, 5);
                    RoutingPlan newPlan = Reinsert(removedPlan.Item1, removedPlan.Item2, 1000);

                    if (Cost(newPlan) <= Cost(bestPlan))
                    {
                        bestPlan = newPlan;
                    }
                }

                break;
            }
            // Close timer
            timer.Enabled = false;
        }

        static public double Cost(RoutingPlan plan)
        {

        }

        static Tuple<RoutingPlan, List<Visit>> RemoveVisits(RoutingPlan plan, int toRemove, double D)
        {
            // Get first visit to remove
            List<Visit> inplan = plan.GetVisits();
            Visit visit = ChooseRandomVisit(inplan);
            inplan.Remove(visit);
            plan.RemoveVisit(visit);
            List<Visit> removed = new List<Visit> {visit};

            // While there is more to remove, get more visits related to v
            while (removed.Count < toRemove)
            {
                Visit currentVisit = ChooseRandomVisit(removed);
                List<Visit> lst = RankUsingRelatedness(currentVisit, inplan);
                double rand = new Random().Next(0, 1);
                int location = (int) (lst.Count * Math.Pow(rand, D));
                currentVisit = lst[location];
                removed.Add(currentVisit);
                inplan.Remove(currentVisit);
                plan.RemoveVisit(currentVisit);
            }

            return new Tuple<RoutingPlan, List<Visit>>(plan, removed);
        }

        // Return random element from list of visits
        static public Visit ChooseRandomVisit(List<Visit> visits)
        {
            int rand = new Random().Next(visits.Count);
            return visits[rand];
        }


        // Rank all visits to a certain visit by relatedness in decreasing order
        static public List<Visit> RankUsingRelatedness(Visit visit, List<Visit> allVisits)
        {
            List<Visit> ranked = allVisits.OrderByDescending(o => o.Relatedness(visit)).ToList();
            return ranked;
        }

        static RoutingPlan Reinsert(RoutingPlan plan, List<Visit> visits, int discrep)
        {
            if (visits.Count != 0)
            {
                Visit visit = ChooseFarthestVisit(visits);
                int i = 0;
                foreach (Tuple<int,int> position in RankedPositions(visit))
                {
                    if (i <= discrep)
                    {
                        // TODO: NO POINTER PROBLEMS?
                        RoutingPlan currentPlan = plan; // TODO: SHAW STORES HERE, IS THERE A DIFFERENCE? DOES HE KEEP IT STORED OUTSIDE OF THIS FUNCITON?
                        currentPlan.AddStopToRoute(visit, position);
                        List<Visit> newVisits = visits;
                        newVisits.Remove(visit);
                        Reinsert(currentPlan, newVisits, discrep - i);
                        i++;
                    }
                }
            }
            return plan;
        }

        static Visit ChooseFarthestVisit(List<Visit> visits)
        {

        }

    }

    // Classes
    public class Visit
    {
        bool depot;
        int x_value;
        int y_value;
        int begin_window;
        int end_window;

        public Visit(Tuple<int, int> visit)
        {
            depot = true;
            x_value = visit.Item1;
            y_value = visit.Item2;
        }

        public Visit(Tuple<int,int,int,int> visit)
        {
            depot = false;
            x_value = visit.Item1;
            y_value = visit.Item2;
            begin_window = visit.Item3;
            end_window = visit.Item4;
        }

        // TODO: DEZE MOET NOG AANGEPAST WORDEN NAAR DE WERKELIJKE KOSTEN FUNCTIE
        public double Relatedness(Visit other)
        {
            return (Math.Pow(x_value - other.x_value, 2) + Math.Pow(y_value - other.y_value, 2));
        }
    }

    public class VisitSet
    {
        public VisitSet()
        {

        }
    }

    // Depot is at 0,0 and currently I take the euclidic distance as cost
    public class RoutingPlan
    {

        // Initialize routingplan
        List<Route> routingPlan = new List<Route>();

        public RoutingPlan(List<Tuple<int,int,int,int>> locations)
        {

            // Add every visit to a unique route inbetween the depot
            // and every route to the routing plan
            foreach (Tuple<int, int, int,int> location in locations)
            {
                Visit visit = new Visit(location);
                Route route = new Route();
                route.AddStop(visit, 1);
                routingPlan.Add(route);
            }
        }

        public void AddStopToRoute(Visit visit, Tuple<int,int> position)
        {
            routingPlan[position.Item1].AddStop(visit, position.Item2);
        }

        public List<Visit> GetVisits()
        {
            List<Visit> allVisits = new List<Visit>();
            foreach (Route route in routingPlan)
            {
                List<Visit> visits = route.GetRouteVisits();
                allVisits.AddRange(visits);
            }
            return allVisits;
        }

        public void RemoveVisit(Visit visit)
        {
            for (int i = 0; i < routingPlan.Count; i++)
            {
                if (routingPlan[i].Contains(visit))
                {
                    routingPlan[i].RemoveVisit(visit);
                    return;
                }
            }
        }
    }

    public class Route
    {
        List<Visit> route = new List<Visit>();

        public Route()
        {
            // Add depot to beginning and end of the route
            route.Add(new Visit(new Tuple<int, int>(0,0)));
            route.Add(new Visit(new Tuple<int, int>(0,0)));
        }

        public void AddStop(Visit visit, int location)
        {
            route.Insert(location, visit);
        }

        public List<Visit> GetRouteVisits()
        {
            return route;
        }

        public bool Contains(Visit visit)
        {
            if (route.Contains(visit))
            {
                return true;
            }
            return false;
        }

        public void RemoveVisit(Visit visit)
        {
            route.Remove(visit);
        }
    }
    */
}
