using System;
using IBL.BO;
//using IDAL.DO;
//using DalObject;
using System.Linq;
using System.Collections.Generic;

namespace IBL
{
    public partial class BL: IBL/*: IBL.IBL*/
    {
        // public IDAL.IDal dl;
        public IDAL.IDal AccessIdal;
        List<DroneToList> DronesBL;
        internal static Random rand;//random
        internal static int chargeRate;

        public BL()
        {
            AccessIdal = new DalObject.DalObject();
            rand =  new Random(DateTime.Now.Millisecond);
            chargeRate = AccessIdal.GetChargeRate();
            DronesBL = (List<DroneToList>)(from item in AccessIdal.GetALLDrone()
                                           select new DroneToList()
                                           {
                                               Id = item.Id,
                                               Model = item.Model,
                                               MaxWeight = (WeightCategories)item.MaxWeight,
                                               Status = 0,
                                               //CurrentLocation = new Location() { Lattitude = item.Lattitude, Longitude = item.Longitude },
                                               // IdOfParcel=item.
                                               Battery = rand.Next(20, 41),
                                               IdOfParcel=-1
                                               

                                           }).ToList();
            foreach(DroneToList item in DronesBL)
            {
                if(item.IdOfParcel!=-1)
                {
                    if(AccessIdal.CheckParcel(item.IdOfParcel))
                    {
                        item.Status = DroneStatuses.Delivery;
                        Station s = closestStation(item.CurrentLocation.Longitude, item.CurrentLocation.Lattitude);
                        Drone dd = GetDrone(item.Id);
                        double ba = amountOfbattery(dd,dd.CurrentLocation, s.StationLocation);
                        item.Battery = rand.Next((int)ba, 101);
                        IDAL.DO.Parcel p = AccessIdal.GetParcel(item.IdOfParcel);
                        DateTime d = new DateTime();
                        if (p.PickedUp == d)
                        {
                           
                            item.CurrentLocation = new Location() { Lattitude = s.StationLocation.Lattitude, Longitude = s.StationLocation.Longitude };
                          
                        }
                        else if (p.Delivered == d)
                        {
                            IDAL.DO.Customer c = AccessIdal.GetCustomer(p.SenderId);

                            item.CurrentLocation = new Location() { Lattitude = c.Lattitude, Longitude = c.Longitude };
                        }
                    }
                }
                if(item.Status!=DroneStatuses.Delivery)
                {
                    int x = rand.Next(0, 2);
                    if (x == 1)
                        item.Status = DroneStatuses.Available;//YOU WROTY DELIVEREY, WHY????#####
                    else
                        item.Status = DroneStatuses.Maintenance;
                }
                if(item.Status == DroneStatuses.Maintenance)
                {
                    IEnumerable<Station> ss = GetAllStations();
                    int index = rand.Next(0, ss.Count());
                    item.CurrentLocation = new Location() { Lattitude = ss.ElementAt(index).StationLocation.Lattitude, Longitude = ss.ElementAt(index).StationLocation.Longitude };
                    item.Battery = rand.Next(20, 41);
                
                }
                if (item.Status == DroneStatuses.Available)
                {
                    //IEnumerable<Customer> cc = GetAllCustomers();
                    //int index = rand.Next(0, cc.Count());
                    //item.CurrentLocation = new Location() { Lattitude = cc.ElementAt(index).CustLocation.Lattitude, Longitude = cc.ElementAt(index).CustLocation.Longitude };

                    ////הגרלות הגרלות#############################33
                    if (item.CurrentLocation == null)
                    {
                        item.CurrentLocation = new Location();
                        item.CurrentLocation.Lattitude = rand.Next(30, 33) + ((double)rand.Next(0, 1000000) / 1000000);
                        item.CurrentLocation.Longitude = rand.Next(34, 36) + ((double)rand.Next(0, 1000000) / 1000000);
                    }
                    Station s = closestStation(item.CurrentLocation.Longitude, item.CurrentLocation.Lattitude);
                    Drone dd = GetDrone(item.Id);
                    double ba = amountOfbattery(dd,dd.CurrentLocation, s.StationLocation);

                }

            }

        }

        private Station closestStation(double lon, double lat)
        {
            double max=10000000000000000000, d = 0;
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
            return GetStation(ids);
        }

        private double amountOfbattery(Drone d, Location l,Location L2)
        {
            double[] arr = AccessIdal.ElectricityUse();
            double s;
            s = getDistanceFromLatLonInKm(l.Lattitude, l.Longitude, L2.Lattitude, L2.Lattitude);

            if (d.Status == DroneStatuses.Available)
            {
                s *= arr[0];
            }
            else
            {
                switch (d.MaxWeight)
                {
                    case WeightCategories.Light:
                        s *= arr[1];
                        break;
                    case WeightCategories.Medium:
                        s *= arr[2];
                        break;
                    case WeightCategories.Heavy:
                        s *= arr[3];
                        break;
                    default:
                        break;
                }
            }
            return s;
        }
        private double Deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }
        public double getDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371; // Radius of the earth in km
            double dLat = Deg2rad(lat2 - lat1);  // deg2rad below
            double dLon = Deg2rad(lon2 - lon1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(Deg2rad(lat1)) * Math.Cos(Deg2rad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2);///calculating by the formula
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c; // Distance in km
            return d;
        }

        public IEnumerable<IDAL.DO.DroneCharge> GetAllDroneCharges()
        {
            return from item in AccessIdal.GetALLDroneCharges()
                   orderby item.DroneId
                   select AccessIdal.GetDroneCharge(item.DroneId);
        }
    }
}
