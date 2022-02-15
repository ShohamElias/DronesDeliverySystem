using System;
using BO;
using System.Linq;
using System.Collections.Generic;
using BlApi;
using DalApi;
using System.Runtime.CompilerServices;

namespace BL
{
    sealed partial class BL: IBL
    {
        internal DalApi.IDal AccessIdal;

        public List<DroneToList> DronesBL;
        internal static Random rand;//random
        internal static double chargeRate;
        public static int parcelNum;

        internal static readonly IBL instance = new BL();
        public static IBL Instance { get => instance; }


        public BL()
        {
        //    lock (AccessIdal)
        //    {

                AccessIdal = DalApi.DalFactory.GetDal();
                rand = new Random(DateTime.Now.Millisecond);
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
                                                   IdOfParcel = -1

                                               }).ToList();
            //}
            foreach (var item in DronesBL)
            {
                item.CurrentLocation = new Location();
                item.CurrentLocation.Lattitude = rand.Next(30, 33) + ((double)rand.Next(0, 1000000) / 1000000);
                item.CurrentLocation.Longitude = rand.Next(34, 36) + ((double)rand.Next(0, 1000000) / 1000000);

            }

            foreach (Station s in GetAllStations())
            {
                foreach (var item in s.DronesinCharge)
                {
                    DroneToList d = DronesBL.Find(x => x.Id == item.DroneId);
                    if(d!=null)
                    {
                        d.CurrentLocation = s.StationLocation;
                        d.Status = DroneStatuses.Maintenance;
                        DronesBL.Remove(DronesBL.Find(x => x.Id == item.DroneId));
                        DronesBL.Add(d);
                    }
                }
            }

            foreach (var item in GetAllParcels())
            {
                if(item.DroneParcel.Id>0 && item.Delivered == null)
                {
                    DroneToList d = DronesBL.Find(x => x.Id == item.DroneParcel.Id);
                    DroneToList s = d;
                    d.Status = DroneStatuses.Delivery;
                    d.IdOfParcel = item.Id;
                    
                        d.CurrentLocation = GetCustomer(item.Sender.Id).CustLocation;
                    //}
                  //  DronesBL.Remove(s);
                    //DronesBL.Add(d);
                }
            }
            
            foreach (var item in DronesBL)
            {
                if (item.Status == DroneStatuses.Available)
                {
                    item.CurrentLocation = new Location();
                    item.CurrentLocation.Lattitude = rand.Next(30, 33) + ((double)rand.Next(0, 1000000) / 1000000);
                    item.CurrentLocation.Longitude = rand.Next(34, 36) + ((double)rand.Next(0, 1000000) / 1000000);
                }
            }
        }
        public void simulator(int droneId, Action updateWPF, Func<bool> check)
        {
            new Simulation(this, droneId,  updateWPF, check);
        }

        internal Station closestStation(double lon, double lat)
        {
            lock (AccessIdal)
            {

                double max = double.MaxValue, d = 0;
                int ids = 0;
                foreach (DO.Station item in AccessIdal.GetALLStation())
                {
                    d = AccessIdal.StationDistance(lat, lon, item.Id);
                    if (d < max)
                    {
                        max = d;
                        ids = item.Id;
                    }
                }

                return GetStation(ids);
            }
        }

        internal double amountOfbattery(Drone d, Location l,Location L2)
        {
            lock (AccessIdal)
            {
                double[] arr = AccessIdal.ElectricityUse();
                double s;
                s = getDistanceFromLatLonInKm(l.Lattitude, l.Longitude, L2.Lattitude, L2.Longitude);

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
                return s/50 /*/ 100*/;
            }
        }
        private double Deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DO.DroneCharge> GetAllDroneCharges()
        {
            lock (AccessIdal)
            {

                return from item in AccessIdal.GetALLDroneCharges()
                       orderby item.DroneId
                       select AccessIdal.GetDroneCharge(item.DroneId);
            }
        }

        
    }
}
