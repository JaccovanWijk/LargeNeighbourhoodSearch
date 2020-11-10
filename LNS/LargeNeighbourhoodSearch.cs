using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LNS
{
    class LargeNeighbourhoodSearch 
    {
        RoutingPlan bestPlan;
        double bestPlanCost;
        int maxIterations;
        int discrepancies;
        int attempts;
        int determinism;
        int toRemove = 1;
        Random rand = new Random();

        int currentAttempts;

        int iterations = 0;
        List<string> text = new List<string>();

        public LargeNeighbourhoodSearch(RoutingPlan plan, int maxIterations, int discrepancies,
            int attempts, int determinism)
        {
            this.bestPlan = plan;
            this.bestPlanCost = bestPlan.CalculateCost();
            this.maxIterations = maxIterations;
            this.discrepancies = discrepancies;
            this.attempts = attempts;
            this.determinism = determinism;
        }


        public RoutingPlan Search()
        {
            currentAttempts = 0;
            /*
            for (int i = 0; i < maxIterations; i++)
            {
            */
            while (bestPlan.GetAmountOfRoutes() > 28) {
                        
                

                // Check if currentAttempts >= attempts
                if (currentAttempts >= attempts)
                {
                    toRemove++;
                    currentAttempts = 0;
                }

                double currentCost = bestPlan.CalculateCost();
                iterations = 0;

                // Copy bestPlan
                RoutingPlan newPlan = new RoutingPlan(bestPlan.CopyRoutes(), bestPlan.GetProblem());

                List<Visit> removed = RemoveVisits(newPlan);
                Reinsert(newPlan, removed, discrepancies);

                if (bestPlan.CalculateCost() < currentCost)
                {
                    currentAttempts = 0;
                }
                else
                {
                    currentAttempts++;
                }
            }

            /*
            foreach (Route route in bestPlan.GetRoutes())
            {
                Console.WriteLine(route.GetVisits().Count);
            }
            */

            System.IO.File.WriteAllLines(@"C:\Users\Jacco\source\repos\LNS\LNS\WriteLines.txt", text);

            /*
            Problem problem = bestPlan.GetProblem();
            Route route = bestPlan.GetRoutes()[4];
            List<Visit> visits = route.GetVisits();

            Console.WriteLine(problem.GetDistanceFromDepot(route.GetVisits()[0]));
            Console.WriteLine(visits[0].GetWindowStart() + " <-> " + visits[0].GetWindowEnd());

            for (int i = 1; i < route.GetVisits().Count - 1; i++) 
            {
                Console.WriteLine(problem.GetVisitDistance(visits[i-1], visits[i]));
                Console.WriteLine(visits[i].GetWindowStart() + " <-> " + visits[i].GetWindowEnd());
            }

            Console.WriteLine(problem.GetDistanceFromDepot(visits[visits.Count - 1]));
            */
            return bestPlan;
        }

        public List<Visit> RemoveVisits(RoutingPlan plan)
        {
            List<Visit> allVisits = plan.GetVisits();
            int randomIndex = rand.Next(allVisits.Count);
            //plan.RemoveVisit(allVisits[randomIndex]); // Remove inplan, RemoveVisit(plan) and add to removed set

            // Create list with only the removed visit
            List<Visit> removedVisits = new List<Visit> { allVisits[randomIndex] };

            while (removedVisits.Count < toRemove)
            {
                // Get random removed visit id
                //List<Visit> removed = plan.GetRemoved();
                randomIndex = rand.Next(removedVisits.Count);
                Visit randomVisit = removedVisits[randomIndex];

                // Rank non-removed visits with relatedness
                List<Visit> ranked = plan.RankRelatedness(randomVisit, removedVisits);

                // Get random double between 0 and 1
                double random = rand.NextDouble();

                // Get slightly random element from ranked list and remove it
                Visit newRemoved = ranked[Convert.ToInt32((ranked.Count-1) * Math.Pow(random, determinism))];
                //plan.RemoveVisit(newRemoved);
                removedVisits.Add(newRemoved);
            }

            return removedVisits;
        }

        
        public void Reinsert(RoutingPlan plan, List<Visit> toInsert, int discrep)//List<Route> routes, List<Visit> toInsert, int discrep)
        {
            //List<Visit> toInsert = plan.GetRemoved();

            /*
            Console.WriteLine(1);
            bestPlan.PrintRoutingPlan();

            Console.WriteLine(2);
            plan.Tester(toInsert[0], (50, 1));
            plan.PrintRoutingPlan();

            Console.WriteLine(3);
            bestPlan.PrintRoutingPlan();
            */

            // WORKING FOR TESTER, BUT CODE UNDERNEATH DOESNT GET THE RIGHT ROUTES AND NEVER DELETES!

            iterations++;

            //Console.WriteLine(toInsert.Count);
            if (toInsert.Count == 0)
            {
                // Check for new best plan
                // TODO: GET COST WHEN CALCULATING RANKEDPOSITIONS????y
                double newCost = plan.CalculateCost();
                if (newCost < bestPlanCost)
                {
                    //plan.PrintRoutingPlan();
                    bestPlan = plan;
                    bestPlanCost = newCost;

                    // Print aantal iteraties/stappen
                    text.Add((bestPlan.GetAmountOfRoutes()).ToString() + "   " + iterations + "   " + toRemove + "   " + bestPlan.GetAmountVisits() / bestPlan.GetAmountOfRoutes() + "   " + bestPlan.GetAverageRouteLength());
                    Console.WriteLine("Current amount of routes: " + bestPlan.GetAmountOfRoutes());
                    Console.WriteLine("Iterations: " + iterations);
                    Console.WriteLine("toRemove: " + toRemove);
                    Console.WriteLine("Visits / Amount of routes: " + (double) bestPlan.GetAmountVisits() / bestPlan.GetAmountOfRoutes());
                    Console.WriteLine("Average Route length: " + (double) bestPlan.GetAverageRouteLength());
                    Console.WriteLine();
                    
                }
                else
                {
                    //currentAttempts++;
                }
            }
            else
            {
                Visit visit = plan.ChooseFarthest(toInsert);
                toInsert.Remove(visit);

                int i = 0;
                foreach ((int, int) position in plan.RankedPositions(visit))
                {
                    if (i <= discrep)
                    {
                        // Copy bestPlan
                        RoutingPlan newPlan = new RoutingPlan(plan.CopyRoutes(), plan.GetProblem());
                        //Console.WriteLine("derdst" + toInsert.Count);
                        // Make new plan and call reinsert
                        newPlan.PlaceVisit(visit, position);
                        //Console.WriteLine("viertst" + toInsert.Count);
                        Reinsert(newPlan, toInsert, discrep - i);
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }
                
            }
        }
        

        public void PrintRoutingPlan(List<Route> routes)
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

    }
}
