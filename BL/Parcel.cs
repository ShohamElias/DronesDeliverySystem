using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{ 
        public class Parcel
        {
            public int Id { get; set; }

            public CustomerInParcel Sender { get; set; }

            public CustomerInParcel Target { get; set; }


            //public int SenderId { get; set; }
           // public int Id { get; set; }

            public DroneInParcel DroneParcel { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public DateTime? Requested { get; set; }
           // public int DroneId { get; set; }
            public DateTime? Scheduled { get; set; }
            public DateTime? PickedUp { get; set; }
            public DateTime? Delivered { get; set; }
            public bool? IsPicked { get; set; }
            public bool? IsDelivered { get; set; }


        public override string ToString()
            {
                return $"Parcel: Id = {Id}, SenderId = {Sender.Id}, TargetId= {Target.Id}, Weight = {Weight}, Priority = {Priority}, Requested = {Requested}, DroneId ={DroneParcel.Id}, Scheduled = {Scheduled}, PickedUp = {PickedUp}, Delivered = {Delivered}";
            }



        }
    }