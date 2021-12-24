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
        /// <summary>
        /// adapter- gets a DO drone and returns a BO drone
        /// </summary>
        /// <param name="droneDO"></param> the DO drone
        /// <returns></returns> BO drone
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
            for (int i = 0; i < DronesBL.Count(); i++)//#########IDK IF ITS SUPPOSED TO BE LIKE THAT, BUT YOU DIDNT COPY THE SADOT OF THE BL
            {
                if (id == DronesBL[i].Id)
                {
                    droneBO.Status = DronesBL[i].Status;
                    droneBO.Battery = DronesBL[i].Battery;
                }
            }
            //stationBO.DronesinCharge= from sic in AccessIdal.GetALLDrone(sic=> sic.Id==Id )
            //                          let 
            return droneBO;

        }
        /// <summary>
        /// the func gets an id of drone and returns the drone
        /// </summary>
        /// <param name="id"></param> id of a drone
        /// <returns></returns>
        public Drone GetDrone(int id)
        {

            IDAL.DO.Drone d;
            if (id == 0)
                return new Drone();
            try
            {
                d = AccessIdal.GetDrone(id);
            }
            catch (IDAL.DO.BadIdException)
            {

                throw new BadIdException(id, "this Drone doesn't exists");
            }
            DroneToList dt = DronesBL.Find(x => x.Id == id);
            Drone db = new Drone()
            {
                Id = id,
                Model = d.Model,
                MaxWeight = (WeightCategories)d.MaxWeight,
                Status = dt.Status,
                Battery = dt.Battery,
                CurrentLocation = dt.CurrentLocation,
            };
            db.CurrentParcel = new ParcelInTransfer()
            {
                Id = dt.IdOfParcel,
                Weight = GetParcel(dt.IdOfParcel).Weight,
                Priority = GetParcel(dt.IdOfParcel).Priority,
            };
            if (dt.IdOfParcel != -1)
            {
                db.CurrentParcel.Sender = new CustomerInParcel() { Id = GetParcel(dt.IdOfParcel).Sender.Id, CustomerName = GetParcel(dt.IdOfParcel).Sender.CustomerName };
                db.CurrentParcel.Target = new CustomerInParcel() { Id = GetParcel(dt.IdOfParcel).Target.Id, CustomerName = GetParcel(dt.IdOfParcel).Target.CustomerName };
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
            }
            else
            {
                db.CurrentParcel.Sender = new CustomerInParcel();
                db.CurrentParcel.Target = new CustomerInParcel();
                db.CurrentParcel.PickingLocation = new Location();
                db.CurrentParcel.TargetLocation = new Location();
            }

            return db;
        }
        /// <summary>
        /// the func gets a drone and add it to the list
        /// </summary>
        /// <param name="d"></param> the drone we add
        /// <param name="stationId"></param> anid of station to charge the drone
        public void AddDrone(Drone d, int stationId)
        {

            // IDAL.DO.Station s = AccessIdal.GetStation(stationId);
            IDAL.DO.Drone droneDO = new IDAL.DO.Drone()
            {
                Id = d.Id,
                Model = d.Model,
                MaxWeight = (IDAL.DO.WeightCategories)d.MaxWeight,
            };
            //if (d.CurrentLocation != null && !AccessIdal.CheckStation(stationId))
            //    throw new BadIdException(stationId, "this station doesn't exist");
            //if (s.ChargeSlots <= 0)
            //    throw new StationProblemException(s.Id, "there is no empty charge slot");
            try
            {
                AccessIdal.AddDrone(droneDO);
            }
            catch (IDAL.DO.IDExistsException)
            {
                throw new IDExistsException(droneDO.Id, "this Drone already exists"); ///##############
            }

            DroneToList dt = new DroneToList()
            {
                Id = droneDO.Id,
                Battery = d.Battery,
                Status = d.Status,
                MaxWeight = d.MaxWeight,
                Model = droneDO.Model,
                IdOfParcel = -1,
                CurrentLocation = d.CurrentLocation,
                

        };
            //if (d.CurrentLocation == null)
            //    dt.CurrentLocation = new Location() { Lattitude = s.Lattitude, Longitude = s.Longitude };
            //else
            if (d.Status == DroneStatuses.Maintenance)
            {
                IDAL.DO.Station s = AccessIdal.GetStation(stationId);
                dt.CurrentLocation = new Location() { Lattitude = s.Lattitude, Longitude = s.Longitude };
                IDAL.DO.DroneCharge dic = new IDAL.DO.DroneCharge()
                {
                    DroneId = d.Id,
                    StationId = s.Id
                };
                AccessIdal.AddDroneCharge(dic);
                s.ChargeSlots--;

                Updatestation(s.Id, s.Name, s.ChargeSlots);
            }
            bool k=false;
            if (dt.Status == DroneStatuses.Delivery)
            {
                dt.Status = DroneStatuses.Available;
                k = true;
            }
            DronesBL.Add(dt);
            if (k)
                LinkDroneToParcel(dt.Id);
          

        }
        /// <summary>
        /// the func returns all of the drones
        /// </summary>
        /// <returns></returns>list of drones
        public IEnumerable<Drone> GetAllDrones()
        {
            return from droneDO in AccessIdal.GetALLDrone()
                   orderby droneDO.Id
                   select GetDrone(droneDO.Id);
        }
        /// <summary>
        /// the func gets a drone id and update it
        /// </summary>
        /// <param name="id"></param> the id of the drone
        /// <param name="m"></param> the model name - the new one to update
        public void UpdateDrone(int id, string m)
        {
            if (!AccessIdal.CheckDrone(id))
                throw new BadIdException(id,"this dronedont exist");
            try
            {
                IDAL.DO.Drone d = AccessIdal.GetDrone(id);
                d.Id = id;
                d.Model = m;
                AccessIdal.UpdateDrone(d);
            }
            catch (IDAL.DO.BadIdException)
            {

                throw new BadIdException(id,"this drone doesnt exists");
            }

            int i = DronesBL.FindIndex(x => x.Id == id);
            DronesBL.ElementAt(i).Model = m;
            
        }
        /// <summary>
        /// the func gets an id of drone and charge it in the clothest station
        /// </summary>
        /// <param name="id"></param> the id of the drone
        public void DroneToCharge(int id)
        {
            if (!AccessIdal.CheckDrone(id))
                throw new IDAL.DO.BadIdException(id, "this drone doesnt exist"); 
            IDAL.DO.Drone d = AccessIdal.GetDrone(id);
            DroneToList dt = DronesBL.Find(x => x.Id == id);
            if (dt.Status != DroneStatuses.Available)
                throw new WrongDroneStatException(id, "this drone is already charging or in a delivery -> not available"); 
            Station s = closestStation(dt.CurrentLocation.Longitude, dt.CurrentLocation.Lattitude);
            Location l = new Location() { Lattitude = s.StationLocation.Lattitude, Longitude = s.StationLocation.Longitude };
            double b = amountOfbattery(GetDrone(id), dt.CurrentLocation, l);
            if (b > dt.Battery)
                throw new BatteryIssueException(dt.Id, "there wasnt enough battery"); 
            DronesBL.Remove(dt);
            dt.Battery -= b;
            dt.CurrentLocation = new Location() { Lattitude = s.StationLocation.Lattitude, Longitude = s.StationLocation.Longitude };
            dt.Status = DroneStatuses.Maintenance;
            dt.TimeCharge = DateTime.Now;
            DronesBL.Add(dt);
            Station ss = GetStation(s.Id);
            DroneCharge dc = new DroneCharge() { Battery = dt.Battery, DroneId = dt.Id };
            ss.DronesinCharge.Add(dc);
            IDAL.DO.DroneCharge dic = new IDAL.DO.DroneCharge()
            {
                DroneId = dc.DroneId,
                StationId = s.Id
            };
            AccessIdal.AddDroneCharge(dic);
            ss.ChargeSlots--;
            
            Updatestation(ss.Id, ss.Name, ss.ChargeSlots);
        }

        /// <summary>
        /// the func gets a drone id and end its charging
        /// </summary>
        /// <param name="id"></param> the id of the drone
        /// <param name="timeI"></param> the amount of time it was charging (updating the battery)
        public void EndCharging(int id)
        {
            if (!AccessIdal.CheckDrone(id))
                throw new IDAL.DO.BadIdException(id, "this drone doesnt exist"); //#######
            DroneToList dt = DronesBL.Find(x => x.Id == id);
            DroneToList dss = DronesBL.Find(x => x.Id == id);
            if (dt.Status != DroneStatuses.Maintenance)
                throw new WrongDroneStatException(id, "this drone is not in charge"); //#####
           // DronesBL.Remove(dt);
            dt.Status = DroneStatuses.Available;
            TimeSpan timeSpan = DateTime.Now- dt.TimeCharge ;
            dt.Battery += timeSpan.TotalHours * chargeRate;
            if (dt.Battery >= 100)
                dt.Battery = 100;
            DronesBL.Remove(dss);

            DronesBL.Add(dt);
            IDAL.DO.DroneCharge dc = AccessIdal.GetDroneCharge(id);
            Station s = GetStation(dc.StationId);
            s.ChargeSlots++;
            AccessIdal.EndingCharge(id);
            Updatestation(s.Id, "", s.ChargeSlots);
        }

        /// <summary>
        /// the func gets an id of a drone and link a parcel to it
        /// </summary>
        /// <param name="id"></param> the id of the drone
        public void LinkDroneToParcel(int id)
        {
            if (!AccessIdal.CheckDrone(id))
                throw new BadIdException(id, "this drone doesn't exist");
            Drone d = GetDrone(id);
            if (d.Status != DroneStatuses.Available)
                throw new WrongDroneStatException(id, "this drone is not available");
            IEnumerable<IDAL.DO.Parcel> par = AccessIdal.GetAllUnMachedParcel();
            IDAL.DO.Parcel p = par.First(), p2 = par.First();
            bool flag=false ,flag2 = false;
            foreach (var item in par)
            {
                if (item.Priority > p.Priority)
                {
                    p = item;
                    flag2 = true;
                }
                else if (item.Priority == p.Priority && item.Weight > p.Weight && (WeightCategories)item.Weight <= d.MaxWeight)
                {
                    p = item;
                    flag2 = true;
                }
                double dist = getDistanceFromLatLonInKm(d.CurrentLocation.Lattitude, d.CurrentLocation.Longitude, GetCustomer(p.SenderId).CustLocation.Lattitude, GetCustomer(p.SenderId).CustLocation.Longitude);
                double dist2 = getDistanceFromLatLonInKm(d.CurrentLocation.Lattitude, d.CurrentLocation.Longitude, GetCustomer(item.SenderId).CustLocation.Lattitude, GetCustomer(item.SenderId).CustLocation.Longitude);

                if (item.Priority == p.Priority && item.Weight == p.Weight && dist2 <=dist)
                {
                    p = item;
                    flag2 = true;
                }

                double b = amountOfbattery(d, d.CurrentLocation, GetCustomer(p.SenderId).CustLocation);
                double b2 = amountOfbattery(d, GetCustomer(p.SenderId).CustLocation, GetCustomer(p.TargetId).CustLocation);
                Station st = closestStation(GetCustomer(p.TargetId).CustLocation.Longitude, GetCustomer(p.TargetId).CustLocation.Lattitude);
                double b3 = amountOfbattery(d, GetCustomer(p.TargetId).CustLocation, st.StationLocation);
                if (d.Battery >= b + b2 + b3 &&flag2)
                {
                    p2 = p;
                    flag = true;
                }
                flag2 = false;

            }
            if (!flag)
                throw new Exception("no match was found"); //no mach parcel for drone ##########
            p2.DroneId = d.Id;
            p2.Scheduled = DateTime.Now;
            AccessIdal.UpdateParcel(p2);  
            DroneToList dt = DronesBL.Find(x => x.Id == d.Id);
            DronesBL.Remove(dt);
            dt.Status = DroneStatuses.Delivery;
            dt.IdOfParcel = p2.Id;
            DronesBL.Add(dt);
        }

        /// <summary>
        /// the func retuens the list of drones in the constructor
        /// </summary>
        /// <returns></returns> list of drones
        public IEnumerable<DroneToList> ListDrone()
        {
            List<DroneToList> temp = new();
            foreach (DroneToList item in DronesBL)
            {
                temp.Add(item);
            }
            return temp;
        }

        public IEnumerable<Drone> GetDroneBy(Predicate<Drone> P)
        {
            return from d in GetAllDrones()
                   where P(d)
                   select d;
        }
    }
}
