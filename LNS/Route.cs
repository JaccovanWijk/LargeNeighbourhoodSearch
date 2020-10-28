using System;
using System.Collections.Generic;
using System.Text;

namespace LNS
{
    class Route
    {
        public List<Visit> visits;
        public int maxCapacity;
        public int remainingCapacity;

        public Route(List<Visit> visits, int capacity)
        {
            this.visits = visits;
            this.maxCapacity = capacity;
            this.remainingCapacity = capacity;

            foreach (Visit visit in visits)
            {
                remainingCapacity -= visit.GetDemand();
            }
        }

        public Route Clone()
        {
            Route clone = (Route)this.MemberwiseClone();
            List<Visit> visitClone = new List<Visit>();
            foreach (Visit visit in clone.visits)
            {
                visitClone.Add(visit.Clone());
            }
            clone.visits = visitClone;

            return clone;
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
