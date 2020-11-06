using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LNS
{
    class RoutingPlan : ICloneable// VrpSolution
    {
        List<Route> routes;
        //List<Visit> removed;
        int vehicles;
        Problem problem;
        double planCost;
        List<Visit> allVisits;

        public RoutingPlan(List<Route> routes, Problem problem)
        {
            this.routes = routes;
            this.problem = problem;
            this.vehicles = routes.Count;
            //this.removed = new List<Visit>();

            allVisits = new List<Visit>();
            foreach (Route route in routes)
            {
                foreach (Visit visit in route.GetVisits())
                {
                    allVisits.Add(visit);
                }
            }
        }

        public Problem GetProblem()
        {
            return problem;
        }

        public object Clone()
        {
            return this;
        }

        public void Tester(Visit visit, (int, int) position)
        {
            foreach (Route route in routes)
            {
                if (route.Contains(visit))
                {
                    route.RemoveVisit(visit);
                }
            }
            int count = routes.Count;
            for (int i = 0; i < count; i++)
            {
                if (routes[i].GetVisits().Count == 0)
                {
                    routes.Remove(routes[i]);
                }
                count = routes.Count;
            }

            routes[position.Item1].AddVisit(visit, position.Item2);

        }

        public void PlaceVisit(Visit visit, (int, int) position)
        {
            // Remove visit from previous position
            foreach (Route route in routes)
            {
                if (route.Contains(visit))
                {
                    route.RemoveVisit(visit);
                    break;
                }
            }


            // Check for empty routes and delete them
            int count = routes.Count;
            for (int i = 0; i < count; i++)
            {
                if (routes[i].GetVisits().Count == 0)
                {
                    routes.Remove(routes[i]);
                }
                count = routes.Count;
            }


            // Place visit at next position
            routes[position.Item1].AddVisit(visit, position.Item2);
        }

        public List<Route> CopyRoutes()
        {
            List<Route> copy = new List<Route>();
            foreach (Route route in routes)
            {
                Route copyRoute = route.Clone();
                copy.Add(copyRoute);
            }
            return copy;
        }

        public double CalculateCost()
        {
            planCost = 0;
            foreach (Route route in routes)
            {
                List<Visit> visits = route.GetVisits();

                // distance depot -> first visit
                planCost += problem.GetDistanceFromDepot(visits[0]);

                // distances first visit -> ... -> last visit
                for (int i = 1; i < visits.Count - 1; i++)
                {
                    planCost += problem.GetVisitDistance(visits[i], visits[i + 1]);
                }

                // distance last visit -> depot
                planCost += problem.GetDistanceFromDepot(visits[visits.Count - 1]);
            }
            return planCost;
        }

        public double GetPlanCost()
        {
            return planCost;
        }

        /*
        public void RemoveVisit(Visit visit)
        {
            foreach (Route route in routes)
            {
                if (route.Contains(visit))
                {
                    route.RemoveVisit(visit);
                    removed.Add(visit);
                    return;
                }
            }
        }
        

        public void PlaceVisit(Visit visit, (int, int) position)
        {
            for (int i = 0; i < routes.Count; i++)
            {
                if (i == position.Item1)
                {
                    routes[i].AddVisit(visit, position.Item2);
                    removed.Remove(visit);
                    break;
                }
            }
        }
        */

        public List<Visit> RankRelatedness(Visit visit, List<Visit> removed)
        {
            // Rank them by relatedness
            List<Visit> related = allVisits.OrderByDescending(o => Relatedness(visit, o)).ToList();

            // Exclude removed visits
            foreach (Visit visit1 in removed)
            {
                related.Remove(visit1);
            }

            return related;
        }

        public double Relatedness(Visit visit1, Visit visit2)
        {
            // Get normalised distance
            double maxDistance = problem.GetMaxDistance();
            double distance = problem.GetVisitDistance(visit1, visit2);
            double normDistance = distance / maxDistance;

            // TODO: HOW CAN I CHECK IF A REMOVED VISIT IS IN THE SAME ROUTE? V_ij OF RELATEDNESS
            int sameRoute = 0;

            return 1 / (normDistance + sameRoute);
        }

        public Visit ChooseFarthest(List<Visit> visits)
        {
            double maxDistance = 0;
            Visit farthest = visits[0];
            foreach (Visit visit in visits)
            {
                double distance = problem.GetDistanceFromDepot(visit);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    farthest = visit;
                }
            }
            return farthest;
        }

        public List<(int, int)> RankedPositions(Visit visit)
        {
            // Make list of all routes and positions
            List<(int, int)> positions = new List<(int, int)>();
            for (int i = 0; i < routes.Count - 1; i++)
            {
                // Loop over possible positions in current route
                for (int j = 0; j < routes[i].GetVisits().Count + 1; j++)
                {
                    // TODO: CHECK IF (TIMEWINDOWS, MAXSERVICETIME, )AND CAPACITY WORK, AND ITS OLD POSTITION
                    // Check if it fits in timewindow
                    if (TimeWindowCheck(visit, (i,j)) && CapacityCheck(visit, i))
                    {
                        positions.Add((i, j));
                    }
                }
            }

            // Sort positions on score
            List<(int, int)> sortedPositions = positions.OrderBy(o => Score(visit, o)).ToList();
            /*
            foreach ((int, int) position in sortedPositions)
            {
                double score = Score(visit, position);
                Console.WriteLine(score);
            }
            */
            return positions;
        }

        public double Score(Visit visit, (int, int) location)
        {
            int route = location.Item1;
            int position = location.Item2;
            double cost = 0;
            // if j = 0, get depot -> visit -> visits[j] without depot -> visit[j]
            if (position == 0)
            {
                cost += problem.GetDistanceFromDepot(visit);
                cost += problem.GetVisitDistance(visit, routes[route].GetVisits()[position]);

                cost -= problem.GetDistanceFromDepot(routes[route].GetVisits()[position]);
            }
            // if 0 < j < count, get visits[j-1] -> visit -> visits[j] without visits[j-1] -> visits[j]
            else if (position > 0 && position < routes[route].GetVisits().Count)
            {
                cost += problem.GetVisitDistance(routes[route].GetVisits()[position - 1], visit);
                cost += problem.GetVisitDistance(visit, routes[route].GetVisits()[position]);

                cost -= problem.GetVisitDistance(routes[route].GetVisits()[position - 1], routes[route].GetVisits()[position]);
            }
            // if j = count, get visits[j-1] -> visit -> depot without visits[j-1] -> depot
            else if (position == routes[route].GetVisits().Count)
            {
                cost += problem.GetVisitDistance(routes[route].GetVisits()[position - 1], visit);
                cost += problem.GetDistanceFromDepot(visit);

                cost -= problem.GetDistanceFromDepot(routes[route].GetVisits()[position - 1]);
            }

            return cost;
        }


        public void PrintRoutingPlan()
        {
            for (int i = 0; i < routes.Count; i++)
            {
                Console.Write("Route " + i + ": Depot -> ");
                foreach (Visit visit in routes[i].GetVisits())
                {
                    Console.Write(visit.GetId());
                    Console.Write(" -> ");
                }
                Console.WriteLine("Depot.");
            }
        }

        public int GetAmountVisits()
        {
            return problem.GetAmountVisits();
        }

        public List<Visit> GetVisits()
        {
            return allVisits;
        }

        /*
        public List<Visit> GetRemoved()
        {
            return removed;
        }

        public void SetRemoved(List<Visit> removed)
        {
            this.removed = removed;
        }
        */

        public bool TimeWindowCheck(Visit visit, (int, int) position)
        {
            // Get route and visits where visit is tested
            Route route = routes[position.Item1];
            List<Visit> visits = route.GetVisits();

            // Get previous visit (check if it is the depot)
            //Visit prevVisit = problem.GetDepot();
            int prevWindowStart = 0;
            //int prevWindowEnd = problem.GetMaxServiceTime();
            int prevDropTime = 0;
            if (position.Item2 > 0)
            {
                //prevVisit = visits[position.Item2 - 1];
                prevWindowStart = visits[position.Item2 - 1].GetWindowStart();
                //prevWindowEnd = visits[position.Item2 - 1].GetWindowEnd();
                prevDropTime = visits[position.Item2 - 1].GetDropTime();
            }

            // Get next visit (check if it is the depot)
            //Visit nextVisit = problem.GetDepot();
            //int nextWindowStart = 0;
            int nextWindowEnd = problem.GetMaxServiceTime();
            if (position.Item2 < visits.Count)
            {
                //nextVisit = visits[position.Item2];
                //nextWindowStart = visits[position.Item2].GetWindowStart();
                nextWindowEnd = visits[position.Item2].GetWindowEnd();
            }

            // Get begin and end of window for visit
            int beginWindow = visit.GetWindowStart();
            int endWindow = visit.GetWindowEnd();
            int dropTime = visit.GetDropTime();

            // Check if begin window is after begin previous window + droptime previouw visit 
            if (beginWindow < prevWindowStart + prevDropTime)
            {
                return false;
            }

            // Check if end window is before end next window + droptime next visit
            if (endWindow + dropTime > nextWindowEnd)
            {
                return false;
            }

            return true;
        }

        public bool CapacityCheck(Visit visit, int route)
        {
            // TODO: FIX CURRENT CAPACITY
            /*
            int currentCapacity = routes[route].GetRemainingCapacity();
            if (currentCapacity - visit.GetDemand() > 0)
            {
                return true;
            }
            return false;
            */
            return true;
        }
    }
}
