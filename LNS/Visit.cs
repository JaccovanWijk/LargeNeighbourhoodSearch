using System;
using System.Collections.Generic;
using System.Text;

namespace LNS
{
    class Visit //RouteNode
    {
        int id;
        int demand;
        int windowStartTime;
        int windowEndTime;
        int xCoordinate;
        int yCoordinate;
        int dropTime;
        //int route; ????????????????????

        public Visit(int id, int demand, int windowStartTime, int windowEndTime, int xCoordinate, int yCoordinate, int dropTime)
        {
            this.id = id;
            this.demand = demand;
            this.windowStartTime = windowStartTime;
            this.windowEndTime = windowEndTime;
            this.xCoordinate = xCoordinate;
            this.yCoordinate = yCoordinate;
            this.dropTime = dropTime;
        }

        // Depot
        public Visit(int xCoordinate, int yCoordinate)
        {
            this.xCoordinate = xCoordinate;
            this.yCoordinate = yCoordinate;
        }

        public int GetId()
        {
            return id;
        }

        public int GetDemand()
        {
            return demand;
        }

        public int GetWindowStart()
        {
            return windowStartTime;
        }

        public int GetWindowEnd()
        {
            return windowEndTime;
        }

        public int GetX()
        {
            return xCoordinate;
        }

        public int GetY()
        {
            return yCoordinate;
        }

        public int GetDropTime()
        {
            return dropTime;
        }
    }
}
