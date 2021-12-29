using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;

namespace DalObject
{
    sealed partial class DalObject : IDal
    {
        internal static readonly IDal instance = new DalObject();
        public static IDal Instance{ get => instance; }
        public DalObject()
        {
            DataSource.Intialize();
        }
        
        /// <summary>
        /// the function gets two locations and calculates the distance between them
        /// </summary>
        /// <param name="lat1">first location latitude</param>
        /// <param name="lon1"> first ocation longtitude</param>
        /// <param name="lat2">second one</param>
        /// <param name="lon2">the second</param>
        /// <returns></returns>
        public  double getDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371; // Radius of the earth in km
            double dLat = Deg2rad(lat2 - lat1);  // deg2rad below
            double dLon = Deg2rad(lon2 - lon1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(Deg2rad(lat1)) * Math.Cos(Deg2rad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2);///calculating by the formula
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c; // Distance in km
            return d;
        }
        /// <summary>
        /// the function gets a number in dergrees and converts it to radians
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>
        public  double Deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }
        public IEnumerable<Customer> GetALLCustomer()
        {
            return from cust in DataSource.CustomersList
                   select cust;
        }

        public Customer GetCustomer(int id)
        {
            if (id == 0)
                return new Customer();
            if (!CheckCustomer(id))
                throw new DO.BadIdException(id, "Customer id doesnt exist: ");

            Customer cust1 = DataSource.CustomersList.Find(cust1 => cust1.Id == id);
            return cust1;
        }

        public bool CheckCustomer(int id)
        {
            return DataSource.CustomersList.Any(cust1 => cust1.Id == id);
        }

        /// <summary>
        /// the function gets a location and custoner id, and calculates the distance between the customer and location
        /// </summary>
        /// <param name="lat">the location latitude</param>
        /// <param name="lon1">longtitude</param>
        /// <param name="id">the customer id</param>
        /// <returns></returns>
        public   double CustomerDistance(double lat, double lon1, int id) 
        {
            Customer d = DataSource.CustomersList.Find(x => x.Id == id);//finding the customer
            if (d.Id != id)
                throw new DO.BadIdException(id, "This id doesnt exists");
            return getDistanceFromLatLonInKm(lat, lon1, d.Lattitude, d.Longitude);//sending to the func to calculate
        }
        
        /// <summary>
        /// the function adds a new customer to the list
        /// </summary>
        /// <param name="cus">the given object</param>
        public  void AddCustomer(Customer cus)
        {
            Customer c2 = DataSource.CustomersList.Find(x => x.Id == cus.Id); //finding the station by its id
            if (c2.Id == cus.Id)
                throw new DO.IDExistsException(cus.Id, "This parcel id already exists");
            DataSource.CustomersList.Add( cus);
        }

        public void UpdateCustomer(Customer newD)
        {
            Customer d2 = DataSource.CustomersList.Find(x => x.Id == newD.Id); //finding the station by its id
            if (d2.Id != newD.Id)
                throw new DO.BadIdException(newD.Id, "This Customer does not exist");
            DataSource.CustomersList.Remove(d2);
            DataSource.CustomersList.Add(newD);
        }

 

        /// <summary>
        /// the function rturns a copy of the Customer List
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public IEnumerable<Customer> ListCustomer()
        {
            //List<Customer> temp = new();
            //foreach (Customer item in DataSource.CustomersList)
            //{
            //    temp.Add(item);
            //}
            //return temp;
            return DataSource.CustomersList.ToList();
        }

        public IEnumerable<Customer> GetALLCustomersBy(Predicate<Customer> P)
        {
            return from d in DataSource.CustomersList
                   where P(d)
                   select d;
        }
    }
}
