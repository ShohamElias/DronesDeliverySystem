﻿using System;
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
                    Sender = GetParcel(dt.IdOfParcel).Sender,////deep????
                    Target = GetParcel(dt.IdOfParcel).Target,
                    Weight = GetParcel(dt.IdOfParcel).Weight,
                    Priority = GetParcel(dt.IdOfParcel).Priority,


                }

            };
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
                MaxWeight = (IDAL.DO.WeightCategories)w,                
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
                //update
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
            if (dt.Status != DroneStatuses.Available)
                throw;
            //station and battery

            IDAL.DO.Station s = dis(dt.CurrentLocation.Longitude, dt.CurrentLocation.Lattitude);

         //   if(d.Status==)
        }

    }
}
