using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using System.Xml.Linq;
using DO;

namespace DAL
{
    sealed partial class DLXML : IDal
    {
        #region singelton
        static readonly DLXML instance = new DLXML();
        static DLXML() { }
        DLXML() { }
        public static DLXML Instance { get => instance; }
        #endregion

        #region DS DLXML Files
        string customerPath = @"customerXML.xml";
        string dronechargePath = @"drone-chargeXML.xml";
        string dronePath = @"droneXML.xml";
        string parcelPath = @"parcelXML.xml";
        string stationPath = @"stationXML.xml";
        #endregion


        #region Drone 
        void AddCustomer(DO.Customer cus)
        {
            List<DO.Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<DO.Customer>(customerPath);
            Customer dd = ListCustomer.Find(s => s.Id == cus.Id);

            if (cus.Id == dd.Id)
                throw new DO.IDExistsException(cus.Id, "Duplicate customer ID");

            ListCustomer.Add(cus); //no need to Clone()

            XMLTools.SaveListToXMLSerializer(ListCustomer, customerPath);
        }

        public DO.Customer GetCustomer(int id)
        {
           if (!CheckCustomer(id))///
                throw new DO.BadIdException(id, "Customer id doesnt exist: ");

            List<DO.Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<DO.Customer>(customerPath);
            Customer dd = ListCustomer.Find(s => s.Id == id);
            return dd;

        }
        public bool CheckCustomer(int id)
        {
            List<DO.Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<DO.Customer>(customerPath);
            return ListCustomer.Any(x => x.Id == id);
        }

        public IEnumerable<DO.Customer> GetALLCustomer()
        {
            List<DO.Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<DO.Customer>(customerPath);
            return from Customer in ListCustomer
                   select Customer;
        }

        double CustomerDistance(double lat, double lon1, int id)
        {
            List<DO.Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<DO.Customer>(customerPath);
            Customer d = ListCustomer.Find(x => x.Id == id);//finding the customer
            if (d.Id != id)
                throw new DO.BadIdException(id, "This id doesnt exists");
            return getDistanceFromLatLonInKm(lat, lon1, d.Lattitude, d.Longitude);//sending to the func to calculate
        }
        IEnumerable<DO.Customer> ListCustomer()
        {
            List<DO.Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<DO.Customer>(customerPath);
            return ListCustomer.ToList();
        }
        public void UpdateCustomer(DO.Customer newD)
        {
            List<DO.Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<DO.Customer>(customerPath);
            Customer d2 = ListCustomer.Find(x => x.Id == newD.Id); //finding the station by its id
            if (d2.Id != newD.Id)
                throw new DO.BadIdException(newD.Id, "This Customer does not exist");
            ListCustomer.Remove(d2);
            ListCustomer.Add(newD);
            XMLTools.SaveListToXMLSerializer(ListCustomer, customerPath);
        }

        public double getDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
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
        public double Deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }
        #endregion
    }
}
