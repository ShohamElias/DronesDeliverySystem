using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class ParcelInTransfer
        {
            public int Id { get; set; }
            public Priorities Priority { get; set; }
            public WeightCategories Weight { get; set; }
            public Location PickingLocation { get; set; }
            public Location TargetLocation { get; set; }
            public CustomerInParcel Sender { get; set; }
            public CustomerInParcel Target { get; set; }
            public double TransferLength { get; set; }

        }
    }
}
