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
        public void AddParcel( int sender, int target, WeightCategories w, Priorities p) //מה הוא בכלל צרי לקבל
        {
            IDAL.DO.Parcel par = new IDAL.DO.Parcel()
            {
                //Id = newParcel.Id,
                SenderId =sender,
                TargetId = target,
                Weight = (IDAL.DO.WeightCategories)w,
                Priority = (IDAL.DO.Priorities)p,
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
                   select new Parcel()
                   {
                       Id = item.Id,
                       Sender = new CustomerInParcel() { Id = item.Id, CustomerName = AccessIdal.GetCustomer(item.SenderId).Name },
                       Target = new CustomerInParcel() { Id = item.Id, CustomerName = AccessIdal.GetCustomer(item.TargetId).Name },
                       DroneParcel = new DroneInParcel() { Id = item.DroneId, Battery = AccessIdal.GetDrone(item.DroneId).Battery, CurrentLocation = new Location() { Lattitude = AccessIdal.GetDrone(item.DroneId).Lattitude, Longitude= AccessIdal.GetDrone(item.DroneId).Lattitude } }, ////????
                       //???
                      Weight= (WeightCategories)item.Weight,
                      Priority=(Priorities)item.Priority,
                      Requested=item.Requested,
                      Delivered=item.Delivered,
                      PickedUp=item.PickedUp,
                      Scheduled=item.Scheduled
                      
             
                   };
        }



    }
}
