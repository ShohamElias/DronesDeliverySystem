using System;
using BO;
using System.Threading;
using static BL.BL;
using System.Linq;

namespace BL
{
    class Simulation
    {
        public Simulation(BL blOBJ, int droneId, Action updateWPF, Func<bool> check)
        {
            while (check())
            {

            }
        }
    }
}
