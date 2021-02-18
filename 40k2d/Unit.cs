using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _40k2d
{
    public class Unit
    {
        public float xPos, yPos, xShoot, yShoot, size;
        public bool active, shooting, lineHit;
        public Color color = Color.Black;
        public Player owner;

        public bool Hit(float xLoc, float yLoc)
        {
            float rad = size / 2;
            float middleX = xPos + rad;
            float middleY = yPos + rad;
            if ((xLoc - middleX) * (xLoc - middleX) +
            (yLoc - middleY) * (yLoc - middleY) <= rad * rad)
                return true;
            else
                return false;
        }
        public void Move(float x, float y, bool allU = false)
        {
            if (active || allU)
            {
                xPos += x;
                yPos += y;
            }
        }

        public void Paint(PaintEventArgs e)
        {
            e.Graphics.FillEllipse(new SolidBrush(color), new RectangleF(xPos, yPos, size, size));
            e.Graphics.FillEllipse(new SolidBrush(Color.GhostWhite), new RectangleF(xPos + 2, yPos + 2, size - 4, size - 4));
        }

        public void PaintShoot(PaintEventArgs e)
        {
            if (shooting)
                e.Graphics.DrawLine(new Pen(lineHit ? Color.Red : Color.Black), xPos + (size / 2), yPos + (size / 2), xShoot, yShoot);
        }

        public bool LineHit(int x1In, int x2In, int y1In, int y2In, int x3In, int y3In)
        {
            float rad = size / 2;
            float middleX = xPos + rad;
            float middleY = yPos + rad;
            return doIntersect(new Point(x1In, y1In), new Point(x2In, y2In), new Point(Convert.ToInt32(middleX), Convert.ToInt32(middleY)), new Point(Convert.ToInt32(x3In), Convert.ToInt32(y3In)));
        }

        public bool onSegment(Point p, Point q, Point r)
        {
            if (q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
                q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y))
                return true;

            return false;
        }

        // To find orientation of ordered triplet (p, q, r). 
        // The function returns following values 
        // 0 --> p, q and r are colinear 
        // 1 --> Clockwise 
        // 2 --> Counterclockwise 
        public int orientation(Point p, Point q, Point r)
        {
            // See https://www.geeksforgeeks.org/orientation-3-ordered-points/ 
            // for details of below formula. 
            int val = (q.Y - p.Y) * (r.X - q.X) -
                    (q.X - p.X) * (r.Y - q.Y);

            if (val == 0) return 0; // colinear 

            return (val > 0) ? 1 : 2; // clock or counterclock wise 
        }

        // The main function that returns true if line segment 'p1q1' 
        // and 'p2q2' intersect. 
        public bool doIntersect(Point p1, Point q1, Point p2, Point q2)
        {
            // Find the four orientations needed for general and 
            // special cases 
            int o1 = orientation(p1, q1, p2);
            int o2 = orientation(p1, q1, q2);
            int o3 = orientation(p2, q2, p1);
            int o4 = orientation(p2, q2, q1);

            if (o1 != o2 && o3 != o4)
                return true;

            // Special Cases 
            // p1, q1 and p2 are colinear and p2 lies on segment p1q1 
            if (o1 == 0 && onSegment(p1, p2, q1)) return true;

            // p1, q1 and q2 are colinear and q2 lies on segment p1q1 
            if (o2 == 0 && onSegment(p1, q2, q1)) return true;

            // p2, q2 and p1 are colinear and p1 lies on segment p2q2 
            if (o3 == 0 && onSegment(p2, p1, q2)) return true;

            // p2, q2 and q1 are colinear and q1 lies on segment p2q2 
            if (o4 == 0 && onSegment(p2, q1, q2)) return true;

            return false; // Doesn't fall in any of the above cases 
        }

        public void PaintClosest(PaintEventArgs e, float x, float y)
        {
            float rad = size / 2;
            float middleX = xPos + rad;
            float middleY = yPos + rad;
            double xc = middleX + rad * ((x - middleX) / (Math.Sqrt(((x - middleX) * (x - middleX)) + ((y - middleY) * (y - middleY)))));
            double yc = middleY + rad * ((y - middleY) / (Math.Sqrt(((x - middleX) * (x - middleX)) + ((y - middleY) * (y - middleY)))));

            if (double.IsNaN(xc) || double.IsNaN(yc))
                return;
            e.Graphics.DrawRectangle(new Pen(Color.Black), new Rectangle(Convert.ToInt32(xc), Convert.ToInt32(yc), 1, 1));
        }

        public void PaintArc(PaintEventArgs e, float x, float y)
        {

            //float rad = size / 2;
            //float middleX = xPos + rad;
            //float middleY = yPos + rad;
            //for (int i = 0; i <= 10; i++)
            //{
            //    var sub_angle = (i / 10) * DegreeToRadian(90); ;
            //    var xi = middleX + rad * (Math.Sin(sub_angle) * fx + (1 - Math.Cos(sub_angle)) * (-lx));
            //    var yi = middleY + rad * (Math.Sin(sub_angle) * fy + (1 - Math.Cos(sub_angle)) * (-ly));

            //    e.Graphics.DrawRectangle(new Pen(Color.Black), new Rectangle(Convert.ToInt32(xi), Convert.ToInt32(yi), 1, 1));
            //}

            foreach (Point p in AllPoints(new Point(Convert.ToInt32(x),Convert.ToInt32(y))))
            {
                e.Graphics.DrawRectangle(new Pen(Color.Black), new Rectangle(Convert.ToInt32(p.X), Convert.ToInt32(p.Y), 1, 1));
            }
            
        }

        private IEnumerable<Point> GetCirclePoints()
        {
            int rad = Convert.ToInt32(size / 2);
            int middleX = Convert.ToInt32(xPos + rad);
            int middleY = Convert.ToInt32(yPos + rad);
            int x1 = rad, y1 = 0;

            // Printing the initial point on the
            // axes after translation
            var points = new HashSet<Point> { new Point(x1 + middleX, y1 + middleY) };

            // When radius is zero only1 a single
            // point will be printed
            if (rad > 0)
            {

                points.Add(new Point(x1 + middleX, -y1 + middleY));
                points.Add(new Point(y1 + middleX, x1 + middleY));
                points.Add(new Point(-y1 + middleX, x1 + middleY));
            }

            // Initialising the value of P
            int P = 1 - rad;
            while (x1 > y1)
            {

                y1++;

                // Mid-point is inside or on the perimeter
                if (P <= 0)
                    P = P + 2 * y1 + 1;

                // Mid-point is outside the perimeter
                else
                {
                    x1--;
                    P = P + 2 * y1 - 2 * x1 + 1;
                }

                // All the perimeter points have already1  
                // been printed
                if (x1 < y1)
                    break;

                // Printing the generated point and its  
                // reflection in the other octants after
                // translation
                points.Add(new Point(x1 + middleX, y1 + middleY));
                points.Add(new Point(-x1 + middleX, y1 + middleY));
                points.Add(new Point(x1 + middleX, -y1 + middleY));
                points.Add(new Point(-x1 + middleX, -y1 + middleY));

                // If the generated point is on the  
                // line x1 = y1 then the perimeter points
                // have already1 been printed
                if (x1 != y1)
                {
                    points.Add(new Point(y1 + middleX, x1 + middleY));
                    points.Add(new Point(-y1 + middleX, x1 + middleY));
                    points.Add(new Point(y1 + middleX, -x1 + middleY));
                    points.Add(new Point(-y1 + middleX, -x1 + middleY));
                }
            }

            return points;
        }

        public IEnumerable<Point> AllPoints( Point other)
        {
            int rad = Convert.ToInt32(size / 2);
            int middleX = Convert.ToInt32(xPos + rad);
            int middleY = Convert.ToInt32(yPos + rad);
            var radians = Math.Atan2(other.Y - middleY, other.X - middleX);

            var leftRad = radians - (Math.PI / 2);

            var points = new HashSet<Point>();

            for (double i = 0; i < Math.PI; i += 0.5)
            {
                var x = rad * Math.Cos(leftRad + i);
                var y = rad * Math.Sin(leftRad + i);
                points.Add(new Point((int)x, (int)y));
            }

            return points.Select(p => new Point(p.X + middleX, p.Y + middleY));
        }

        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public void ShootAt(float x, float y)
        {
            shooting = true;
            xShoot = x;
            yShoot = y;
        }


    }
}
