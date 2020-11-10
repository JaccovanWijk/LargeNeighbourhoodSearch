using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LNS
{
    class ReadProblem
    {
        List<string> lines;
        Problem problem;

        public ReadProblem(string name) {
            string[] linesArray = System.IO.File.ReadAllLines(@"C:\Users\Jacco\source\repos\LNS\" + name);
            lines = new List<string>(linesArray);

            // Get capacity from first line
            int capacity = Int32.Parse(PopFirstLine());

            // Get depot from second line
            string line = PopFirstLine();
            string[] parts = SplitLine(line);
            int depotX = Int32.Parse(parts[1]);
            int depotY = Int32.Parse(parts[2]);
            int maxServiceTime = Int32.Parse(parts[5]);

            // Get info on visits
            List<Visit> allVisits = new List<Visit>();
            foreach (string newLine in lines)//.GetRange(0, 100))
            {
                string[] newParts = SplitLine(newLine);
                int id = Int32.Parse(newParts[0]);
                int xCoordinate = Int32.Parse(newParts[1]);
                int yCoordinate = Int32.Parse(newParts[2]);
                int demand = Int32.Parse(newParts[3]);
                int windowStartTime = Int32.Parse(newParts[4]);
                int windowEndTime = Int32.Parse(newParts[5]);
                int serviceTime = Int32.Parse(newParts[6]);

                allVisits.Add(new Visit(id, demand, windowStartTime, windowEndTime, xCoordinate, yCoordinate, serviceTime));
            }

            // Create problem class
            problem = new Problem(allVisits, depotX, depotY, capacity, maxServiceTime);

        }

        public string PopFirstLine()
        {
            string line = lines[0];
            lines.RemoveAt(0);
            return line;
        }

        public string[] SplitLine(string line)
        {
            // Return string array of characters (remove fist which is empty)
            string[] splitLine = System.Text.RegularExpressions.Regex.Split(line, @"\s+");
            return splitLine.Skip(1).ToArray();
        }

        public Problem GetProblem()
        {
            return problem;
        }
    }
}
