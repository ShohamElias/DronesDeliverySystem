using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
    {
        class ParcelAtCustomer
        {
            public int Id { get; set; }

            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public ParcelStatuses Status { get; set; }

            public CustomerInParcel OtherCustomer { get; set; }
        }
    }
