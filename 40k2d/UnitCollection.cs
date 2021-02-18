using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _40k2d
{
    public class UnitCollection
    {
        private List<Unit> units = new List<Unit>();

        public List<Unit> GetAll()
        {
            return units;
        }
        public void AddUNit(Unit u)
        {
            units.Add(u);
        }
        public void MoveAllUnits(float x, float y)
        {
            units.ForEach(u => u.Move(x, y, true));
        }
        public void MoveUnit(float x, float y)
        {
            units.ForEach(u => u.Move(x, y));
        }
        public Unit Collision(float x, float y)
        {
            return units.FirstOrDefault(u => u.Hit(x, y));
        }

        public void PaintUnits(PaintEventArgs e)
        {
            units.ForEach(u => u.Paint(e));
        }
        public void PaintClosest(PaintEventArgs e, float x, float y)
        {
            units.ForEach(u => u.PaintClosest(e,x,y));
            units.ForEach(u => u.PaintArc(e, x, y));
        }
    }
}
