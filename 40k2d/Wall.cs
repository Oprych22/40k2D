using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _40k2d
{
    public class Wall
    {
        public int x1, x2, y1, y2;
        public bool active;

        public void Paint(PaintEventArgs e)
        {
            if (active)
                e.Graphics.DrawLine(new Pen(Color.DarkSlateGray,2), x1, y1, x2, y2);
        }

        public bool Hit(int x1In, int x2In, int y1In, int y2In)
        {
            if (x1 > x2In || x2 > x1In || !active)
            {
                return false;
            }

            // If one rectangle is above other  
            if (y1 < y2In || y2 < y2In)
            {
                return false;
            }
            return true;
        }
    }
}
