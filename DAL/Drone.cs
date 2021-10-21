using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public class Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public int MaxWeight { get; set; }
            public string Status { get; set; }
            public double Battery { get; set; }

            public override string ToString()
            {
                return $"Customer: Id= {Id}, Model= {Model}, MaxWeight={MaxWeight},Status= {Status},Battery= {Battery}";
            }
        }
    }
}
