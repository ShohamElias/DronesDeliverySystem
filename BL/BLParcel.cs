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
            s.CopyPropertiesTo(parcelBO);
            parcelDO.CopyPropertiesTo(parcelBO);
            //stationBO.DronesinCharge= from sic in AccessIdal.GetALLDrone(sic=> sic.Id==Id )
            //                          let 
            return parcelBO;

        }
        public void AddParcel(Parcel p /*int sender, int target, WeightCategories w, Priorities p*/) //מה הוא בכלל צרי לקבל
        {
            IDAL.DO.Parcel par = new IDAL.DO.Parcel()
            {
                //Id = newParcel.Id,
                SenderId =p.Sender.Id,
                TargetId = p.Target.Id,
                Weight = (IDAL.DO.WeightCategories)p.Weight,
                Priority = (IDAL.DO.Priorities)p.Priority,
                Requested = DateTime.Now,
               // DroneId = null,
                PickedUp = new DateTime(0,0,0),
                Scheduled = new DateTime(0, 0, 0),
                Delivered = new DateTime(0, 0, 0)

            };
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
            if (!AccessIdal.CheckParcel(id))
                throw new BadIdException("parcel doesnt exist");
            IDAL.DO.Parcel p = AccessIdal.GetParcel(id);
            Parcel pl = new Parcel()
            {
                Id=p.Id,
                Sender = new CustomerInParcel() { CustomerName = AccessIdal.GetCustomer(p.SenderId).Name, Id = p.Id },
                Target = new CustomerInParcel() { CustomerName = AccessIdal.GetCustomer(p.TargetId).Name, Id = p.Id },
                DroneParcel=new DroneInParcel() { Battery=GetDrone(p.DroneId).Battery,Id=p.DroneId,CurrentLocation=GetDrone(p.DroneId).CurrentLocation},
                Weight= (WeightCategories)p.Weight,
                Priority= (Priorities)p.Priority,
                Requested=p.Requested,
                Scheduled=p.Scheduled,
                Delivered=p.Delivered,
                PickedUp=p.PickedUp
            };
            return pl;
        }
    }
}
