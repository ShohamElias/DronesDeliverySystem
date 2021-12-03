using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;


namespace IBL
{
    public partial class BL
    {
        /// <summary>
        /// adapter- gets a DO parcel and returns a BO parcel
        /// </summary>
        /// <param name="parcelDO"></param> the DO parcel
        /// <returns></returns> BO parcel
        private Parcel parcelDoBoAdapter(IDAL.DO.Parcel parcelDO)
        {
            Parcel parcelBO = new Parcel();
            int id = parcelDO.Id;
            IDAL.DO.Parcel s;
            try //???
            {
                s = AccessIdal.GetParcel(id);
            }
            catch (IDAL.DO.BadIdException)
            {

                throw new BadIdException("station");
            }
            return parcelBO;

        }
        /// <summary>
        /// gets a parcel and add it to the list
        /// </summary>
        /// <param name="p"></param>
        public void AddParcel(Parcel p ) 
        {
            DateTime d = new DateTime();
            IDAL.DO.Parcel par = new IDAL.DO.Parcel();
            par.Id = p.Id;
            par.SenderId = p.Sender.Id;
            par.TargetId = p.Target.Id;
            par.Weight = (IDAL.DO.WeightCategories)p.Weight;
            par.Priority = (IDAL.DO.Priorities)p.Priority;
            par.Requested = DateTime.Now;
  
            par.PickedUp = d;
            par.Scheduled = d;
            par.Delivered = d;

            try
            {
                AccessIdal.AddParcel(par);
            }
            catch (IDAL.DO.IDExistsException)
            {
                throw new IDExistsException(par.Id,"this parcel already exists");
            }
        }
        /// <summary>
        /// returns all the parcel in the list
        /// </summary>
        /// <returns></returns> list of parcels
        public IEnumerable<Parcel> GetAllParcels()
        {
            return from item in AccessIdal.GetALLParcel()
                   orderby item.Id
                   select GetParcel(item.Id);
        }
        /// <summary>
        /// gets and id of a parcel and returns the parcel
        /// </summary>
        /// <param name="id"></param> id of parcel
        /// <returns></returns> parcel
        public Parcel GetParcel(int id)
        {
            if (id == -1)
                return new Parcel();
            IDAL.DO.Parcel p;
            try
            {
                p = AccessIdal.GetParcel(id);
            }
            catch (IDAL.DO.BadIdException)
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
                PickedUp = p.PickedUp
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
        /// <summary>
        /// gets a parcel and update it
        /// </summary>
        /// <param name="p"></param> the parcel
        public void UpdateParcel(Parcel p)
        {
            IDAL.DO.Parcel pDO = AccessIdal.GetParcel(p.Id);
            pDO.PickedUp = p.PickedUp;
            pDO.Requested = p.Requested;
            pDO.Scheduled = p.Scheduled;
            pDO.Delivered = p.Delivered;
            if (!AccessIdal.CheckDrone(p.DroneParcel.Id))
                pDO.DroneId = p.DroneParcel.Id; //error if null?
            AccessIdal.UpdateParcel(pDO);
        }
        /// <summary>
        /// gets and id of drone and update the parcel to be picked
        /// </summary>
        /// <param name="id"></param> id of drone
        public void PickParcel(int id)
        {
            Drone d;
            try
            {
                d = GetDrone(id);
            }
            catch (IDAL.DO.BadIdException)
            {

                throw new BadIdException(id,"this Drone doesn;t exist"); 
            }
            DroneToList dt = DronesBL.Find(x => x.Id == id);
            DroneToList dss= DronesBL.Find(x => x.Id == id);
            
            Parcel p;
            try
            {
                p = GetParcel(dt.IdOfParcel);
            }
            catch (BadIdException)
            {
                throw new BadIdException(dt.IdOfParcel,"this parcel doesn't exist");
            }
            DateTime dtm = new DateTime();
            if (d.Status == DroneStatuses.Delivery && p.Scheduled != dtm)
            {
                double b = amountOfbattery(d, d.CurrentLocation, GetCustomer(p.Sender.Id).CustLocation);
                d.Battery -= b;
                dt.CurrentLocation.Lattitude = GetCustomer(p.Sender.Id).CustLocation.Lattitude;
                dt.CurrentLocation.Longitude = GetCustomer(p.Sender.Id).CustLocation.Longitude;
                p.PickedUp = DateTime.Now;
                DronesBL.Remove(dss);
                DronesBL.Add(dt);
                UpdateParcel(p);

            }
            else
                throw new WrongDroneStatException("there was an issue, parcel couldnt be picked");//########
            
        }
        /// <summary>
        /// gets an id of drone and update its parcel to be delivered
        /// </summary>
        /// <param name="id"></param> id of drone
        public void DeliveringParcel(int id)
        {
            if (!AccessIdal.CheckDrone(id))
                throw new BadIdException(id, "drone doesnt exist");
            DateTime dtm = new DateTime();
            DroneToList dt = DronesBL.Find(x => x.Id == id);
            Parcel p;
            try
            {
                p = GetParcel(dt.IdOfParcel);
            }
            catch (BadIdException)
            {
                throw new BadIdException(dt.IdOfParcel,"this parcel doesnt exist"); 
            }
            Drone d = GetDrone(id);
            if (dt.Status == DroneStatuses.Delivery && p.PickedUp != dtm)
            {
                double b = amountOfbattery(d, d.CurrentLocation, GetCustomer(p.Target.Id).CustLocation);
                dt.Battery -= b;
                dt.CurrentLocation.Lattitude = GetCustomer(p.Target.Id).CustLocation.Lattitude;
                dt.CurrentLocation.Longitude = GetCustomer(p.Target.Id).CustLocation.Longitude;
                dt.Status = DroneStatuses.Available;
                p.Delivered = DateTime.Now;
                UpdateParcel(p);
                DronesBL.Remove(DronesBL.Find(x => x.Id == id));
                DronesBL.Add(dt);

            }
        }
        /// <summary>
        /// gets an id of parcel and returns a string output
        /// </summary>
        /// <param name="_id"></param> id of parcel
        /// <returns></returns> string of the parcel items
        public string ShowOneParcel(int _id)
        {
            Parcel s = GetParcel(_id); //finding the station by its id
            return s.ToString();
        }
        /// <summary>
        /// returbs all of the unmached parcels
        /// </summary>
        /// <returns></returns> list of parcels
        public IEnumerable<Parcel> GetAllUnmachedParcels()
        {
            return from item in GetAllParcels()
                   where item.DroneParcel.Id == 0
                   select GetParcel(item.Id);
        }
    }
}
