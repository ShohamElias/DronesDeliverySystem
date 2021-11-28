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
        private Drone droneDoBoAdapter(IDAL.DO.Drone droneDO)
        {
            Drone droneBO = new Drone();
            int id = droneDO.Id;
            IDAL.DO.Drone s;
            try //???
            {
                s = AccessIdal.GetDrone(id);
            }
            catch (IDAL.DO.BadIdException)
            {

                throw new BadIdException("station");
            }
            s.CopyPropertiesTo(droneBO);
            droneDO.CopyPropertiesTo(droneBO);
            //stationBO.DronesinCharge= from sic in AccessIdal.GetALLDrone(sic=> sic.Id==Id )
            //                          let 
            return droneBO;

        }
        public Drone GetDrone(int id)
        {
            IDAL.DO.Drone d;
            try
            {
                d = AccessIdal.GetDrone(id);
            }
            catch (IDAL.DO.BadIdException)
            {

                throw new BadIdException("Drone");
            }
            //return droneDoBoAdapter(d);
            DroneToList dt = DronesBL.Find(x => x.Id == id);

            Drone db = new Drone()
            {
                Id = id,
                Model = d.Model,
                MaxWeight = (WeightCategories)d.MaxWeight,
                Status = dt.Status,
                Battery = dt.Battery,
                CurrentLocation = dt.CurrentLocation,
                CurrentParcel = new ParcelInTransfer()
                {
                    Id = dt.IdOfParcel,
                    Sender = new CustomerInParcel() { Id = GetParcel(dt.IdOfParcel).Sender.Id, CustomerName = GetParcel(dt.IdOfParcel).Sender.CustomerName },
                    Target = new CustomerInParcel() { Id = GetParcel(dt.IdOfParcel).Target.Id, CustomerName = GetParcel(dt.IdOfParcel).Target.CustomerName },
                    //  PickingLocation=new Location() { Lattitude= GetParcel(dt.IdOfParcel).Sender.}
                    // Sender = GetParcel(dt.IdOfParcel).Sender,////deep????
                    // Target = GetParcel(dt.IdOfParcel).Target,
                    Weight = GetParcel(dt.IdOfParcel).Weight,
                    Priority = GetParcel(dt.IdOfParcel).Priority


                }
                  //CurrentParcel.




            };
            IDAL.DO.Customer c = AccessIdal.GetCustomer(GetParcel(dt.IdOfParcel).Sender.Id);
            db.CurrentParcel.PickingLocation = new Location()
            {
                Lattitude = c.Lattitude,
                Longitude = c.Longitude

            };
            c = AccessIdal.GetCustomer(GetParcel(dt.IdOfParcel).Target.Id);
            db.CurrentParcel.TargetLocation = new Location()
            {
                Longitude = c.Longitude,
                Lattitude = c.Lattitude
            };

            return db;
            /* //Drone droneBL = new Drone();
             //try
             //{
             //    IDAL.DO.Drone droneDal = AccessIdal.GetDrone(id);
             //    droneBL.Id = droneDal.Id;
             //    droneBL.Model = droneDal.Model;
             //    droneBL.MaxWeight = (WeightCategories)droneDal.MaxWeight;
             //    droneBL.Status = (DroneStatuses)droneDal.Status;
             //    droneBL.Battery = droneDal.Battery;
             //    droneBL.CurrentLocation = new Location() { Lattitude = droneDal.Lattitude, Longitude = droneDal.Longitude };
             //   // droneBL.CurrentParcel #################3


             //}
             //catch (IDAL.DO.BadIdException)
             //{

             //    throw new BadIdException("Drone");
             //}
             //return droneBL;*/
        }
          public void AddDrone(Drone d, int stationId/*int id, string model, WeightCategories w, int stationId*/)
        {
            if (!AccessIdal.CheckStation(stationId))
                throw new BadIdException("station");
            IDAL.DO.Station s = AccessIdal.GetStation(stationId);

            IDAL.DO.Drone droneDO = new IDAL.DO.Drone()
            {
                Id = d.Id,
                Model = d.Model,
                MaxWeight = (IDAL.DO.WeightCategories)d.MaxWeight,                
                Lattitude = s.Lattitude,
                Longitude = s.Longitude
            };
            // droneDO.CopyPropertiesTo(droneDO);
            // droneDO.Battery = rand.Next(20, 41); //@@@
            //droneDO.Status=(IDAL.DO.DroneStatuses)2; //@@@
            
            if (s.ChargeSlots <= 0)
                throw;
            try
            {
                AccessIdal.AddDrone(droneDO);
            }
            catch (IDAL.DO.IDExistsException)
            {
                throw new IDExistsException("Drone");
            }

            DroneToList dt = new DroneToList()
            {
                Id = droneDO.Id,
                Battery = rand.Next(20, 41),
                Status = DroneStatuses.Maintenance,
                CurrentLocation = new Location() { Lattitude = s.Lattitude, Longitude = s.Longitude },
                MaxWeight = d.MaxWeight,
                Model = droneDO.Model,
                IdOfParcel = -1

            };
            DronesBL.Add(dt);
            //IDAL.DO.Station s = AccessIdal.GetStation(stationId);

            //IDAL.DO.Drone newDrone = new IDAL.DO.Drone()
            //{
            //    Id = id,
            //    Model = model,
            //    MaxWeight = (IDAL.DO.WeightCategories)w,
            //    Battery = rand.Next(20, 41),
            //    Status = (IDAL.DO.DroneStatuses)2,
            //    Lattitude = s.Lattitude,
            //    Longitude = s.Longitude


            //};
            //try
            //{
            //    AccessIdal.AddDrone(newDrone);
            //}
            //catch (IDAL.DO.IDExistsException)
            //{
            //    throw new IDExistsException("Drone");
            //}

        }
        public IEnumerable<Drone> GetAllDrones()
        {
            return from droneDO in AccessIdal.GetALLDrone()
                   orderby droneDO.Id
                   select droneDoBoAdapter(droneDO);
            //return from item in AccessIdal.GetALLDrone()
            //       select new Drone()
            //       {
            //           Id = item.Id,
            //           Model = item.Model,
            //           MaxWeight = (WeightCategories)item.MaxWeight,
            //           Status= (DroneStatuses)item.Status,
            //           Battery=item.Battery,
            //           CurrentLocation = new Location() { Longitude = item.Longitude, Lattitude = item.Lattitude },  //////??????????
            //           //CurrentParcel~~~~~~~~~~~~~~~~~~~~~######################
            //       };
        }

        public void UpdateDrone(int id, string m)
        {
            if (!AccessIdal.CheckDrone(id))
                throw new BadIdException("dont exist");
            try
            {
                IDAL.DO.Drone d = AccessIdal.GetDrone(id);
                d.Id = id;
                d.Model = m;
                AccessIdal.UpdateDrone(d);
            }
            catch (IDAL.DO.BadIdException)
            {

                throw new BadIdException("drone");
            }
        }

        public void DroneToCharge(int id)
        {
            if (!AccessIdal.CheckDrone(id))
                throw new IDAL.DO.BadIdException("doesnt exust");
            IDAL.DO.Drone d = AccessIdal.GetDrone(id);
            DroneToList dt = DronesBL.Find(x => x.Id == id);
            DronesBL.Remove(dt);

            if (dt.Status != DroneStatuses.Available)
                throw;
            //station and battery

            IDAL.DO.Station s = dis(dt.CurrentLocation.Longitude, dt.CurrentLocation.Lattitude);
            Location l = new Location() { Lattitude = s.Lattitude, Longitude = s.Longitude };
            double b = amountOfbattery(GetDrone(id), l);
            if (b < dt.Battery)
                throw; //אין מספיק בטריה
            dt.Battery -= b;
            dt.CurrentLocation = new Location() { Lattitude = s.Lattitude, Longitude = s.Longitude };
            dt.Status = DroneStatuses.Maintenance;
            // DronesBL.Remove()
            //  UpdateDrone()
            DronesBL.Add(dt);
            Station ss = GetStation(s.Id);
            DroneCharge dc = new DroneCharge() { Battery = dt.Battery, DroneId = dt.Id };
            
            ss.DronesinCharge.Add(dc);

            //##### הוספה של פונ ADD DAL
            ss.ChargeSlots--;
            Updatestation(ss.Id, "", ss.ChargeSlots);
          
           // s.ChargeSlots--;
            //   if(d.Status==)
            // Updatestation(ss.Id);
            //Station ss = new Station()
            //{
            //    Id = s.Id,
            //    ChargeSlots = s.ChargeSlots,
            //    StationLocation = new Location() { Lattitude = s.Lattitude, Longitude = s.Longitude },
            //    Name = s.Name
            //};
        }
        public void EndCharging(int id, int timeI)
        {
            if (!AccessIdal.CheckDrone(id))
                throw;
            Drone d = GetDrone(id);
            if (d.Status != DroneStatuses.Maintenance)
                throw;
            d.Battery += timeI * chargeRate;
            d.Status = DroneStatuses.Available;
            IDAL.DO.DroneCharge dc = AccessIdal.GetDroneCharge(id);
            Station s = GetStation(dc.StationId);
            s.ChargeSlots++;
            AccessIdal.DeleteDroneCharge(id);
            Updatestation(s.Id, "", s.ChargeSlots);

        }

        public void LinkDroneToParcel(int id)
        {
            if (!AccessIdal.CheckDrone(id))
                throw new BadIdException("drone");
            Drone d = GetDrone(id);
            if (d.Status != DroneStatuses.Available)
                throw;
            if(GetAllParcels().Any(x=>x.Priority==Priorities.TBN))
            {
                if(GetAllParcels().Any(x=>x.Weight==d.MaxWeight))
                {
                    Parcel p = closest(d);
                    double b = amountOfbattery(d, GetCustomer(p.Sender.Id).CustLocation);
                    double b2 = amountOfbatteryFrom(d, GetCustomer(p.Sender.Id).CustLocation, GetCustomer(p.Target.Id).CustLocation);
                    Station st = dis(GetCustomer(p.Target.Id).CustLocation.Longitude, GetCustomer(p.Target.Id).CustLocation.Lattitude);
                    double b3 = amountOfbatteryFrom(d, GetCustomer(p.Target.Id).CustLocation, st.StationLocation);
                    if(b+b2+b3>d.Battery)
                        //throw ot just change ?? like flag=true; else ||flag==true
                }
            }
        }
    }
}
