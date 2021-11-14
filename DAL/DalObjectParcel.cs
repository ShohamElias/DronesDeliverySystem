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
        ///the function adds a new parcel to the list
        /// </summary>
        /// <param name="per">the given object</param>
        public  void AddParcel(Parcel per)
        {
            DataSource.ParcelsList.Add(per);
        }

        /// <summary>
        /// the function update a parcel that was picked and the drone that picked it
        /// </summary>
        /// <param name="parcelId">the parcel to pick</param>
        public  void PickParcel(int parcelId)
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
        public  void DeliveringParcel(int parcelId)
        {
            Parcel p = DataSource.ParcelsList.Find(x => x.Id == parcelId); //finding the parcel by its id
            p.Delivered = DateTime.Now; //adding delivering time
            Drone d = DataSource.DroneList.Find(x => x.Id == p.DroneId); //finding the drone that was connected to the parcel by its id
            d.Status = DroneStatuses.Available;   //updating its status

        }

       
        /// <summary>
        /// the function gets an id and prints the parcel with the same id
        /// </summary>
        /// <param name="_id"></param>the given parcels id 
        /// <returns></returns>
        public string ShowOneParcel(int _id)
        {
            Parcel p = DataSource.ParcelsList.Find(x => x.Id == _id); //finding the parcel by its id
            return p.ToString();
        }

        /// <summary>
        /// the func create a copy of the parcel list and returns it
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> ListParcel()
        {
            List<Parcel> temp = new();
            foreach (Parcel item in DataSource.ParcelsList)
            {
                temp.Add(item);
            }
            return temp;
        }

    }
}
