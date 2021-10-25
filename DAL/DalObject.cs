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

        public void addDrone(Drone d)//static?
        {
            DataSource.drones[DataSource.Config.dronesI++] = d;
        }

        public void UpdateDrone(Drone d)//static?+ לעדכן ככה או ע"י ID
        {
            for (int i = 0; i < DataSource.Config.dronesI; i++)
            {
                if (DataSource.drones[i].Id == d.Id)
                    DataSource.drones[i] = d;
            }
        }

        public Drone GetDrone(int id)//static?+ 
        {
            for (int i = 0; i < DataSource.Config.dronesI; i++)
            {
                if (DataSource.drones[i].Id == id)
                    return DataSource.drones[i];
            }
            return null;
        }
        public void addStation(Station s)//static?
        {
            //Station s = new Station();
            //s.Id = id;
            //s.Name = name;
            //s.Lattitude = lattitude;
            //s.Longitude = longitude;
            //s.ChargeSlots = chargeSlots;
            DataSource.stations[DataSource.Config.stationsI++] = s;
        }

        public void addCustomer(Customer cus)//static?
        {
            //Customer cus = new Customer();
            //cus.Id = id;
            //cus.Name = name;
            //cus.Lattitude = lattitude;
            //cus.Longitude = longitude;
            //cus.Phone = phone;
            DataSource.customers[DataSource.Config.customersI++] = cus;
        }

        public void addParcel(Parcel per)//static?
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
            DataSource.parcels[DataSource.Config.parcelsI++] = per;
        }
    }
}
