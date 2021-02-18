using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _40k2d
{
    public class Player
    {
        public UnitCollection units = new UnitCollection();

        public Player(int startX, int startY, Color colorIn)
        {
            units.AddUNit(new Unit() { size = 30, xPos = startX, yPos = startY, color = colorIn, owner = this });
            units.AddUNit(new Unit() { size = 30, xPos = startX+50, yPos = startY, color = colorIn, owner = this });
            units.AddUNit(new Unit() { size = 30, xPos = startX, yPos = startY+ 50, color = colorIn, owner = this });
            units.AddUNit(new Unit() { size = 30, xPos = startX + 50, yPos = startY+ 50, color = colorIn, owner = this });
        }

    }
}
