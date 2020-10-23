using System;
using System.Collections.Generic;
using System.Text;

namespace LNS
{
    class VisitDistance
    {
        Visit currentVisit;
        List<Visit> otherVisits;
        List<double> distances;

        public VisitDistance(Visit currentVisit, List<Visit> otherVisits, List<double> distances)
        {
            this.currentVisit = currentVisit;
            this.otherVisits = otherVisits;
            this.distances = distances;
        }

        public Visit GetVisit()
        {
            return currentVisit;
        }

        public double GetDistance(Visit otherVisit)
        {
            int indexVisit = otherVisits.IndexOf(otherVisit);
            return distances[indexVisit];
        }
    }
}
