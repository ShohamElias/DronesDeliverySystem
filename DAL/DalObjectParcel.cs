using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalObject
{
     partial class DalObject : DalApi.IDal
    {
       
        /// <summary>
        ///the function adds a new parcel to the list
        /// </summary>
        /// <param name="per">the given object</param>
        public  void AddParcel(Parcel per)
        {
            per.Id = DataSource.Config.countIdParcel++;
            DataSource.ParcelsList.Add(per);
        }

        public void UpdateParcel(Parcel newD)
        {
            Parcel d2 = DataSource.ParcelsList.Find(x => x.Id == newD.Id); //finding the station by its id
            if (d2.Id != newD.Id)
                throw new DO.BadIdException(newD.Id, "This Parcel does not exist");
            DataSource.ParcelsList.Remove(d2);
            DataSource.ParcelsList.Add(newD);
        }

        public Parcel GetParcel(int id)
        {
            if (!CheckParcel(id))
                throw new DO.BadIdException(id, "Parcel id doesnt exist: ");

            Parcel p = DataSource.ParcelsList.Find(p => p.Id == id);
            return p;
        }

        public bool CheckParcel(int id)
        {
            return DataSource.ParcelsList.Any(p => p.Id == id);
        }
        public void RemoveParcel(int id)
        {
            if (!CheckParcel(id))
                throw new BadIdException(id, "this parcel doesnt exist");
            DataSource.ParcelsList.Remove(GetParcel(id));
        }
        public IEnumerable<Parcel> GetALLParcel()
        {
            return from p in DataSource.ParcelsList
                   select p;
        }

        public IEnumerable<Parcel> GetAllUnMachedParcel()
        {

            return from item in GetALLParcel()
                   where item.DroneId == 0
                   select GetParcel(item.Id);
        }

        /// <summary>
        /// the function update a parcel that was picked and the drone that picked it
        /// </summary>
        /// <param name="parcelId">the parcel to pick</param>
        public  void PickParcel(int parcelId)
        {
            Parcel p = DataSource.ParcelsList.Find(x => x.Id == parcelId); //finding the parcel by its id
            if (p.Id != parcelId)
                throw new DO.BadIdException(parcelId, "This parcel id doesnt exists");
            DataSource.ParcelsList.Remove(p);
            p.PickedUp = DateTime.Now;
            DataSource.ParcelsList.Add(p);
            Drone d = DataSource.DroneList.Find(x => x.Id == p.DroneId); //finding the drone that was connected to the parcel by its id
            //d.Status = DroneStatuses.Delivery;   //updating its status#################
        }

        /// <summary>
        /// //the function update a parcel that was dlivered
        /// </summary>
        /// <param name="parcelId"></param>The given id of a parcel
        public  void DeliveringParcel(int parcelId)
        {
            Parcel p = DataSource.ParcelsList.Find(x => x.Id == parcelId); //finding the parcel by its id
            if (p.Id != parcelId) 
                throw new DO.BadIdException(parcelId, "This parcel id doesnt exists");
            DataSource.ParcelsList.Remove(p);
            p.Delivered = DateTime.Now;
            DataSource.ParcelsList.Add(p); Drone d = DataSource.DroneList.Find(x => x.Id == p.DroneId); //finding the drone that was connected to the parcel by its id
            //d.Status = DroneStatuses.Available;   //updating its status###################

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

        public IEnumerable<Parcel> GetALLParcelsBy(Predicate<Parcel> P)
        {
            return from d in DataSource.ParcelsList
                   where P(d)
                   select d;
        }

    }
}
