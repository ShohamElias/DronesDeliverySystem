using System;
using IBL.BO;
//using IDAL.DO;
//using DalObject;
using System.Linq;
using System.Collections.Generic;

namespace IBL
{
    public partial class BL: IBL.IBL
    {
        // public IDAL.IDal dl;
        public IDAL.IDal AccessIdal;
        List<DroneToList> DronesBL;
        internal static Random rand;//random


        public BL()
        {
            AccessIdal = new DalObject.DalObject();
            rand =  new Random(DateTime.Now.Millisecond);
            DronesBL = (List<DroneToList>)(from item in AccessIdal.GetALLDrone()
                                           select new DroneToList()
                                           {
                                               Id = item.Id,
                                               Model = item.Model,
                                               MaxWeight = (WeightCategories)item.MaxWeight,
                                               Status = 0,
                                               CurrentLocation = new Location() { Lattitude = item.Lattitude, Longitude = item.Longitude },
                                               // IdOfParcel=item.
                                               Battery = rand.Next(20, 41),
                                               IdOfParcel=-1
                                               

                                           });
            foreach(DroneToList item in DronesBL)
            {
                if(item.IdOfParcel!=-1)
                {
                    if(AccessIdal.CheckParcel(item.IdOfParcel))
                    {
                        item.Status = (DroneStatuses)2;
                        //battery status
                        IDAL.DO.Parcel p = AccessIdal.GetParcel(item.IdOfParcel);
                        DateTime d = new DateTime(0, 0, 0);
                        if(p.PickedUp==d)
                        {
                            //פונק של מרחק
                        }
                        else if(p.Delivered==d)
                        {
                            IDAL.DO.Customer c = AccessIdal.GetCustomer(p.SenderId);

                            item.CurrentLocation = new Location() { Lattitude = c.Lattitude, Longitude = c.Longitude };
                        }
                    }
                }
                if(item.Status!=DroneStatuses.Delivery)
                {
                    //avaitabal/maintains
                }
                if(item.Status == DroneStatuses.Maintenance)
                {
                    //הגרלה בין תחנות
                    item.Battery = rand.Next(20, 41);
                
                }
                if (item.Status == DroneStatuses.Available)
                {
                    //הגרלות הגרלות
                }

            }

        }

        private IDAL.DO.Station dis(double lon, double lat)
        {
            double max=0, d = 0;
            int ids=0;
            foreach (IDAL.DO.Station  item in AccessIdal.GetALLStation())
            {
                d = AccessIdal.StationDistance(lat, lon, item.Id);
                if(d<max)
                {
                    max = d;
                    ids = item.Id;
                }
            }

            return AccessIdal.GetStation(ids);
        }
    }
}
