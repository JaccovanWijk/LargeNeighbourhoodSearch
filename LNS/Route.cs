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
            
            //clone.visits = visits; WERKT NIET?

            return clone;
        }

        public void AddVisit(Visit visit, int position)
        {
            if (position <= visits.Count)
            {
                visits.Insert(position, visit);
            }
            else
            {
                visits.Add(visit);
            }
        }

        public void RemoveVisit(Visit visit)
        {
            //visits.Remove(visit);
            foreach (Visit visit1 in visits)
            {
                if (visit.GetId() == visit1.GetId())
                {
                    visits.Remove(visit1);
                    break;
                }
            }
        }

        public List<Visit> GetVisits()
        {
            return visits;
        }

        public int GetAmountVisits()
        {
            return visits.Count;
        }

        public int GetRemainingCapacity()
        {
            remainingCapacity = maxCapacity;
            foreach(Visit visit in visits)
            {
                remainingCapacity -= visit.GetDemand();
            }
            return remainingCapacity;
        }

        public bool Contains(Visit visit)
        {
            //return (visits.Contains(visit));

            foreach (Visit visit1 in visits)
            {
                if (visit.Equals(visit1))
                {
                    return true;
                }
            }
            return false;
        }

        public bool Equals(Route route)
        {
            if (route.GetAmountVisits() != visits.Count)
            {
                return false;
            }

            List<Visit> otherVisits = route.GetVisits();
            for (int i = 0; i < visits.Count; i++)
            {
                if (!visits[i].Equals(otherVisits[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
