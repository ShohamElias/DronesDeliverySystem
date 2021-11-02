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

        public static  void addDrone(Drone d)//the function adds a new drone to the list
        {
            DataSource.dronelist.Add( d);
            //Console.WriteLine((float)getDistanceFromLatLonInKm(0.05, 80, 0.058, 80.3));
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
            Customer d = DataSource.customersList.Find(x => x.Id == id);

            return getDistanceFromLatLonInKm(lat, lon1, d.Lattitude, d.Longitude);
        }

        public static double StationDistance(double lat, double lon1, int id)//static?+ 
        {
            Station d = DataSource.stationsList.Find(x => x.Id == id);

            return getDistanceFromLatLonInKm(lat, lon1, d.Lattitude, d.Longitude);
        }
        public static void addStation(Station s)//the function adds a new station to the list
        {
            //Station s = new Station();
            //s.Id = id;
            //s.Name = name;
            //s.Lattitude = lattitude;
            //s.Longitude = longitude;
            //s.ChargeSlots = chargeSlots;
            DataSource.stationsList.Add(s);
        }

        public static void addCustomer(Customer cus)//the function adds a new customer to the list
        {
            //Customer cus = new Customer();
            //cus.Id = id;
            //cus.Name = name;
            //cus.Lattitude = lattitude;
            //cus.Longitude = longitude;
            //cus.Phone = phone;
            DataSource.customersList.Add( cus);
        }

        public static void addParcel(Parcel per)//the function adds a new parcel to the list
        {
            //Parcel per = new Parcel();
            //per.Id = id;
            //per.TargetId = targetId;
            //per.Weight = weight;
            //per.Priority = priority;
            //per.Requested = requested;
            //per.DroneId = droneId;
            //per.SenderId = senderId;
            //per.Scheduled = scheduled;
            //per.PickedUp = pickedUp;
            //per.Delivered = delivered;
            DataSource.parcelsList.Add(per);
        }
        public static void linkParcelToDrone(int parcelId, int droneId)//the function gets a parcel and a drone and linked them (the parcel will be delivered by this drone)
        {
            Parcel p = DataSource.parcelsList.Find(x => x.Id == parcelId);
            p.DroneId = droneId;
            //foreach (Parcel item in parcelsList)
            //{
            //    if (item.Id == parcelId)
            //        item.DroneId = droneId;
            //}
        }
        public static void pickParcel(int parcelId)
        {
            Parcel p = DataSource.parcelsList.Find(x => x.Id == parcelId);
            p.PickedUp = DateTime.Now;
            Drone d = DataSource.dronelist.Find(x => x.Id == p.DroneId);
            d.Status = DroneStatuses.Delivery;   //??
        }
        public static void deliveringParcel(int parcelId)
        {
            Parcel p = DataSource.parcelsList.Find(x => x.Id == parcelId);
            p.Delivered = DateTime.Now;
            Drone d = DataSource.dronelist.Find(x => x.Id == p.DroneId);
            d.Status = DroneStatuses.Available;   //??

        }
        public static void droneToCharge(int droneId, int stationId)
        {
            Drone d = DataSource.dronelist.Find(x => x.Id == droneId);
            d.Status = DroneStatuses.Maintenance; 
            DroneCharge dc = new DroneCharge()
            {
                DroneId = droneId,
                StationId = stationId
            };
            Station s =DataSource.stationsList.Find(x => x.Id == stationId);
            s.ChargeSlots--;
        }
        public static void EndingCharge(int droneId)
        {
            Drone d = DataSource.dronelist.Find(x => x.Id == droneId);
            d.Status = DroneStatuses.Available;
            //~~~~~~~~~~~~~~~~~~~? station and dronecharging list
        }

        public static string ShowOneDrone(int _id)
        {
            Drone d = DataSource.dronelist.Find(x => x.Id == _id);
            return d.ToString();
        }
        public static string ShowOneCustomer(int _id)
        {
            Customer c = DataSource.customersList.Find(x => x.Id == _id);
            return c.ToString();
        }
        public static string ShowOneStation(int _id)
        {
            Station s = DataSource.stationsList.Find(x => x.Id == _id);
            return s.ToString();
        }
        public static string ShowOneParcel(int _id)
        {
            Parcel p = DataSource.parcelsList.Find(x => x.Id == _id);
            return p.ToString();
        }

        public static void ShowDrone()
        {
            foreach (Drone item in DataSource.dronelist)
            {
                Console.WriteLine(item.ToString());
            }

        }

        public static void ShowStation()
        {
            foreach (Station item in DataSource.stationsList)
            {
                Console.WriteLine(item.ToString());
            }

        }
        public static void ShowParcel()
        {
            foreach (Parcel item in DataSource.parcelsList)
            {
                Console.WriteLine(item.ToString());
            }

        }
        public static void ShowCustomer()
        {
            foreach (Customer item in DataSource.customersList)
            {
                Console.WriteLine(item.ToString());
            }

        }
        public static void UnmatchedParcels()
        {
            foreach (Parcel item in DataSource.parcelsList)
            {
              if(item.DroneId==0) 
                Console.WriteLine(item.ToString());
            }

        }
        public static void ShowEmptySlots()
        {
            foreach (Station item in DataSource.stationsList)
            {
                if(item.ChargeSlots!=0)
                   Console.WriteLine(item.ToString());
            }

        }
    }
}
