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
        public void AddStation(int id, string name,Location l )
        {
            IDAL.DO.Station newS = new IDAL.DO.Station()
            { 
                Id=id,
                Name=name,
                Lattitude=l.Lattitude,
                Longitude=l.Longitude,
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

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
