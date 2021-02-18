using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _40k2d
{
    public static class Extensions
    {
        public static List<Unit> All(this UnitCollection units)
        {
            return units.GetAll();
        }
    }
}
