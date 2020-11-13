using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

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
                /*
                for (int i = 0; i < 10_000; i++)
                {
                    pen.Color = Color.FromArgb(rand.Next());
                    var pt1 = new Point(rand.Next(bmp.Width), rand.Next(bmp.Height));
                    var pt2 = new Point(rand.Next(bmp.Width), rand.Next(bmp.Height));
                    gfx.DrawLine(pen, pt1, pt2);
                }
                */
                bmp.Save("RouteVisualisation.png");
            }
        }
    }
}
