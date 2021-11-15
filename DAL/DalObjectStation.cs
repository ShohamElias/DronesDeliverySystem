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
            return getDistanceFromLatLonInKm(lat, lon1, d.Lattitude, d.Longitude);//sending to the func to calculate
        }

        /// <summary>
        /// the function adds a new station to the list 
        /// </summary>
        /// <param name="s">the given object</param>
        public  void AddStation(Station s)
        {
            DataSource.StationsList.Add(s);
        }

        /// <summary>
        /// the function gets an id and prints the station with the same id
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public string ShowOneStation(int _id)
        {
            Station s = DataSource.StationsList.Find(x => x.Id == _id); //finding the station by its id
            s.Name = "jhgf";
            return s.ToString();
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

        
    }
}
