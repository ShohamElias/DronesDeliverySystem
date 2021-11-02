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
        public static  void addDrone(Drone d)
        {
            DataSource.DroneList.Add(d);
        }

        public static double getDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371; // Radius of the earth in km
            double dLat = deg2rad(lat2 - lat1);  // deg2rad below
            double dLon = deg2rad(lon2 - lon1);
            double a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
              ;
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c; // Distance in km
            return d;
        }
        
        public static double deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
         }
        

        public static  double CustomerDistance(double lat, double lon1, int id)//static?+ 
        {
            Customer d = DataSource.CustomersList.Find(x => x.Id == id);

            return getDistanceFromLatLonInKm(lat, lon1, d.Lattitude, d.Longitude);
        }

        public static double StationDistance(double lat, double lon1, int id)//static?+ 
        {
            Station d = DataSource.StationsList.Find(x => x.Id == id);

            return getDistanceFromLatLonInKm(lat, lon1, d.Lattitude, d.Longitude);
        }
        /// <summary>
        /// the function adds a new station to the list 
        /// </summary>
        /// <param name="s">the given object</param>
        public static void addStation(Station s)
        {
 
            DataSource.StationsList.Add(s);
        }
        /// <summary>
        /// the function adds a new customer to the list
        /// </summary>
        /// <param name="cus">the given object</param>
        public static void addCustomer(Customer cus)
        {
            DataSource.CustomersList.Add( cus);
        }
        /// <summary>
        ///the function adds a new parcel to the list
        /// </summary>
        /// <param name="per">the given object</param>
        public static void addParcel(Parcel per)
        {
            DataSource.ParcelsList.Add(per);
        }
        /// <summary>
        ///the function gets a parcel and a drone and linked them (the parcel will be delivered by this drone)
        /// </summary>
        /// <param name="parcelId">the given object id</param>
        /// <param name="droneId">the given object id</param>
        public static void linkParcelToDrone(int parcelId, int droneId)
        {
            Parcel p = DataSource.ParcelsList.Find(x => x.Id == parcelId); //finding the parcel by its id
            p.DroneId = droneId; //adding the drone id to the parcel
            p.Scheduled = DateTime.Now;
        }
        /// <summary>
        /// the function update a parcel that was picked and the drone that picked it
        /// </summary>
        /// <param name="parcelId">the parcel to pick</param>
        public static void pickParcel(int parcelId) 
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
        public static void deliveringParcel(int parcelId) 
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
        public static void droneToCharge(int droneId, int stationId)
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
        public static void EndingCharge(int droneId)  
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
        public static string ShowOneDrone(int _id) 
        {
            Drone d = DataSource.DroneList.Find(x => x.Id == _id); //finding the drone by its id
            return d.ToString();
        }
        /// <summary>
        /// the function gets an id and prints the customer with the same id
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public static string ShowOneCustomer(int _id) 
        {
            Customer c = DataSource.CustomersList.Find(x => x.Id == _id); //finding the customer by its id
            return c.ToString();
        }
        /// <summary>
        /// the function gets an id and prints the station with the same id
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public static string ShowOneStation(int _id) 
        {
            Station s = DataSource.StationsList.Find(x => x.Id == _id); //finding the station by its id
            return s.ToString();
        }
        /// <summary>
        /// the function gets an id and prints the parcel with the same id
        /// </summary>
        /// <param name="_id"></param>the given parcels id 
        /// <returns></returns>
        public static string ShowOneParcel(int _id) 
        {
            Parcel p = DataSource.ParcelsList.Find(x => x.Id == _id); //finding the parcel by its id
            return p.ToString();
        }

        public static void ShowDrone()
        {
            foreach (Drone item in DataSource.DroneList)
            {
                Console.WriteLine(item.ToString());
            }

        }

        public static void ShowStation()
        {
            foreach (Station item in DataSource.StationsList) 
            {
                Console.WriteLine(item.ToString());
            }

        }
        public static void ShowParcel() //a function that prints the parcels in the lists
        {
            foreach (Parcel item in DataSource.ParcelsList) //for each parcels in the list
            {
                Console.WriteLine(item.ToString());
            }

        }
        public static void ShowCustomer() //a function that prints the customers in our list
        {
            foreach (Customer item in DataSource.CustomersList) //for each customer in the list
            {
                Console.WriteLine(item.ToString());
            }

        }
        public static void UnmatchedParcels() //a function that prints the parcels that hasnt been linked to a drone
        {
            foreach (Parcel item in DataSource.ParcelsList) //for each station in the list
            {
              if(item.DroneId==0)  //print only those that hasnt been linked to a drone
                    Console.WriteLine(item.ToString());
            }

        }
        public static void ShowEmptySlots() //a function that prints the stations that has empty slots
        {
            foreach (Station item in DataSource.StationsList) //for each station in the list
            {
                if(item.ChargeSlots!=0) //print only those with empty charging slots
                   Console.WriteLine(item.ToString());
            }

        }
    }
}
