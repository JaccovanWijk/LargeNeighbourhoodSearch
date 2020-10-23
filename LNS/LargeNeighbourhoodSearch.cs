using System;
using System.Collections.Generic;
using System.Text;

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
            for (int i = 0; i < maxIterations; i++)
            {
                RoutingPlan removedPlan = RemoveVisits(bestPlan); // roRemove and determinism are global
                Reinsert(removedPlan, discrepancies); // Visits is in removedPlan
            }

            return bestPlan;
        }

        public RoutingPlan RemoveVisits(RoutingPlan plan)
        {
            List<Visit> allVisits = plan.GetVisits();
            int randomIndex = rand.Next(allVisits.Count);
            plan.RemoveVisit(allVisits[randomIndex]); // Remove inplan, RemoveVisit(plan) and add to removed set
            while (plan.GetRemoved().Count < toRemove)
            {
                // Get random removed visit id
                List<Visit> removed = plan.GetRemoved();
                randomIndex = rand.Next(removed.Count);
                Visit randomVisit = removed[randomIndex];

                // Rank non-removed visits with relatedness
                List<Visit> ranked = plan.RankRelatedness(randomVisit, removed);

                // Get random double between 0 and 1
                double random = rand.Next();

                // Get slightly random element from ranked list and remove it
                Visit newRemoved = ranked[Convert.ToInt32(ranked.Count * Math.Pow(random, determinism))];
                plan.RemoveVisit(newRemoved);
            }

            return plan;
        }

        public void Reinsert(RoutingPlan plan, int discrep)
        {

            List<Visit> toInsert = plan.GetRemoved();


            if (toInsert.Count == 0)
            {
                // Check for new best plan
                plan.PrintRoutingPlan();

                double newCost = plan.CalculateCost();
                if (newCost < bestPlanCost)
                {
                    bestPlan = plan;
                    bestPlanCost = newCost;
                }

            else
            {
                Visit visit = plan.ChooseFarthest(toInsert);
                    
                int i = 0;
                foreach ((int, int) position in plan.RankedPositions(visit))
                {
                    if (i <= discrep)
                    {
                        // TODO: HOW TO STORE AND RETRIEVE WITHOUT MUCH COST
                        // TODO: KEEP TRACK OF REMOVED
                        plan.PlaceVisit(visit, position);
                        Reinsert(plan, discrep - i);
                        plan.RemoveVisit(visit);
                        i++;

                        //RoutingPlan testPlan = TestInsertion(visit, position);
                        //Reinsert(testPlan, discrep - i);
                        //i++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            }
        }
    }
}
