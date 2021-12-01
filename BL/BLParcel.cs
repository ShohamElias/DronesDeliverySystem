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
            //s.CopyPropertiesTo(parcelBO);
            //parcelDO.CopyPropertiesTo(parcelBO);
            //stationBO.DronesinCharge= from sic in AccessIdal.GetALLDrone(sic=> sic.Id==Id )
            //                          let 
            return parcelBO;

        }
        public void AddParcel(Parcel p /*int sender, int target, WeightCategories w, Priorities p*/) //מה הוא בכלל צרי לקבל
        {
            DateTime d = new DateTime();
            IDAL.DO.Parcel par = new IDAL.DO.Parcel();
            par.Id = p.Id;
            par.SenderId = p.Sender.Id;
            par.TargetId = p.Target.Id;
            par.Weight = (IDAL.DO.WeightCategories)p.Weight;
            par.Priority = (IDAL.DO.Priorities)p.Priority;
            par.Requested = DateTime.Now;
            // DroneId = null,
            par.PickedUp = d;// new DateTime(0, 0, 0);
            par.Scheduled = d;// new DateTime(0, 0, 0);
            par.Delivered = d;// new DateTime(0, 0, 0);

            //{
            //    Id = p.Id,
            //    SenderId = p.Sender.Id,
            //    TargetId = p.Target.Id,
            //    Weight = (IDAL.DO.WeightCategories)p.Weight,
            //    Priority = (IDAL.DO.Priorities)p.Priority,
            //    Requested = DateTime.Now,
            //   // DroneId = null,
            //    PickedUp = new DateTime(0,0,0),
            //    Scheduled = new DateTime(0, 0, 0),
            //    Delivered = new DateTime(0, 0, 0)

            //};
            try
            {
                AccessIdal.AddParcel(par);
            }
            catch (IDAL.DO.IDExistsException)
            {
                throw new IDExistsException("parcel");
            }
        }

        public IEnumerable<Parcel> GetAllParcels()
        {
            return from item in AccessIdal.GetALLParcel()
                   orderby item.Id
                   select GetParcel(item.Id);
        }

        public Parcel GetParcel(int id)
        {
            if (id == -1)
                return new Parcel();
            //if (!AccessIdal.CheckParcel(id))
            //    throw new BadIdException("parcel doesnt exist");
            //IDAL.DO.Parcel p = AccessIdal.GetParcel(id);
            //Parcel pl = new Parcel()
            //{
            //    Id=p.Id,
            //    Sender = new CustomerInParcel() { CustomerName = AccessIdal.GetCustomer(p.SenderId).Name, Id = p.Id },
            //    Target = new CustomerInParcel() { CustomerName = AccessIdal.GetCustomer(p.TargetId).Name, Id = p.Id },
            //    DroneParcel=new DroneInParcel() { Battery=GetDrone(p.DroneId).Battery,Id=p.DroneId,CurrentLocation=GetDrone(p.DroneId).CurrentLocation},
            //    Weight= (WeightCategories)p.Weight,
            //    Priority= (Priorities)p.Priority,
            //    Requested=p.Requested,
            //    Scheduled=p.Scheduled,
            //    Delivered=p.Delivered,
            //    PickedUp=p.PickedUp
            //};
            //return pl;
            IDAL.DO.Parcel p;
            try
            {
                p = AccessIdal.GetParcel(id);
            }
            catch (IDAL.DO.BadIdException)
            {

                throw new BadIdException(id,"parcel");
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
                pl.Sender = new CustomerInParcel() { CustomerName = AccessIdal.GetCustomer(p.SenderId).Name, Id = p.Id };
                pl.Target = new CustomerInParcel() { CustomerName = AccessIdal.GetCustomer(p.TargetId).Name, Id = p.Id };
                pl.DroneParcel = new DroneInParcel() { Battery = GetDrone(p.DroneId).Battery, Id = p.DroneId, CurrentLocation = GetDrone(p.DroneId).CurrentLocation };
            }
            else
            {
                pl.Sender = new CustomerInParcel();
                pl.Target = new CustomerInParcel() ;
                pl.DroneParcel = new DroneInParcel();
            }
            
            return pl;
        }

        public void UpdateParcel(Parcel p)
        {
            IDAL.DO.Parcel pDO = AccessIdal.GetParcel(p.Id);
            pDO.PickedUp = p.PickedUp;
            pDO.Requested = p.Requested;
            pDO.Scheduled = p.Scheduled;
            pDO.Delivered = p.Delivered;
            if(!AccessIdal.CheckDrone(p.DroneParcel.Id))
               pDO.DroneId = p.DroneParcel.Id; //error if null?
            AccessIdal.UpdateParcel(pDO);
        }
        public void PickParcel(int id)
        {
            Drone d;
            try
            {
                d= GetDrone(id);
            }
            catch (Exception e)
            {

                throw new BadIdException("Drone"); //exception e, throw e?????
            }
            DroneToList dt = DronesBL.Find(x => x.Id == id);
            DronesBL.Remove(dt);
            Parcel p;
            try
            {
                 p = GetParcel(dt.IdOfParcel);
            } 
            catch(BadIdException)
            {
                throw new BadIdException("parcel"); //################3
            }
            DateTime dtm = new DateTime(0, 0, 0);
            if (d.Status == DroneStatuses.Delivery && p.Scheduled != dtm)
            {
                double b = amountOfbattery(d,d.CurrentLocation, GetCustomer(p.Sender.Id).CustLocation);
                d.Battery -= b;
                dt.CurrentLocation.Lattitude = GetCustomer(p.Sender.Id).CustLocation.Lattitude;
                dt.CurrentLocation.Longitude = GetCustomer(p.Sender.Id).CustLocation.Longitude;
                p.PickedUp = DateTime.Now;
                DronesBL.Add(dt);
                UpdateParcel(p);

            }
            else
                throw new WrongDroneStatException("there was an issue, parcel couldnt be picked");//########
        }

        public void DeliveringParcel(int id)
        {
            if (!AccessIdal.CheckDrone(id))
                throw new BadIdException("drone doesnt exist");
            DateTime dtm = new DateTime(0, 0, 0);
            DroneToList dt = DronesBL.Find(x => x.Id == id);
            Parcel p;
            try
            {
                p = GetParcel(dt.IdOfParcel);
            }
            catch (BadIdException )
            {
                throw new BadIdException("parcel"); //####################
            }
            Drone d = GetDrone(id);
            DronesBL.Remove(dt);
            if(dt.Status==DroneStatuses.Delivery&&p.PickedUp==dtm)
            {
               double b = amountOfbattery(d,d.CurrentLocation, GetCustomer(p.Target.Id).CustLocation);
                dt.Battery -= b;
                dt.CurrentLocation.Lattitude = GetCustomer(p.Target.Id).CustLocation.Lattitude;
                dt.CurrentLocation.Longitude = GetCustomer(p.Target.Id).CustLocation.Longitude;
                dt.Status = DroneStatuses.Available;
                p.Delivered = DateTime.Now;
                UpdateParcel(p);
                DronesBL.Add(dt);

            }
        }

        public string ShowOneParcel(int _id)
        {
            Parcel s = GetParcel(_id); //finding the station by its id
            return s.ToString();
        }
    }
}
