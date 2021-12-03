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
            for(int i=0; i<DronesBL.Count(); i++)//#########IDK IF ITS SUPPOSED TO BE LIKE THAT, BUT YOU DIDNT COPY THE SADOT OF THE BL
            {
                if(id== DronesBL[i].Id)
                {
                    droneBO.Status = DronesBL[i].Status;
                    droneBO.Battery = DronesBL[i].Battery;
                }
            }
            //stationBO.DronesinCharge= from sic in AccessIdal.GetALLDrone(sic=> sic.Id==Id )
            //                          let 
            return droneBO;

        }
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

                throw new BadIdException("Drone"); //###############33
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
        public void AddDrone(Drone d, int stationId) ////*int id, string model, WeightCategories w, int stationId*/ -- input in cunsuleBL
        {
            if (!AccessIdal.CheckStation(stationId))
                throw new BadIdException("station"); //#################3
            IDAL.DO.Station s = AccessIdal.GetStation(stationId);
            IDAL.DO.Drone droneDO = new IDAL.DO.Drone()
            {
                Id = d.Id,
                Model = d.Model,
                MaxWeight = (IDAL.DO.WeightCategories)d.MaxWeight,
            };
            if (s.ChargeSlots <= 0)
                throw new StationProblemException(s.Id, "there is no empty charge slot");//############
            try
            {
                AccessIdal.AddDrone(droneDO);
            }
            catch (IDAL.DO.IDExistsException)
            {
                throw new IDExistsException("Drone"); ///##############
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

        }
        public IEnumerable<Drone> GetAllDrones()
        {
            return from droneDO in AccessIdal.GetALLDrone()
                   orderby droneDO.Id
                   select GetDrone(droneDO.Id);
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

            int i = DronesBL.FindIndex(x => x.Id == id);
            DronesBL.ElementAt(i).Model = m;
            //for (int i = 0; i < DronesBL.Count(); i++)
            //{
            //    if (id == DronesBL[i].Id)
            //        DronesBL[i].Model = m;
            //}
        }

        public void DroneToCharge(int id)
        {
            if (!AccessIdal.CheckDrone(id))
                throw new IDAL.DO.BadIdException("doesnt exist"); ///######
            IDAL.DO.Drone d = AccessIdal.GetDrone(id);
            DroneToList dt = DronesBL.Find(x => x.Id == id);
            if (dt.Status != DroneStatuses.Available)
                throw new WrongDroneStatException(id, "this drone is not available"); ///########
            Station s = closestStation(dt.CurrentLocation.Longitude, dt.CurrentLocation.Lattitude);
            Location l = new Location() { Lattitude = s.StationLocation.Lattitude, Longitude = s.StationLocation.Longitude };
            double b = amountOfbattery(GetDrone(id), dt.CurrentLocation, l);
            if (b > dt.Battery)
                throw new BatteryIssueException(dt.Id, "there wasnt enough battery"); //אין מספיק בטריה#####
            DronesBL.Remove(dt);
            dt.Battery -= b;
            dt.CurrentLocation = new Location() { Lattitude = s.StationLocation.Lattitude, Longitude = s.StationLocation.Longitude };
            dt.Status = DroneStatuses.Maintenance;

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
            Updatestation(ss.Id, "", ss.ChargeSlots);
        }
        public void EndCharging(int id, int timeI)//timeI-hours? if it is we need to do modulu 60 in consul (time/60+time%60)
        {
            if (!AccessIdal.CheckDrone(id))
                throw new IDAL.DO.BadIdException(id, "this drone doesnt exist"); //#######
            for (int i = 0; i < DronesBL.Count(); i++)
            {
                if (id == DronesBL[i].Id)
                {
                    if (DronesBL[i].Status != DroneStatuses.Maintenance)
                        throw new WrongDroneStatException(id, "this drone is not in charge"); //#####
                    DronesBL[i].Status=DroneStatuses.Available;
                    DronesBL[i].Battery += timeI * chargeRate;
                }
            }
            //d.Battery += timeI * chargeRate;
            //d.Status = DroneStatuses.Available;
            IDAL.DO.DroneCharge dc = AccessIdal.GetDroneCharge(id);
            Station s = GetStation(dc.StationId);
            s.ChargeSlots++;
            AccessIdal.EndingCharge(id);
            Updatestation(s.Id, "", s.ChargeSlots);
        }

        

        public void LinkDroneToParcel(int id)
        {
            if (!AccessIdal.CheckDrone(id))
                throw new BadIdException(id, "drone");
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

                if (item.Priority == p.Priority && item.Weight == p.Weight && dist2 < dist)
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
            //p2.DroneParcel = new DroneInParcel() { Id = d.Id, Battery = d.Battery, CurrentLocation = new Location() { Lattitude = d.CurrentLocation.Lattitude, Longitude = d.CurrentLocation.Longitude } };
            p2.Scheduled = DateTime.Now;
            AccessIdal.UpdateParcel(p2);  //maybe we should go straight to DAL.Update
            DroneToList dt = DronesBL.Find(x => x.Id == d.Id);
            DronesBL.Remove(dt);
            dt.Status = DroneStatuses.Delivery;
            dt.IdOfParcel = p2.Id;
            DronesBL.Add(dt);
        }

        public string ShowOneDrone(int _id)
        {
            Drone s = GetDrone(_id); //finding the station by its id
            return s.ToString();
        }

        public IEnumerable<DroneToList> ListDrone()
        {
            List<DroneToList> temp = new();
            foreach (DroneToList item in DronesBL)
            {
                temp.Add(item);
            }
            return temp;
        }
    }
}
