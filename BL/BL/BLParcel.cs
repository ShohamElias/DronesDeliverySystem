using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BlApi;
using System.Runtime.CompilerServices;


namespace BL
{
    partial class BL
    {
        /// <summary>
        /// gets a parcel and add it to the list
        /// </summary>
        /// <param name="p"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddParcel(Parcel p)
        {
            DO.Parcel par = new DO.Parcel();
            try
            {
                GetCustomer(p.Sender.Id);
                GetCustomer(p.Target.Id);

            }
            catch (DO.BadIdException)
            {
                throw new BadIdException(p.Sender.Id, "this target or sender  ID doesnt exists");
            }
            par.Weight = (DO.WeightCategories)p.Weight;
            par.Priority = (DO.Priorities)p.Priority;
            par.Requested = p.Requested;
            par.Id = p.Id;
            par.SenderId = p.Sender.Id;
            par.TargetId = p.Target.Id;
            par.PickedUp = null;
            par.IsDelivered = false;
            par.IsPicked = false;
            par.Scheduled = null;
            par.Delivered = null;
            lock (AccessIdal)
            {
                AccessIdal.AddParcel(par);
            }
        }

        /// <summary>
        /// returns all the parcel in the list
        /// </summary>
        /// <returns></returns> list of parcels
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetAllParcels()
        {
            lock (AccessIdal)
            {
                return from item in AccessIdal.GetALLParcel()
                       orderby item.Id
                       select GetParcel(item.Id);
            }
        }

        /// <summary>
        /// gets and id of a parcel and returns the parcel
        /// </summary>
        /// <param name="id"></param> id of parcel
        /// <returns></returns> parcel
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int id)
        {
            if (id == -1)
                return new Parcel();
            DO.Parcel p;
            lock (AccessIdal)
            {
                try
                {
                    p = AccessIdal.GetParcel(id);
                }
                catch (DO.BadIdException)
                {
                    throw new BadIdException(id, "this parcel doesn't exist");
                }
                Parcel pl = new Parcel()
                {
                    Id = p.Id,
                    Weight = (WeightCategories)p.Weight,
                    Priority = (Priorities)p.Priority,
                    Requested = p.Requested,
                    Scheduled = p.Scheduled,
                    Delivered = p.Delivered,
                    PickedUp = p.PickedUp,
                    IsPicked = p.IsPicked,
                    IsDelivered = p.IsDelivered
                };
                if (p.SenderId != 0)
                {
                    pl.Sender = new CustomerInParcel() { CustomerName = AccessIdal.GetCustomer(p.SenderId).Name, Id = p.SenderId };
                    pl.Target = new CustomerInParcel() { CustomerName = AccessIdal.GetCustomer(p.TargetId).Name, Id = p.TargetId };
                    if (p.DroneId > 0)
                    {
                        DroneToList dt = DronesBL.Find(x => x.Id == p.DroneId);
                        pl.DroneParcel = new DroneInParcel() { Battery = dt.Battery, Id = p.DroneId, CurrentLocation = new Location() { Longitude = dt.CurrentLocation.Longitude, Lattitude = dt.CurrentLocation.Lattitude } };
                    }
                }
                else if (p.SenderId == 0)
                {
                    pl.Sender = new CustomerInParcel();
                    pl.Target = new CustomerInParcel();
                    pl.DroneParcel = new DroneInParcel();
                }

                if (p.DroneId <= 0)
                {
                    pl.DroneParcel = new DroneInParcel() { Id = 0 };
                }
                return pl;
            }
        }

        /// <summary>
        /// gets a parcel and update it
        /// </summary>
        /// <param name="p"></param> the parcel
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcel(Parcel p)
        {
            lock (AccessIdal)
            {
                DO.Parcel pDO = AccessIdal.GetParcel(p.Id);
                pDO.PickedUp = p.PickedUp;
                pDO.Requested = p.Requested;
                pDO.Scheduled = p.Scheduled;
                pDO.Delivered = p.Delivered;
                pDO.IsDelivered = p.IsDelivered;
                pDO.IsPicked = p.IsPicked;
                if (!AccessIdal.CheckDrone(p.DroneParcel.Id))
                    pDO.DroneId = p.DroneParcel.Id; //error if null?
                AccessIdal.UpdateParcel(pDO);
            }
        }

        /// <summary>
        /// gets and id of drone and update the parcel to be picked
        /// </summary>
        /// <param name="id"></param> id of drone
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PickParcel(int id)
        {
            Drone d;
            try
            {
                d = GetDrone(id);
            }
            catch (DO.BadIdException)
            {

                throw new BadIdException(id, "this Drone doesnt exist");
            }
            DroneToList dt = DronesBL.Find(x => x.Id == id);
            Parcel p;
            try
            {
                p = GetParcel(dt.IdOfParcel);
            }
            catch (BadIdException)
            {
                throw new BadIdException(dt.IdOfParcel, "this parcel doesn't exist");
            }
            if (d.Status == DroneStatuses.Delivery && p.Scheduled != null)
            {
                double b = amountOfbattery(d, d.CurrentLocation, GetCustomer(p.Sender.Id).CustLocation);
                d.Battery -= b;//#######
                dt.Battery -= b;
                if (dt.Battery < 0)
                    dt.Battery = 0;
                dt.CurrentLocation.Lattitude = GetCustomer(p.Sender.Id).CustLocation.Lattitude;
                dt.CurrentLocation.Longitude = GetCustomer(p.Sender.Id).CustLocation.Longitude;
                p.PickedUp = DateTime.Now;
                UpdateParcel(p);
            }
            else
                throw new WrongDroneStatException("there was an issue, parcel couldnt be picked");//########

        }

        /// <summary>
        /// gets an id of drone and update its parcel to be delivered
        /// </summary>
        /// <param name="id"></param> id of drone
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeliveringParcel(int id)
        {
            lock (AccessIdal)
            {
                if (!AccessIdal.CheckDrone(id))
                    throw new BadIdException(id, "drone doesnt exist");
            }
            DroneToList dt = DronesBL.Find(x => x.Id == id);
            Parcel p;
            try
            {
                p = GetParcel(dt.IdOfParcel);
            }
            catch (BadIdException)
            {
                throw new BadIdException(dt.IdOfParcel, "this parcel doesnt exist");
            }
            Drone d = GetDrone(id);
            if (dt.Status == DroneStatuses.Delivery && p.PickedUp != null)
            {
                double b = amountOfbattery(d, d.CurrentLocation, GetCustomer(p.Target.Id).CustLocation);
                dt.Battery -= b;
                if (dt.Battery < 0)
                    dt.Battery = 0;
                dt.CurrentLocation.Lattitude = GetCustomer(p.Target.Id).CustLocation.Lattitude;
                dt.CurrentLocation.Longitude = GetCustomer(p.Target.Id).CustLocation.Longitude;
                dt.Status = DroneStatuses.Available;
                p.Delivered = DateTime.Now;
                UpdateParcel(p);
            }
            else
                throw new Exception("there was an issue with the parcel or drone status");
        }

        /// <summary>
        /// returbs all of the unmached parcels
        /// </summary>
        /// <returns></returns> list of parcels
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetAllUnmachedParcels()
        {
            lock (AccessIdal)
            {
                return from sic in AccessIdal.GetALLParcelsBy(x=>x.Scheduled==null)
                       select GetParcel(sic.Id);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveParcel(int id)
        {
            lock (AccessIdal)
            {
                try
                {
                    AccessIdal.RemoveParcel(id);
                }
                catch (BadIdException)
                {
                    throw new BadIdException(id, "This parcel doesnt exist");
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int GetNextParcel()
        {
            lock (AccessIdal)
            {
                parcelNum = AccessIdal.getParcelMax();
                return parcelNum;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcelBy(Predicate<Parcel> P)
        {
            return from d in GetAllParcels()
                   where P(d)
                   select d;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> GetAllParcelsToList()
        {
            return from d in GetAllParcels()
                   select GetParcelToList(d.Id);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> GetAllParcelsToListBy(Predicate<Parcel> P)
        {
            return from d in GetAllParcels()
                   where P(d)
                   select GetParcelToList(d.Id);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public ParcelToList GetParcelToList(int id)
        {
            Parcel p;
            try
            {
                p = GetParcel(id);
            }
            catch (DO.BadIdException)
            {

                throw new BadIdException(id, "this Drone doesn;t exist");
            }

            ParcelToList pt = new ParcelToList()
            {
                Id = p.Id,
                Priority = p.Priority,
                Weight = p.Weight,
                SenderName = p.Sender.CustomerName,
                TargetName = p.Target.CustomerName,
            };

            if (p.Delivered != null)
                pt.Status = ParcelStatuses.Delivered;
            else if (p.PickedUp != null)
                pt.Status = ParcelStatuses.Collected;
            else if (p.Scheduled != null)
                pt.Status = ParcelStatuses.Associated;
            else
                pt.Status = ParcelStatuses.Defined;
            return pt;
        }
    }
}
