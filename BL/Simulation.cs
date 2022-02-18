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
                                try
                                {
                                    Station s = blObject.closestStation(d.CurrentLocation.Longitude, d.CurrentLocation.Lattitude);
                                    if (s.Id == 0)
                                    {
                                        throw;
                                    }
                                    distance = blObject.getDistanceFromLatLonInKm(d.CurrentLocation.Lattitude, d.CurrentLocation.Longitude, s.StationLocation.Lattitude, s.StationLocation.Longitude);
                                    Drone dt = blObject.GetDrone(droneId);
                                    double b = blObject.amountOfbattery(dt, d.CurrentLocation, s.StationLocation);
                                    b /= distance;

                                    while (distance > 1)
                                    {
                                        d.Battery -= b * 10;
                                        ReportProgressInSimulation();
                                        distance -= 10;
                                        Thread.Sleep(200);
                                    }

                                    d.Battery = battery;
                                    blObject.DroneToCharge(droneId);
                                    ReportProgressInSimulation();
                                }
                                catch
                                {

                                }
                                Thread.Sleep(500);

                            }
                        }
                        break;
                    case DroneStatuses.Delivery:
                        {
                            Parcel p = blObject.GetParcel(d.IdOfParcel);
                            double ELECTRICITY = AccessIdal.ElectricityUse()[(int)p.Weight + 1];
                            if (p.PickedUp==null)
                            {
                                Location loc = new() { Lattitude = d.CurrentLocation.Lattitude, Longitude = d.CurrentLocation.Longitude };
                                battery = d.Battery;
                                Customer c = blObject.GetCustomer(p.Sender.Id);
                                distance = blObject.getDistanceFromLatLonInKm(d.CurrentLocation.Lattitude, d.CurrentLocation.Longitude, c.CustLocation.Lattitude, c.CustLocation.Longitude);
                                Drone dt = blObject.GetDrone(droneId);
                                
                                while (distance>=2)
                                {
                                    d.Battery -= ELECTRICITY*2;
                                    distance -= 2;
                                    ReportProgressInSimulation();
                                    Thread.Sleep(20);
                                }
                                d.Battery = battery;
                                d.CurrentLocation = loc;
                                blObject.PickParcel(droneId);
                                ReportProgressInSimulation();
                                Thread.Sleep(100);
                            }
                            //from sender to target
                            {
                                battery = d.Battery;
                                Customer c = blObject.GetCustomer(p.Target.Id);
                                Location loc = new() { Lattitude = d.CurrentLocation.Lattitude, Longitude = d.CurrentLocation.Longitude };
                                distance = blObject.getDistanceFromLatLonInKm(d.CurrentLocation.Lattitude, d.CurrentLocation.Longitude, c.CustLocation.Lattitude, c.CustLocation.Longitude);
                                Drone dt = blObject.GetDrone(droneId);

                                while (distance >= 2)
                                {
                                    d.Battery -= ELECTRICITY * 2;
                                    distance -= 2;
                                    ReportProgressInSimulation();
                                    Thread.Sleep(50);
                                }
                                d.Battery = battery;
                                d.CurrentLocation = loc;
                                blObject.DeliveringParcel(droneId);
                                ReportProgressInSimulation();
                                Thread.Sleep(100);
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
                Thread.Sleep(500);
            }
        }
    }
}
