using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public class DalObject
    {
        public DalObject()
        {
            DataSource.Intialize();
        }
        /// <summary>
        /// the function adds a new drone to the list
        /// </summary>
        /// <param name="d"></param>
        public static  void AddDrone(Drone d)
        {
            DataSource.DroneList.Add(d);
        }
        /// <summary>
        /// the function gets two locations and calculates the distance between them
        /// </summary>
        /// <param name="lat1">first location latitude</param>
        /// <param name="lon1"> first ocation longtitude</param>
        /// <param name="lat2">second one</param>
        /// <param name="lon2">the second</param>
        /// <returns></returns>
        public static double getDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
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
        public static double Deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
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
            return getDistanceFromLatLonInKm(lat, lon1, d.Lattitude, d.Longitude);//sending to the func to calculate
        }
        /// <summary>
        /// the function gets a location and station id, and calculates the distance between the station and location
        /// </summary>
        /// <param name="lat">the location latitude</param>
        /// <param name="lon1">longtitude</param>
        /// <param name="id">the station id</param>
        /// <returns>the distance</returns>
        public  double StationDistance(double lat, double lon1, int id) 
        {
            Station d = DataSource.StationsList.Find(x => x.Id == id);//finding the station in the list
            return getDistanceFromLatLonInKm(lat, lon1, d.Lattitude, d.Longitude);//sending to the func to calculate
        }
        /// <summary>
        /// the function adds a new station to the list 
        /// </summary>
        /// <param name="s">the given object</param>
        public static void AddStation(Station s)
        {
            DataSource.StationsList.Add(s);
        }
        /// <summary>
        /// the function adds a new customer to the list
        /// </summary>
        /// <param name="cus">the given object</param>
        public static void AddCustomer(Customer cus)
        {
            DataSource.CustomersList.Add( cus);
        }
        /// <summary>
        ///the function adds a new parcel to the list
        /// </summary>
        /// <param name="per">the given object</param>
        public static void AddParcel(Parcel per)
        {
            DataSource.ParcelsList.Add(per);
        }
        /// <summary>
        ///the function gets a parcel and a drone and linked them (the parcel will be delivered by this drone)
        /// </summary>
        /// <param name="parcelId">the given object id</param>
        /// <param name="droneId">the given object id</param>
        public static void LinkParcelToDrone(int parcelId, int droneId)
        {
            Parcel p = DataSource.ParcelsList.Find(x => x.Id == parcelId); //finding the parcel by its id
            p.DroneId = droneId; //adding the drone id to the parcel
            p.Scheduled = DateTime.Now;
        }
        /// <summary>
        /// the function update a parcel that was picked and the drone that picked it
        /// </summary>
        /// <param name="parcelId">the parcel to pick</param>
        public static void PickParcel(int parcelId) 
        {
            Parcel p = DataSource.ParcelsList.Find(x => x.Id == parcelId); //finding the parcel by its id
            p.PickedUp = DateTime.Now;
            Drone d = DataSource.DroneList.Find(x => x.Id == p.DroneId); //finding the drone that was connected to the parcel by its id
            d.Status = DroneStatuses.Delivery;   //updating its status
        }

        /// <summary>
        /// //the function update a parcel that was dlivered
        /// </summary>
        /// <param name="parcelId"></param>The given id of a parcel
        public static void DeliveringParcel(int parcelId) 
        {
            Parcel p = DataSource.ParcelsList.Find(x => x.Id == parcelId); //finding the parcel by its id
            p.Delivered = DateTime.Now; //adding delivering time
            Drone d = DataSource.DroneList.Find(x => x.Id == p.DroneId); //finding the drone that was connected to the parcel by its id
            d.Status = DroneStatuses.Available;   //updating its status

        }
        /// <summary>
        /// the function get a drone and a station nd sent the drone to be recharged
        /// </summary>
        /// <param name="droneId"></param>the drone to charge
        /// <param name="stationId"></param>the station he will be charged at
        public static void DroneToCharge(int droneId, int stationId)
        {
            Drone d = DataSource.DroneList.Find(x => x.Id == droneId);//finding the drone
            d.Status = DroneStatuses.Maintenance; //changing to the needed status
            DroneCharge dc = new DroneCharge()//creating a dronecharge object
            {
                DroneId = droneId,
                StationId = stationId
            };
            DataSource.DChargeList.Add(dc);//adding it to the list
            Station s =DataSource.StationsList.Find(x => x.Id == stationId);//finding the station
            s.ChargeSlots--;
        }
        /// <summary>
        /// ending the charging of a given drone
        /// </summary>
        /// <param name="droneId"></param>
        public void EndingCharge(int droneId)  
        {
            Drone d = DataSource.DroneList.Find(x => x.Id == droneId);//finding the drone
            d.Status = DroneStatuses.Available;//changing its status
            DroneCharge dc = DataSource.DChargeList.Find(x => x.DroneId == droneId);//finding the dronecharge object
            Station sta1 = DataSource.StationsList.Find(x => x.Id == dc.StationId);//finsding the station he was charged at
            sta1.ChargeSlots++;//adding an empty charge slot
            DataSource.DChargeList.Remove(dc);//removing from the list
        }
        /// <summary>
        /// the function gets an id and prints the drone with the same id
        /// </summary>
        /// <param name="_id"></param>the wanted drone
        /// <returns></returns>
        public  string ShowOneDrone(int _id) 
        {
            Drone d = DataSource.DroneList.Find(x => x.Id == _id); //finding the drone by its id
            return d.ToString();
        }
        /// <summary>
        /// the function gets an id and prints the customer with the same id
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public  string ShowOneCustomer(int _id) 
        {
            Customer c = DataSource.CustomersList.Find(x => x.Id == _id); //finding the customer by its id
            return c.ToString();
        }
        /// <summary>
        /// the function gets an id and prints the station with the same id
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public  string  ShowOneStation(int _id) 
        {
            Station s = DataSource.StationsList.Find(x => x.Id == _id); //finding the station by its id
            return s.ToString();
        }
        /// <summary>
        /// the function gets an id and prints the parcel with the same id
        /// </summary>
        /// <param name="_id"></param>the given parcels id 
        /// <returns></returns>
        public  string  ShowOneParcel(int _id) 
        {
            Parcel p = DataSource.ParcelsList.Find(x => x.Id == _id); //finding the parcel by its id
            return p.ToString();
        }

        
        public List<Drone> ListDrone()
        {
            List<Drone> temp = new();
            foreach (Drone item in DataSource.DroneList)
            {
                temp.Add(item);
            }
            return temp;
        }

        public List<Station> ListStation()
        {
            List<Station> temp = new();
            foreach (Station item in DataSource.StationsList)
            {
                temp.Add(item);
            }
            return temp;
        }
        public List<Parcel> ListParcel()
        {
            List<Parcel> temp = new();
            foreach (Parcel item in DataSource.ParcelsList)
            {
                temp.Add(item);
            }
            return temp;
        }
        public List<Customer> ListCustomer()
        {
            List<Customer> temp = new();
            foreach (Customer item in DataSource.CustomersList)
            {
                temp.Add(item);
            }
            return temp;
        }
    }
}
