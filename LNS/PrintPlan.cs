using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using IronXL;

namespace LNS
{
    class PrintPlan
    {
        public void Draw(RoutingPlan plan)
        {
            // FIND MAX DISTANCE
            (int, int, int, int) extrema = plan.GetExtrema();
            int maxX = extrema.Item3; //- extrema.Item1;
            int maxY = extrema.Item4; //- extrema.Item2;

            Random rand = new Random();
            using (var bmp = new Bitmap(maxX * 5, maxY * 5))
            using (var gfx = Graphics.FromImage(bmp))
            using (var pen = new Pen(Color.Black))
            {
                pen.Width = 3.0F;
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                gfx.Clear(Color.White);


                Visit lastVisit = plan.GetProblem().GetDepot();
                int depotX = lastVisit.GetX() * 5;
                int depotY = lastVisit.GetY() * 5;

                // Draw depot
                Rectangle rect = new Rectangle(depotX - 3, depotY - 3, 6, 6);
                gfx.DrawEllipse(pen, rect);

                PointF point1;
                PointF point2;

                foreach (Route route in plan.GetRoutes())
                {
                    pen.Color = Color.FromArgb(rand.Next());

                    // Get visits and depot
                    List<Visit> visits = route.GetVisits();

                    // Start with depot
                    int lastX = depotX;
                    int lastY = depotY;

                    foreach (Visit visit in visits)
                    {
                        // Draw visit
                        int xVisit = visit.GetX() * 5;
                        int yVisit = visit.GetY() * 5;
                        rect = new Rectangle(xVisit - 3, yVisit - 3, 6, 6);
                        gfx.DrawEllipse(pen, rect);

                        // Draw line to visit
                        point1 = new PointF(lastX, lastY);
                        point2 = new PointF(xVisit, yVisit);
                        gfx.DrawLine(pen, point1, point2);

                        // Change last visit
                        lastX = xVisit;
                        lastY = yVisit;
                    }

                    // Draw line back to depot
                    point1 = new PointF(lastX, lastY);
                    point2 = new PointF(depotX, depotY);
                    gfx.DrawLine(pen, point1, point2);
                }

                bmp.Save("RouteVisualisation.png");
            }
        }

        public void printXLS(string filename, List<List<int>> allIterations, List<string> files)
        {
            WorkBook workbook = WorkBook.Load(filename);
            WorkSheet sheet = workbook.DefaultWorkSheet;

            for (int i = 0; i < allIterations.Count; i++)
            {
                sheet.SetCellValue(0, i, files[i]);
                for (int j = 1; j < allIterations[i].Count + 1; j++)
                {
                    sheet.SetCellValue(j, i, allIterations[i][j - 1]);
                }
            }
            //Save Changes
            workbook.SaveAs(filename);


            /*
            //iterate over range of cells
            foreach (var cell in range)
            {
                Console.WriteLine("Cell {0} has value '{1}", cell.RowIndex, cell.Value);
                if (cell.IsNumeric)
                {
                    //Get decimal value to avoid floating numbers precision issue
                    total += cell.DecimalValue;
                }
            }
            //check formula evaluation
            if (sheet["A11"].DecimalValue == total)
            {
                Console.WriteLine("Basic Test Passed");
            }
            */
        }

        public void PrintDeleted(string filename, List<List<List<int>>> matrices, List<string> files)
        {
            WorkBook workbook = WorkBook.Load(filename);
            WorkSheet sheet = workbook.DefaultWorkSheet;

            for (int i = 0; i < matrices.Count; i++)
            {
                int x = (int)Math.Floor((decimal)i / 2);
                sheet.SetCellValue(0, matrices[0].Count * i, files[x]);
                for (int j = 0; j < matrices[0].Count; j++)
                {
                    for (int k = 1; k < matrices[i][j].Count + 1; k++)
                    {
                        sheet.SetCellValue(k, j + (matrices[0].Count * i), matrices[i][j][k - 1]);
                    }
                }
                Console.WriteLine("Printed" + (i + 1) + "/" + matrices.Count);
            }
            //Save Changes
            workbook.SaveAs(filename);
        }

        public void PrintMatrices(string filename, List<List<List<int>>> matrices, List<string> files)
        {
            WorkBook workbook = WorkBook.Load(filename);
            WorkSheet sheet = workbook.DefaultWorkSheet;

            for (int i = 0; i < matrices.Count; i++)
            {
                sheet.SetCellValue(i * matrices[0].Count, 0, files[i]);
                for (int j = 0; j < matrices[0].Count; j++)
                {
                    for (int k = 1; k < matrices[i][j].Count + 1; k++)
                    {
                        sheet.SetCellValue(k + (matrices[0].Count * i) - 1, j + 1, matrices[i][j][k - 1]);
                    }
                }
                Console.WriteLine("Printed" + (i + 1) + "/" + matrices.Count);
            }

            //Save Changes
            workbook.SaveAs(filename);
        }
    }
}
