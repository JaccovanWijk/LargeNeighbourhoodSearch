using System;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LNS
{
    class Program
    {

        static void Main(string[] args)
        {
            List<List<int>> costEachIteration = new List<List<int>>();

            PrintPlan printer = new PrintPlan();

            #region files variable
            /*
            List<string> files = new List<string> { "R101.txt", "R101.txt", "R101.txt", "R101.txt", "R101.txt", "R101.txt", "R101.txt", "R101.txt", "R101.txt", "R101.txt",
                "R102.txt", "R102.txt", "R102.txt", "R102.txt", "R102.txt", "R102.txt", "R102.txt", "R102.txt", "R102.txt", "R102.txt",
                "R103.txt", "R103.txt", "R103.txt", "R103.txt", "R103.txt", "R103.txt", "R103.txt", "R103.txt", "R103.txt", "R103.txt",
                "R104.txt", "R104.txt", "R104.txt", "R104.txt", "R104.txt", "R104.txt", "R104.txt", "R104.txt", "R104.txt", "R104.txt",
                "R105.txt", "R105.txt", "R105.txt", "R105.txt", "R105.txt", "R105.txt", "R105.txt", "R105.txt", "R105.txt", "R105.txt",
                "R106.txt", "R106.txt", "R106.txt", "R106.txt", "R106.txt", "R106.txt", "R106.txt", "R106.txt", "R106.txt", "R106.txt",
                "R107.txt", "R107.txt", "R107.txt", "R107.txt", "R107.txt", "R107.txt", "R107.txt", "R107.txt", "R107.txt", "R107.txt",
                "R108.txt", "R108.txt", "R108.txt", "R108.txt", "R108.txt", "R108.txt", "R108.txt", "R108.txt", "R108.txt", "R108.txt",
                "R109.txt", "R109.txt", "R109.txt", "R109.txt", "R109.txt", "R109.txt", "R109.txt", "R109.txt", "R109.txt", "R109.txt",
                "R110.txt", "R110.txt", "R110.txt", "R110.txt", "R110.txt", "R110.txt", "R110.txt", "R110.txt", "R110.txt", "R110.txt",
                "R111.txt", "R111.txt", "R111.txt", "R111.txt", "R111.txt", "R111.txt", "R111.txt", "R111.txt", "R111.txt", "R111.txt",
                "R112.txt", "R112.txt", "R112.txt", "R112.txt", "R112.txt", "R112.txt", "R112.txt", "R112.txt", "R112.txt", "R112.txt",
                "C101.txt", "C101.txt", "C101.txt", "C101.txt", "C101.txt", "C101.txt", "C101.txt", "C101.txt", "C101.txt", "C101.txt",
                "C102.txt", "C102.txt", "C102.txt", "C102.txt", "C102.txt", "C102.txt", "C102.txt", "C102.txt", "C102.txt", "C102.txt",
                "C103.txt", "C103.txt", "C103.txt", "C103.txt", "C103.txt", "C103.txt", "C103.txt", "C103.txt", "C103.txt", "C103.txt",
                "C104.txt", "C104.txt", "C104.txt", "C104.txt", "C104.txt", "C104.txt", "C104.txt", "C104.txt", "C104.txt", "C104.txt",
                "C105.txt", "C105.txt", "C105.txt", "C105.txt", "C105.txt", "C105.txt", "C105.txt", "C105.txt", "C105.txt", "C105.txt",
                "C106.txt", "C106.txt", "C106.txt", "C106.txt", "C106.txt", "C106.txt", "C106.txt", "C106.txt", "C106.txt", "C106.txt",
                "C107.txt", "C107.txt", "C107.txt", "C107.txt", "C107.txt", "C107.txt", "C107.txt", "C107.txt", "C107.txt", "C107.txt",
                "C108.txt", "C108.txt", "C108.txt", "C108.txt", "C108.txt", "C108.txt", "C108.txt", "C108.txt", "C108.txt", "C108.txt",
                "C109.txt", "C109.txt", "C109.txt", "C109.txt", "C109.txt", "C109.txt", "C109.txt", "C109.txt", "C109.txt", "C109.txt",
                "RC101.txt", "RC101.txt", "RC101.txt", "RC101.txt", "RC101.txt", "RC101.txt", "RC101.txt", "RC101.txt", "RC101.txt", "RC101.txt",
                "RC102.txt", "RC102.txt", "RC102.txt", "RC102.txt", "RC102.txt", "RC102.txt", "RC102.txt", "RC102.txt", "RC102.txt", "RC102.txt",
                "RC103.txt", "RC103.txt", "RC103.txt", "RC103.txt", "RC103.txt", "RC103.txt", "RC103.txt", "RC103.txt", "RC103.txt", "RC103.txt",
                "RC104.txt", "RC104.txt", "RC104.txt", "RC104.txt", "RC104.txt", "RC104.txt", "RC104.txt", "RC104.txt", "RC104.txt", "RC104.txt",
                "RC105.txt", "RC105.txt", "RC105.txt", "RC105.txt", "RC105.txt", "RC105.txt", "RC105.txt", "RC105.txt", "RC105.txt", "RC105.txt",
                "RC106.txt", "RC106.txt", "RC106.txt", "RC106.txt", "RC106.txt", "RC106.txt", "RC106.txt", "RC106.txt", "RC106.txt", "RC106.txt",
                "RC107.txt", "RC107.txt", "RC107.txt", "RC107.txt", "RC107.txt", "RC107.txt", "RC107.txt", "RC107.txt", "RC107.txt", "RC107.txt",
                "RC108.txt", "RC108.txt", "RC108.txt", "RC108.txt", "RC108.txt", "RC108.txt", "RC108.txt", "RC108.txt", "RC108.txt", "RC108.txt",
                "R201.txt", "R201.txt", "R201.txt", "R201.txt", "R201.txt", "R201.txt", "R201.txt", "R201.txt", "R201.txt", "R201.txt",
                "R202.txt", "R202.txt", "R202.txt", "R202.txt", "R202.txt", "R202.txt", "R202.txt", "R202.txt", "R202.txt", "R202.txt",
                "R203.txt", "R203.txt", "R203.txt", "R203.txt", "R203.txt", "R203.txt", "R203.txt", "R203.txt", "R203.txt", "R203.txt",
                "R204.txt", "R204.txt", "R204.txt", "R204.txt", "R204.txt", "R204.txt", "R204.txt", "R204.txt", "R204.txt", "R204.txt",
                "R205.txt", "R205.txt", "R205.txt", "R205.txt", "R205.txt", "R205.txt", "R205.txt", "R205.txt", "R205.txt", "R205.txt",
                "R206.txt", "R206.txt", "R206.txt", "R206.txt", "R206.txt", "R206.txt", "R206.txt", "R206.txt", "R206.txt", "R206.txt",
                "R207.txt", "R207.txt", "R207.txt", "R207.txt", "R207.txt", "R207.txt", "R207.txt", "R207.txt", "R207.txt", "R207.txt",
                "R208.txt", "R208.txt", "R208.txt", "R208.txt", "R208.txt", "R208.txt", "R208.txt", "R208.txt", "R208.txt", "R208.txt",
                "R209.txt", "R209.txt", "R209.txt", "R209.txt", "R209.txt", "R209.txt", "R209.txt", "R209.txt", "R209.txt", "R209.txt",
                "R210.txt", "R210.txt", "R210.txt", "R210.txt", "R210.txt", "R210.txt", "R210.txt", "R210.txt", "R210.txt", "R210.txt",
                "R211.txt", "R211.txt", "R211.txt", "R211.txt", "R211.txt", "R211.txt", "R211.txt", "R211.txt", "R211.txt", "R211.txt",
                "C201.txt", "C201.txt", "C201.txt", "C201.txt", "C201.txt", "C201.txt", "C201.txt", "C201.txt", "C201.txt", "C201.txt",
                "C202.txt", "C202.txt", "C202.txt", "C202.txt", "C202.txt", "C202.txt", "C202.txt", "C202.txt", "C202.txt", "C202.txt",
                "C203.txt", "C203.txt", "C203.txt", "C203.txt", "C203.txt", "C203.txt", "C203.txt", "C203.txt", "C203.txt", "C203.txt",
                "C204.txt", "C204.txt", "C204.txt", "C204.txt", "C204.txt", "C204.txt", "C204.txt", "C204.txt", "C204.txt", "C204.txt",
                "C205.txt", "C205.txt", "C205.txt", "C205.txt", "C205.txt", "C205.txt", "C205.txt", "C205.txt", "C205.txt", "C205.txt",
                "C206.txt", "C206.txt", "C206.txt", "C206.txt", "C206.txt", "C206.txt", "C206.txt", "C206.txt", "C206.txt", "C206.txt",
                "C207.txt", "C207.txt", "C207.txt", "C207.txt", "C207.txt", "C207.txt", "C207.txt", "C207.txt", "C207.txt", "C207.txt",
                "C208.txt", "C208.txt", "C208.txt", "C208.txt", "C208.txt", "C208.txt", "C208.txt", "C208.txt", "C208.txt", "C208.txt",
                "RC201.txt", "RC201.txt", "RC201.txt", "RC201.txt", "RC201.txt", "RC201.txt", "RC201.txt", "RC201.txt", "RC201.txt", "RC201.txt",
                "RC202.txt", "RC202.txt", "RC202.txt", "RC202.txt", "RC202.txt", "RC202.txt", "RC202.txt", "RC202.txt", "RC202.txt", "RC202.txt",
                "RC203.txt", "RC203.txt", "RC203.txt", "RC203.txt", "RC203.txt", "RC203.txt", "RC203.txt", "RC203.txt", "RC203.txt", "RC203.txt",
                "RC204.txt", "RC204.txt", "RC204.txt", "RC204.txt", "RC204.txt", "RC204.txt", "RC204.txt", "RC204.txt", "RC204.txt", "RC204.txt",
                "RC205.txt", "RC205.txt", "RC205.txt", "RC205.txt", "RC205.txt", "RC205.txt", "RC205.txt", "RC205.txt", "RC205.txt", "RC205.txt",
                "RC206.txt", "RC206.txt", "RC206.txt", "RC206.txt", "RC206.txt", "RC206.txt", "RC206.txt", "RC206.txt", "RC206.txt", "RC206.txt",
                "RC207.txt", "RC207.txt", "RC207.txt", "RC207.txt", "RC207.txt", "RC207.txt", "RC207.txt", "RC207.txt", "RC207.txt", "RC207.txt",
                "RC208.txt", "RC208.txt", "RC208.txt", "RC208.txt", "RC208.txt", "RC208.txt", "RC208.txt", "RC208.txt", "RC208.txt", "RC208.txt" };
            */
            #endregion
            List<string> files = new List<string> { "RC101Split2.txt", "RC101Split2.txt", "RC101Split2.txt", "RC101Split2.txt", "RC101Split2.txt", "RC101Split2.txt", "RC101Split2.txt", "RC101Split2.txt", "RC101Split2.txt", "RC101Split2.txt"}; 

            List<List<List<int>>> allDinges = new List<List<List<int>>>();
            List<List<int>> allTotalRemoved = new List<List<int>>();

            int counter = 1;
            foreach (string file in files)
            {
                Console.WriteLine(file + "   " + counter + "/" + files.Count);
                counter++;
                
                // Get problem from text file
                ReadProblem reader = new ReadProblem(file);
                Problem problem = reader.GetProblem();

                // Initial routing plan with one route per visit
                List<Route> initialRoutes = new List<Route>();
                foreach (Visit visit in problem.GetVisits())
                {
                    initialRoutes.Add(new Route(new List<Visit> { visit }, problem.GetCapacity()));
                }
                RoutingPlan initialPlan = new RoutingPlan(initialRoutes, problem);

                int maxIterations = 2000;
                int discrepancies = 5; // discrep
                int attempts = 250; // a
                int determinism = 10; // D

                LargeNeighbourhoodSearch LNS = new LargeNeighbourhoodSearch(initialPlan,
                    maxIterations, discrepancies, attempts, determinism);
                RoutingPlan plan = LNS.Search();

                costEachIteration.Add(LNS.GetCostEachIteration());

                allDinges.Add(LNS.GetRoutesTogether());
                allTotalRemoved.Add(new List<int> { LNS.GetTotalRemoved() });

                
                List<Route> routes = plan.GetRoutes();
                List<List<int>> solution = new List<List<int>>();
                for (int i = 0; i < 100; i++)
                {
                    List<int> row = new List<int>();
                    for (int j = 0; j < 100; j++)
                    {
                        row.Add(0);
                    }
                    solution.Add(row);
                }

                foreach (Route route in routes)
                {
                    List<int> ids = route.GetIds();

                    foreach (int id1 in ids)
                    {
                        foreach (int id2 in ids)
                        {
                            if (id1 != id2)
                            {
                                solution[id1 - 1][id2 - 1] += 1;
                            }
                        }
                    }
                }
                allDinges.Add(solution);
            }

            Console.WriteLine("Done");

            //printer.printXLS("test1.xlsx", costEachIteration, files);

            printer.PrintDeleted("EerdereRoutesSamen RC101Split2.xlsx", allDinges, files);
            printer.printXLS("Cost RC101Split2.xlsx", costEachIteration, files);
        }
    }
}
