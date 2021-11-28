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
        private Station stationDoBoAdapter(IDAL.DO.Station stationDO)
        {
            Station stationBO = new Station();
            int id = stationDO.Id;
            IDAL.DO.Station s;
            try //???
            {
                 s = AccessIdal.GetStation(id);
            }
            catch (IDAL.DO.BadIdException)
            {

                throw new BadIdException("station");
            }
            s.CopyPropertiesTo(stationBO);
            stationDO.CopyPropertiesTo(stationBO);
            //stationBO.DronesinCharge= from sic in AccessIdal.GetALLDrone(sic=> sic.Id==Id )
            //                          let 
            stationBO.DronesinCharge = new List<DroneCharge>();
            
            return stationBO;
       
        }
        public void AddStation( Station s/*int id, string name,Location l*/ )
        {
            if (!AccessIdal.CheckStation(s.Id))
                throw new BadIdException("station");
            IDAL.DO.Station newS = new IDAL.DO.Station()
            { 
                Id=s.Id,
                Name=s.Name,
                Lattitude=s.StationLocation.Lattitude,
                Longitude=s.StationLocation.Longitude,
                ChargeSlots=0,
                //רשימה מאיפה? זה דיאייאל לא ביאל
            };
            try
            {
                AccessIdal.AddStation(newS);
            }
            catch (IDAL.DO.IDExistsException)
            {

                throw new IDExistsException("Station");
            }
        }

        public void Updatestation(int id, string name, int numOfChargingSlots)
        {
            try
            {
                if (!AccessIdal.CheckStation(id))
                    throw new IDAL.DO.BadIdException("doesmt exist");
                IDAL.DO.Station s = AccessIdal.GetStation(id);
                if (name != "")
                    s.Name = name;
                //Station t=get
                int n = AccessIdal.NumOfChargingNow(id);
                if (numOfChargingSlots >=n)
                    s.ChargeSlots = numOfChargingSlots - n;
                AccessIdal.UpdateStation(s);
            }
            catch (IDAL.DO.BadIdException)
            {

                throw new BadIdException("station");
            }
        }

        public Station GetStation(int id)
        {
            IDAL.DO.Station s;
            try
            {
               s = AccessIdal.GetStation(id);
            }
            catch (IDAL.DO.BadIdException)
            {

                throw new BadIdException("station");
            }
            Station sb = new Station()
            {
                Id = id,
                StationLocation = new Location() { Lattitude = s.Lattitude, Longitude = s.Longitude },
                Name = s.Name,
                ChargeSlots = s.ChargeSlots

            };

            foreach (Drone item in GetAllDrones())
            {
                if(item.CurrentLocation.Longitude==sb.StationLocation.Longitude)
                    if(item.CurrentLocation.Lattitude == sb.StationLocation.Lattitude)
                    {
                        DroneCharge dc = new DroneCharge() { Battery = item.Battery, DroneId = item.Id };
                        sb.DronesinCharge.Add(dc);
                    }
            }

            return sb;
        }

        public IEnumerable<Station> GetAllStations()
        {
            return from item in AccessIdal.GetALLStation()
                   orderby item.Id
                   select GetStation(item.Id);
        }
    }
}
