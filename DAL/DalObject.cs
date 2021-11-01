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

        public static  void addDrone(Drone d)//static?
        {
            DataSource.dronelist.Add( d);
        }

        //public void UpdateDrone(Drone d)//static?+ לעדכן ככה או ע"י ID
        //{
        //    //for (int i = 0; i < DataSource.Config.dronesI; i++)
        //    //{
        //    //    if (DataSource.drones[i].Id == d.Id)
        //    //        DataSource.drones[i] = d;
        //    //}
        //}

        public Drone GetDrone(int id)//static?+ 
        {
            Drone d = DataSource.dronelist.Find(x => x.Id == id);
            return d;
            //for (int i = 0; i < DataSource.Config.dronesI; i++)
            //{

            //    if (DataSource.drones[i].Id == id)
            //        return DataSource.drones[i];
            //}
            //return null;
        }
        public static void addStation(Station s)//static?
        {
            //Station s = new Station();
            //s.Id = id;
            //s.Name = name;
            //s.Lattitude = lattitude;
            //s.Longitude = longitude;
            //s.ChargeSlots = chargeSlots;
            DataSource.stationsList.Add(s);
        }

        public static void addCustomer(Customer cus)//static?
        {
            //Customer cus = new Customer();
            //cus.Id = id;
            //cus.Name = name;
            //cus.Lattitude = lattitude;
            //cus.Longitude = longitude;
            //cus.Phone = phone;
            DataSource.customersList.Add( cus);
        }

        public static void addParcel(Parcel per)//static?
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
        public static void linkParcelToDrone(int parcelId, int droneId)
        {
            Parcel p = DataSource.parcelsList.Find(x => x.Id == parcelId);
            p.DroneId = droneId;
            p.Scheduled = DateTime.Now;
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
            d.Status = DroneStatuses.Delivery;   //???????????????????????
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
        public static void EndingCharge(int droneId)  //??????????????????????????????????????????????????????
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
