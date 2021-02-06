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
        List<int> bestEachIteration;
        List<int> capacityEachIteration;
        List<int> routesEachIteration;
        List<int> maxDistanceEachIteration;
        List<int> AverageDeviationEachIteration;
        List<int> meanEachIteration;
        List<int> costEachIteration;
        List<int> toRemoveEachIteration;
        int totalRemoved;

        List<List<int>> wereInRouteTogether = new List<List<int>>();

        int iterations = 0;
        //List<string> text = new List<string>();

        public LargeNeighbourhoodSearch(RoutingPlan plan, int maxIterations, int discrepancies,
            int attempts, int determinism)
        {
            this.bestPlan = plan;
            this.bestPlanCost = bestPlan.CalculateCost();
            this.maxIterations = maxIterations;
            this.discrepancies = discrepancies;
            this.attempts = attempts;
            this.determinism = determinism;


            
            for (int i = 0; i < 100; i++)
            {
                List<int> row = new List<int>();
                for (int j = 0; j < 100; j++)
                {
                    row.Add(0);
                }
                wereInRouteTogether.Add(row);
            }
        }


        public RoutingPlan Search()
        {
            currentAttempts = 0;
            bestEachIteration = new List<int>();
            
            capacityEachIteration = new List<int>();
            routesEachIteration = new List<int>();
            maxDistanceEachIteration = new List<int>();
            AverageDeviationEachIteration = new List<int>();
            meanEachIteration = new List<int>();
            costEachIteration = new List<int> { (int)(bestPlan.CalculateCost() * 100) };
            toRemoveEachIteration = new List<int>();
            totalRemoved = 0;
            
            for (int i = 0; i < maxIterations; i++) {
            
            //while (bestPlan.GetAmountOfRoutes() > 10) {
                        
                

                // Check if currentAttempts >= attempts
                if (currentAttempts >= attempts)
                {
                    if (toRemove < 100)
                    {
                        toRemove++;
                    }
                    currentAttempts = 0;
                }

                double currentCost = bestPlan.CalculateCost();
                iterations = 0;

                // Copy bestPlan
                RoutingPlan newPlan = new RoutingPlan(bestPlan.CopyRoutes(), bestPlan.GetProblem());
                RoutingPlan copyPlan = new RoutingPlan(bestPlan.CopyRoutes(), bestPlan.GetProblem());

                List<Visit> removed = RemoveVisits(newPlan);
                totalRemoved += toRemove;

                // copy removed
                List<Visit> removedCopy = new List<Visit>();
                foreach (Visit visit1 in removed)
                {
                    removedCopy.Add(visit1);
                }

                Reinsert(newPlan, removed, discrepancies);

                if (bestPlan.CalculateCost() < currentCost)
                {
                    currentAttempts = 0;

                    // Add to weretogether
                    foreach (Route route in copyPlan.GetRoutes())
                    {
                        foreach(Visit visit in removedCopy)
                        {
                            if (route.GetIds().Contains(visit.GetId()))
                            {
                                foreach (Visit v in route.GetVisits())
                                {
                                    if (v.GetId() != visit.GetId())
                                    {
                                        wereInRouteTogether[visit.GetId() - 1][v.GetId() - 1]++;
                                    }
                                }
                            }
                        }
                    }

                }
                else
                {
                    currentAttempts++;
                }

                bestEachIteration.Add(bestPlan.GetAmountOfRoutes());
                
                capacityEachIteration.Add(bestPlan.GetPercentageCapacity());
                routesEachIteration.Add(bestPlan.GetAmountOfRoutes());
                maxDistanceEachIteration.Add(bestPlan.GetLongestNormDistance());
                AverageDeviationEachIteration.Add(bestPlan.GetAverageDeviationRouteLength());
                meanEachIteration.Add((int)(bestPlan.GetAverageRouteLength() * 100));
                costEachIteration.Add((int)(bestPlan.CalculateCost() * 100));
                toRemoveEachIteration.Add(toRemove);
                
            }

            //Console.WriteLine(totalRemoved);

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

                if (ranked.Count > 0)
                {

                    // Get random double between 0 and 1
                    double random = rand.NextDouble();

                    // Get slightly random element from ranked list and remove it
                    Visit newRemoved = ranked[Convert.ToInt32((ranked.Count - 1) * Math.Pow(random, determinism))];

                    //plan.RemoveVisit(newRemoved);
                    removedVisits.Add(newRemoved);
                }
            }

            return removedVisits;
        }

        
        public void Reinsert(RoutingPlan plan, List<Visit> toInsert, int discrep)//List<Route> routes, List<Visit> toInsert, int discrep)
        {
            iterations++;

            if (toInsert.Count == 0)
            {
                double newCost = plan.CalculateCost();
                if (newCost < bestPlanCost)
                {
                    bestPlan = plan;
                    bestPlanCost = newCost;
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

        public List<int> GetBestEachIteration()
        {
            return bestEachIteration;
        }

        public List<int> GetToRemoveEachIteration()
        {
            return toRemoveEachIteration;
        }

        public List<int> GetCostEachIteration()
        {
            return costEachIteration;
        }

        public List<int> GetCapacityEachIteration()
        {
            return capacityEachIteration;
        }
        
        public List<int> GetRoutesEachIteration()
        {
            return routesEachIteration;
        }

        public List<int> GetMaxDistanceEachIteration()
        {
            return maxDistanceEachIteration;
        }

        public List<List<int>> GetRoutesTogether()
        {
            return wereInRouteTogether;
        }

        public int GetTotalRemoved()
        {
            return totalRemoved;
        }

        public List<int> GetAverageDeviationRouteLength()
        {
            return AverageDeviationEachIteration;
        }

        public List<int> GetMeanRouteLength()
        {
            return meanEachIteration;
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
