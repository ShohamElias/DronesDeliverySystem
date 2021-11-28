using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject : IDAL.IDal
    {
        public double[] ElectricityUse()
        {
            double[] arr= new double[] { DataSource.Config.empty, DataSource.Config.light, DataSource.Config.medium, DataSource.Config.heavy, DataSource.Config.chargeRate };
            return arr;
        }
        
        public void UpdateDrone(Drone newD)
        {
            Drone d2 = DataSource.DroneList.Find(x => x.Id == newD.Id); //finding the station by its id
            if (d2.Id != newD.Id)
                throw new IDAL.DO.BadIdException(newD.Id, "This drone does not exist");
            DataSource.DroneList.Remove(d2);
            DataSource.DroneList.Add(newD);
        }

    public Drone GetDrone(int id)
        {
            if (!CheckDrone(id))
                throw new IDAL.DO.BadIdException(id, "Drone id doesnt exist: ");

           Drone d = DataSource.DroneList.Find(d => d.Id == id);
            return d;
        }

        public DroneCharge GetDroneCharge(int id)
        {
            return DataSource.DChargeList.Find(x => x.DroneId == id);
        }

        public void DeleteDroneCharge(int id)
        {
            DataSource.DChargeList.Remove(DataSource.DChargeList.Find(x => x.DroneId == id));
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
                throw new IDAL.DO.IDExistsException(d.Id, "This parcel id already exists");
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
                throw new IDAL.DO.BadIdException(droneId, "This parcel id doesnt exists");
            Drone d = DataSource.DroneList.Find(x => x.Id == droneId);//finding the drone
            if (d.Id != droneId)
                throw new IDAL.DO.BadIdException(droneId, "This drone id doesnt exists");
            p.DroneId = droneId; //adding the drone id to the parcel
            p.Scheduled = DateTime.Now;
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
                throw new IDAL.DO.BadIdException(droneId, "This drone id doesnt exists");
            //DataSource.DroneList.Remove(d);
            //d.Status = DroneStatuses.Maintenance; //changing to the needed status
            //DataSource.DroneList.Add(d);
            Station s = DataSource.StationsList.Find(x => x.Id == stationId);//finding the station
            if (s.Id != stationId)
                throw new IDAL.DO.BadIdException(stationId, "This station id doesnt exists");
            DataSource.StationsList.Remove(s);
            s.ChargeSlots--;
            DataSource.StationsList.Add(s);
            DroneCharge dc = new DroneCharge()//creating a dronecharge object
            {
                DroneId = droneId,
                StationId = stationId
            };
            DataSource.DChargeList.Add(dc);//adding it to the list
            Console.WriteLine(s.ChargeSlots);
        }

        /// <summary>
        /// ending the charging of a given drone
        /// </summary>
        /// <param name="droneId"></param>
        public void EndingCharge(int droneId)
        {
            Drone d = DataSource.DroneList.Find(x => x.Id == droneId);//finding the drone
            if (d.Id != droneId)
                throw new IDAL.DO.BadIdException(droneId, "This drone id doesnt exists");
            //d.Status = DroneStatuses.Available;//changing its status
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
        public string ShowOneDrone(int _id)
        {
            Drone d = DataSource.DroneList.Find(x => x.Id == _id); //finding the drone by its id
            if (d.Id == _id)
                return d.ToString();
            else
                throw new IDAL.DO.BadIdException(_id, "This id doesnt exists");
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

        public  int GetChargeRate()
        {
            return DataSource.Config.chargeRate;
        }
    }
}
