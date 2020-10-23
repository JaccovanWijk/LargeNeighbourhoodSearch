using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LNS
{
    class RoutingPlan // VrpSolution
    {
        List<Route> routes;
        List<Visit> removed = new List<Visit>();
        int vehicles;
        Problem problem;
        double planCost;
        List<Visit> allVisits;

        public RoutingPlan(List<Route> routes, Problem problem)
        {
            this.routes = routes;
            this.problem = problem;
            this.vehicles = routes.Count;

            allVisits = new List<Visit>();
            foreach (Route route in routes)
            {
                foreach (Visit visit in route.GetVisits())
                {
                    allVisits.Add(visit);
                }
            }
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
        
        public List<Visit> RankRelatedness(Visit visit, List<Visit> removed)
        {
            // Rank them by relatedness
            List<Visit> related = allVisits.OrderByDescending(o => Relatedness(visit, o)).ToList();

            // Exclude removed visits
            foreach (Visit newVisit in related)
            {
                if (removed.Contains(newVisit))
                {
                    related.Remove(visit);
                }
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
            // CHECK FOR PLACES WHERE TIME WINDOW ARE NOT VIOLATED, MAXSERVICETIME IS NOT VIOLATED, CALCULATE POSiTION WITH MINIMUM COST

            // Add all positions without filter
            List<(int, int)> positions = new List<(int, int)>();
            for (int i = 0; i < routes.Count; i++)
            {
                for (int j = 0; j < routes[i].GetVisits().Count; j++)
                {
                    positions.Add((i, j));
                }
            }

            /*
            Route bestRoute;
            int location;
            double currentCost = CalculateCost();
            foreach (Route route in routes)
            {
                // Calculate cost for visit in first place route
                location = 0;
                currentCost 
            }
            */

            return positions;
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

        public List<Visit> GetRemoved()
        {
            return removed;
        }

        public void SetRemoved(List<Visit> removed)
        {
            this.removed = removed;
        }
    }
}
