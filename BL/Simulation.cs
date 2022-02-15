using System;
using BO;
using System.Threading;
using static BL.BL;
using System.Linq;

namespace BL
{
    class Simulation
    {
        public Simulation(BL blObject, int droneId, Action ReportProgressInSimulation, Func<bool> IsTimeRun)
        {
            DalApi.IDal AccessIdal = DalApi.DalFactory.GetDal();
                 DroneToList d = blObject.GetAllDronesToList().First(x=>x.Id==droneId);
            double battery, distance;
            while (!IsTimeRun())
            {
                switch (d.Status)
                {
                    case DroneStatuses.Available:
                        try
                        {
                            blObject.LinkDroneToParcel(droneId);
                            ReportProgressInSimulation();
                        }
                        catch 
                        {
                            if (d.Battery < 100)
                            {
                                battery = d.Battery;
                                Station s = blObject.closestStation(d.CurrentLocation.Longitude, d.CurrentLocation.Lattitude);
                                distance = blObject.getDistanceFromLatLonInKm(d.CurrentLocation.Lattitude, d.CurrentLocation.Longitude, s.StationLocation.Lattitude, s.StationLocation.Longitude);
                                while (distance>0)
                                {
                                    d.Battery -= AccessIdal.ElectricityUse()[0];
                                    ReportProgressInSimulation();
                                    distance -= 1;
                                    Thread.Sleep(500);
                                }

                                d.Battery = battery;
                                blObject.DroneToCharge(droneId);
                                ReportProgressInSimulation();
                            }
                        }
                        break;
                    case DroneStatuses.Delivery:
                        {
                            Parcel p = blObject.GetParcel(d.IdOfParcel);
                            if(p.PickedUp==null)
                            {
                                Location loc = new() { Lattitude = d.CurrentLocation.Lattitude, Longitude = d.CurrentLocation.Longitude };
                                battery = d.Battery;
                                Customer c = blObject.GetCustomer(p.Sender.Id);
                                distance = blObject.getDistanceFromLatLonInKm(d.CurrentLocation.Lattitude, d.CurrentLocation.Longitude, c.CustLocation.Lattitude, c.CustLocation.Longitude);
                                Drone dt = blObject.GetDrone(droneId);
                                double b =blObject.amountOfbattery(dt, d.CurrentLocation,blObject. GetCustomer(p.Sender.Id).CustLocation);
                                b /= distance;
                                b *= 2;
                                while (distance>1)
                                {
                                    d.Battery -= b;// (AccessIdal.ElectricityUse()[(int)p.Weight+1]/10);
                                    distance -= 2;
                                    ReportProgressInSimulation();
                                    Thread.Sleep(100);
                                }
                                d.Battery = battery;
                                d.CurrentLocation = loc;
                                blObject.PickParcel(droneId);
                                ReportProgressInSimulation();
                                Thread.Sleep(500);
                            }
                            else
                            {
                                battery = d.Battery;
                                Customer c = blObject.GetCustomer(p.Target.Id);
                                Location loc = new() { Lattitude = d.CurrentLocation.Lattitude, Longitude = d.CurrentLocation.Longitude };
                                distance = blObject.getDistanceFromLatLonInKm(d.CurrentLocation.Lattitude, d.CurrentLocation.Longitude, c.CustLocation.Lattitude, c.CustLocation.Longitude);
                                Drone dt = blObject.GetDrone(droneId);

                                double b = blObject.amountOfbattery(dt, d.CurrentLocation,blObject. GetCustomer(p.Target.Id).CustLocation);
                                b /= distance;
                                b *= 2;
                                while (distance > 1)
                                {
                                    d.Battery -= b;//(AccessIdal.ElectricityUse()[(int)p.Weight + 1]/10);
                                    distance -= 2;
                                    ReportProgressInSimulation();
                                    Thread.Sleep(100);
                                }
                                d.Battery = battery;
                                d.CurrentLocation = loc;
                                blObject.DeliveringParcel(droneId);
                                ReportProgressInSimulation();
                                Thread.Sleep(500);
                            }
                        }
                        break;
                    case DroneStatuses.Maintenance:
                        {
                            while (d.Battery<100)
                            {
                                d.Battery += 10;
                                if(d.Battery > 100)
                                {
                                    d.Battery = 100;
                                }
                                ReportProgressInSimulation();
                                Thread.Sleep(500);
                            }
                            blObject.EndCharging(droneId);
                            ReportProgressInSimulation();
                        }
                        break;
                    default:
                        break;
                }
                Thread.Sleep(1000);
            }
        }
    }
}
