using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Parcel
        {
            public int Id { get; set; }
            public int SenderId { get; set; }
            public int TargetId { get; set; }
            public WeightCategories Weight { get; set; }
            public char Priority { get; set; }
            public DateTime Requested { get; set; }
            public int DroneId { get; set; }
            public DateTime Scheduled { get; set; }
            public DateTime PickedUp { get; set; }
            public DateTime Delivered { get; set; }


            public override string ToString()
            {
                return $"Customer: Id= {Id}, SenderId= {SenderId}, TargetId= {TargetId}, Weight= {Weight}, Priority= {Priority}, Requested= {Requested}, DroneId={DroneId}, Scheduled= {Scheduled}, PickedUp= {PickedUp}, Delivered= {Delivered}";
            }



        }
    }
}
