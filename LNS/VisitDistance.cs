using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LNS
{
    class VisitDistance
    {
        int currentVisitId;
        List<int> otherVisitsIds;
        List<double> distances;

        public VisitDistance(int currentVisit, List<int> otherVisits, List<double> distances)
        {
            this.currentVisitId = currentVisit;
            this.otherVisitsIds = otherVisits;
            this.distances = distances;
        }

        public int GetVisitId()
        {
            return currentVisitId;
        }

        public double GetDistance(Visit otherVisit)
        {
            // Get right visit (reference is changed for copies)
            int indexVisit = otherVisitsIds.IndexOf(otherVisit.GetId());
            return distances[indexVisit];
        }
    }
}
