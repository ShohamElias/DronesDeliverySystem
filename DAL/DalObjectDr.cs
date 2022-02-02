﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using System.Runtime.CompilerServices;


namespace Dal
{
     partial class DalObject : DalApi.IDal
    {
        public double[] ElectricityUse()
        {
            double[] arr= new double[] { DataSource.Config.empty, DataSource.Config.light, DataSource.Config.medium, DataSource.Config.heavy, DataSource.Config.chargeRate };
            return arr;
        }
        public int getParcelMax()
        {
            return DataSource.Config.countIdParcel;
        }
        
        public void UpdateDrone(Drone newD)
        {
            Drone d2 = DataSource.DroneList.Find(x => x.Id == newD.Id); //finding the station by its id
            if (d2.Id != newD.Id)
                throw new DO.BadIdException(newD.Id, "This drone does not exist");
            DataSource.DroneList.Remove(d2);
            DataSource.DroneList.Add(newD);
        }

    public Drone GetDrone(int id)
        {
            if (!CheckDrone(id))
                throw new DO.BadIdException(id, "Drone id doesnt exist: ");

           Drone d = DataSource.DroneList.Find(d => d.Id == id);
            return d;
        }


        public bool CheckDrone(int id)
        {
            return DataSource.DroneList.Any(d => d.Id== id);
        }

        public IEnumerable<Drone> GetALLDrone()
        {
            return from d in DataSource.DroneList
                   select d;
        }

        /// <summary>
        /// the function adds a new drone to the list
        /// </summary>
        /// <param name="d"></param>
        public  void AddDrone(Drone d)
        {
            Drone d2 = DataSource.DroneList.Find(x => x.Id == d.Id); //finding the station by its id
            if (d2.Id == d.Id)
                throw new DO.IDExistsException(d.Id, "This parcel id already exists");
            DataSource.DroneList.Add(d);
        }

        /// <summary>
        ///the function gets a parcel and a drone and linked them (the parcel will be delivered by this drone)
        /// </summary>
        /// <param name="parcelId">the given object id</param>
        /// <param name="droneId">the given object id</param>
        public  void LinkParcelToDrone(int parcelId, int droneId)
        {

            Parcel p = DataSource.ParcelsList.Find(x => x.Id == parcelId); //finding the parcel by its id
            if (p.Id != parcelId)
                throw new DO.BadIdException(parcelId, "This parcel id doesnt exists");
            Drone d = DataSource.DroneList.Find(x => x.Id == droneId);//finding the drone
            if (d.Id != droneId)
                throw new DO.BadIdException(droneId, "This drone id doesnt exists");
            DataSource.ParcelsList.Remove(p);
            p.DroneId = droneId; //adding the drone id to the parcel
            p.Scheduled = DateTime.Now;
            DataSource.ParcelsList.Add(p);
        }

        /// <summary>
        /// the function get a drone and a station nd sent the drone to be recharged
        /// </summary>
        /// <param name="droneId"></param>the drone to charge
        /// <param name="stationId"></param>the station he will be charged at
        public void DroneToCharge(int droneId, int stationId)
        {
            Drone d = DataSource.DroneList.Find(x => x.Id == droneId);//finding the drone
            if (d.Id != droneId)
                throw new DO.BadIdException(droneId, "This drone id doesnt exists");
            Station s = DataSource.StationsList.Find(x => x.Id == stationId);//finding the station
            if (s.Id != stationId)
                throw new DO.BadIdException(stationId, "This station id doesnt exists");
            DataSource.StationsList.Remove(s);
            s.ChargeSlots--;
            DataSource.StationsList.Add(s);
            DroneCharge dc = new DroneCharge()//creating a dronecharge object
            {
                DroneId = droneId,
                StationId = stationId
            };
            DataSource.DChargeList.Add(dc);//adding it to the list
            //Console.WriteLine(s.ChargeSlots);
        }

        /// <summary>
        /// ending the charging of a given drone
        /// </summary>
        /// <param name="droneId"></param>
        public void EndingCharge(int droneId)
        {
            Drone d = DataSource.DroneList.Find(x => x.Id == droneId);//finding the drone
            if (d.Id != droneId)
                throw new DO.BadIdException(droneId, "This drone id doesnt exists");
            //d.Status = DroneStatuses.Available;//changing its status
            DroneCharge dc = DataSource.DChargeList.Find(x => x.DroneId == droneId);//finding the dronecharge object
            Station sta1 = DataSource.StationsList.Find(x => x.Id == dc.StationId);//finsding the station he was charged at
            sta1.ChargeSlots++;//adding an empty charge slot
            DataSource.DChargeList.Remove(dc);//removing from the list
        }


        /// <summary>
        /// the func create a copy of the drones list and returns it
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> ListDrone()
        {
            List<Drone> temp = new();
            foreach (Drone item in DataSource.DroneList)
            {
                temp.Add(item);
            }
            return temp;
        }

        public  double GetChargeRate()
        {
            return DataSource.Config.chargeRate;
        }



        public DroneCharge GetDroneCharge(int id)
        {
            if (!CheckDC(id))
                throw new DO.BadIdException(id, "This drone does not exist in drone charges list: ");
            return DataSource.DChargeList.Find(x => x.DroneId == id);
        }

        public void DeleteDroneCharge(int id)
        {
            if (!CheckDC(id))
                throw new DO.BadIdException(id, "This drone does not exist in drone charges list: ");
            DataSource.DChargeList.Remove(GetDroneCharge(id));
        }

        public void AddDroneCharge(DroneCharge dc)
        {
            DroneCharge d2 = DataSource.DChargeList.Find(x => x.DroneId == dc.DroneId); //finding the station by its id
            if (d2.DroneId == dc.DroneId)
                throw new DO.IDExistsException(dc.DroneId, "This drone id already in charge");
            DataSource.DChargeList.Add(dc);
        }

        public void UpdateDroneCharge(DroneCharge dc)
        {
            DroneCharge d2 = DataSource.DChargeList.Find(x => x.DroneId == dc.DroneId); //finding the station by its id
            if (d2.DroneId != dc.DroneId)
                throw new DO.BadIdException(dc.DroneId, "This drone does not exist in drone charges list");
            DataSource.DChargeList.Remove(d2);
            DataSource.DChargeList.Add(dc);
        }

        public bool CheckDC(int id)
        {
            return DataSource.DChargeList.Any(d => d.DroneId == id);

        }

        public IEnumerable<DroneCharge> GetALLDroneCharges(/*Predicate<DroneCharge> P*/)
        {
            return from d in DataSource.DChargeList
                       //where P(d)
                   select d;
        }
        public IEnumerable<Drone> GetALLDronesBy(Predicate<Drone> P)
        {
            return from d in DataSource.DroneList
                  where P(d)
                   select d;
        }
    }
}
