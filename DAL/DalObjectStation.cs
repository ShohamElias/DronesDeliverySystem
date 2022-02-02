using System;
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


        /// <summary>
        /// the function gets a location and station id, and calculates the distance between the station and location
        /// </summary>
        /// <param name="lat">the location latitude</param>
        /// <param name="lon1">longtitude</param>
        /// <param name="id">the station id</param>
        /// <returns>the distance</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double StationDistance(double lat, double lon1, int id)
        {
            Station d = DataSource.StationsList.Find(x => x.Id == id);//finding the station in the list
            if (d.Id != id)
                throw new DO.BadIdException(id, "This id doesnt exists");
            return getDistanceFromLatLonInKm(lat, lon1, d.Lattitude, d.Longitude);//sending to the func to calculate
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStation(Station newD)
        {
            Station d2 = DataSource.StationsList.Find(x => x.Id == newD.Id); //finding the station by its id
            if (d2.Id != newD.Id)
                throw new DO.BadIdException(newD.Id, "This Station does not exist");
            DataSource.StationsList.Remove(d2);
            DataSource.StationsList.Add(newD);
        }


        /// <summary>
        /// the function adds a new station to the list 
        /// </summary>
        /// <param name="s">the given object</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(Station s)
        {
            Station s2 = DataSource.StationsList.Find(x => x.Id == s.Id); //finding the station by its id
            if (s2.Id == s.Id)
                throw new DO.IDExistsException(s.Id, "This id already exists");
            DataSource.StationsList.Add(s);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int id)
        {
            if (!CheckStation(id))
                throw new DO.BadIdException(id, "Station id doesnt exist: ");

            Station stat1 = DataSource.StationsList.Find(stat1 => stat1.Id == id);
            return stat1;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool CheckStation(int id)
        {
            return DataSource.StationsList.Any(stat1 => stat1.Id == id);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetALLStation()
        {
            return from stat in DataSource.StationsList
                   select stat;
        }


        /// <summary>
        /// the func create a copy of the stations list and returns it
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> ListStation()
        {
            List<Station> temp = new();
            foreach (Station item in DataSource.StationsList)
            {
                temp.Add(item);
            }
            return temp;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetALLStationsBy(Predicate<Station> P)
        {
            return from d in DataSource.StationsList
                   where P(d)
                   select d;
        }
    }
}
