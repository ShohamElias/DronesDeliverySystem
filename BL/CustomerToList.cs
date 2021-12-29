using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public class CustomerToList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }

        public int ParcelsDelivered { get; set; }

        public int PurcelsNotDelivered { get; set; }

        public int ParcelsOnTheWay { get; set; }

        public int ParcelsArrived { get; set; }



    }
}
