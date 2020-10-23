using System;
using System.Collections.Generic;
using System.Text;

namespace LNS
{
    class Route
    {
        List<Visit> visits;
        int remainingCapacity;

        public Route(List<Visit> visits, int capacity)
        {
            this.visits = visits;
            this.remainingCapacity = capacity;

            foreach (Visit visit in visits)
            {
                remainingCapacity -= visit.GetDemand();
            }
        }

        public void AddVisit(Visit visit, int position)
        {
            visits.Insert(position, visit);
        }

        public void RemoveVisit(Visit visit)
        {
            visits.Remove(visit);
        }

        public List<Visit> GetVisits()
        {
            return visits;
        }

        public int GetRemainingCapacity()
        {
            return remainingCapacity;
        }

        public bool Contains(Visit visit)
        {
            return (visits.Contains(visit));
        }
    }
}
