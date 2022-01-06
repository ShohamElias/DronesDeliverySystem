using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
    {
        public class ParcelToList
        {
            public int Id { get; set; }

            public String SenderName { get; set; }
            public String TargetName { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public ParcelStatuses Status { get; set; }
        }
    }

