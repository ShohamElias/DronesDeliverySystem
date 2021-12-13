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
       

        /// <summary>
        /// the function gets a location and station id, and calculates the distance between the station and location
        /// </summary>
        /// <param name="lat">the location latitude</param>
        /// <param name="lon1">longtitude</param>
        /// <param name="id">the station id</param>
        /// <returns>the distance</returns>
        public double StationDistance(double lat, double lon1, int id)
        {
            Station d = DataSource.StationsList.Find(x => x.Id == id);//finding the station in the list
            if (d.Id != id)
                throw new IDAL.DO.BadIdException(id, "This id doesnt exists");
            return getDistanceFromLatLonInKm(lat, lon1, d.Lattitude, d.Longitude);//sending to the func to calculate
        }

        public void UpdateStation(Station newD)
        {
            Station d2 = DataSource.StationsList.Find(x => x.Id == newD.Id); //finding the station by its id
            if (d2.Id != newD.Id)
                throw new IDAL.DO.BadIdException(newD.Id, "This Station does not exist");
            DataSource.StationsList.Remove(d2);
            DataSource.StationsList.Add(newD);
        }


        /// <summary>
        /// the function adds a new station to the list 
        /// </summary>
        /// <param name="s">the given object</param>
        public void AddStation(Station s)
        {
            Station s2 = DataSource.StationsList.Find(x => x.Id == s.Id); //finding the station by its id
            if (s2.Id == s.Id)
                throw new IDAL.DO.IDExistsException(s.Id, "This id already exists");
            DataSource.StationsList.Add(s);
        }

        public Station GetStation(int id)
        {
            if (!CheckStation(id))
                throw new IDAL.DO.BadIdException(id, "Station id doesnt exist: ");

            Station stat1 = DataSource.StationsList.Find(stat1 => stat1.Id == id);
            return stat1;
        }

        public bool CheckStation(int id)
        {
            return DataSource.StationsList.Any(stat1 => stat1.Id == id);
        }

        public IEnumerable<Station> GetALLStation()
        {
            return from stat in DataSource.StationsList
                   select stat;
        }

        /// <summary>
        /// the function gets an id and prints the station with the same id
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public Station ShowOneStation(int _id)
        {
            Station s = DataSource.StationsList.Find(x => x.Id == _id); //finding the station by its id
            if (s.Id == _id)
                return s;
            else
                throw new IDAL.DO.BadIdException(_id, "This id doesnt exists");
        }

        /// <summary>
        /// the func create a copy of the stations list and returns it
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> ListStation()
        {
            List<Station> temp = new();
            foreach (Station item in DataSource.StationsList)
            {
                temp.Add(item);
            }
            return temp;
        }

        public int NumOfChargingNow(int id)
        {
            int x=0;
            foreach (DroneCharge item in DataSource.DChargeList)
            {
                if (item.StationId == id)
                    x++;
            }
            return x;
        }

        public IEnumerable<Station> GetALLStationsBy(Predicate<Station> P)
        {
            return from d in DataSource.StationsList
                   where P(d)
                   select d;
        }
    }
}
