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
        public void AddParcel(Parcel newParcel)
        {
            IDAL.DO.Parcel par = new IDAL.DO.Parcel()
            {
                Id = newParcel.Id,
                SenderId = newParcel.Sender.Id,
                TargetId = newParcel.Target.Id,
                Weight = (IDAL.DO.WeightCategories)newParcel.Weight,
                Priority = (IDAL.DO.Priorities)newParcel.Priority,
                Requested = newParcel.Requested,
                DroneId = newParcel.DroneParcel.Id,
                PickedUp = newParcel.PickedUp,
                Scheduled = newParcel.Scheduled,
                Delivered = newParcel.Delivered

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
