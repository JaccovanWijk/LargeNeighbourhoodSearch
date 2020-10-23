using System;
using System.Collections.Generic;
using System.Text;

namespace LNS
{
    class Problem // VrpProblem
    {
        int capacity;
        List<VisitDistance> visitDistances = new List<VisitDistance>();
        VisitDistance distancesFromDepot;
        Visit depot;

        List<Visit> visits;

        double maxDistance = 0;
        int maxServiceTime;

        public Problem(List<Visit> visits, int depotX, int depotY, int capacity, int maxServiceTime)
        {
            this.visits = visits;
            this.capacity = capacity;
            this.depot = new Visit(depotX, depotY);
            this.maxServiceTime = maxServiceTime;

            findDistances();
        }

        private void findDistances()
        {
            List<double> depotDistances = new List<double>();
            foreach (Visit visit in visits)
            {
                // Get x and y for visit
                int xCoordinate = visit.GetX();
                int yCoordinate = visit.GetY();

                // Set distance to depot for visit
                int xDifferenceDepot = xCoordinate - depot.GetX();
                int yDifferenceDepot = yCoordinate - depot.GetY();
                depotDistances.Add(Math.Sqrt(xDifferenceDepot * xDifferenceDepot + yDifferenceDepot * yDifferenceDepot));

                // Set distance to other visits for current visit
                List<double> distances = new List<double>();
                foreach (Visit otherVisit in visits)
                {
                    int xDifferenceVisits = xCoordinate - otherVisit.GetX();
                    int yDifferenceVisits = yCoordinate - otherVisit.GetY();
                    double distance = Math.Sqrt(xDifferenceVisits * xDifferenceVisits + yDifferenceVisits * yDifferenceVisits);
                    distances.Add(distance);

                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                    }
                }
                visitDistances.Add(new VisitDistance(visit, visits, distances));
            }
            distancesFromDepot = new VisitDistance(depot, visits, depotDistances);
        }

        public int GetAmountVisits()
        {
            return visits.Count;
        }

        public List<Visit> GetVisits()
        {
            return visits;
        }
        
        public double GetVisitDistance(Visit visit1, Visit visit2)
        {
            // Get distances of visit1
            int indexVisit1 = visits.IndexOf(visit1);
            VisitDistance visit1Distances = visitDistances[indexVisit1];

            // Find visit2 in distances visit1
            return visit1Distances.GetDistance(visit2);
        }

        public double GetDistanceFromDepot(Visit visit)
        {
            return distancesFromDepot.GetDistance(visit);
        }

        public double GetMaxDistance()
        {
            return maxDistance;
        }

        public int GetCapacity()
        {
            return capacity;
        }
        
    }
}
