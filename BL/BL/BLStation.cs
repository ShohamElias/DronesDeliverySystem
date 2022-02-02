using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BlApi;
using System.Runtime.CompilerServices;


namespace BL
{
     partial class BL
    {
        /// <summary>
        /// the func is an adpater from dal object to a bl object of station
        /// </summary>
        /// <param name="stationDO"></param> DAL station
        /// <returns></returns> bl station
        private Station stationDoBoAdapter(DO.Station stationDO)
        {
            Station stationBO = new Station();
            int id = stationDO.Id;
            DO.Station s;
            try 
            {
                s = AccessIdal.GetStation(id);
            }
            catch (DO.BadIdException)
            {

                throw new BadIdException("station");
            }
            s.CopyPropertiesTo(stationBO);
            stationDO.CopyPropertiesTo(stationBO);
            stationBO.DronesinCharge = new List<DroneCharge>();
            DroneCharge dc = new DroneCharge();
            stationBO.DronesinCharge.Add(dc);
            return stationBO;
        }

        /// <summary>
        /// the func gets a station and adds it to the db on dal
        /// </summary>
        /// <param name="s"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(Station s)
        {
            lock (AccessIdal)
            {
                if (AccessIdal.CheckStation(s.Id))
                    throw new DO.IDExistsException(s.Id, "this station already exists");
                DO.Station newS = new DO.Station()
                {
                    Id = s.Id,
                    Name = s.Name,
                    Lattitude = s.StationLocation.Lattitude,
                    Longitude = s.StationLocation.Longitude,
                    ChargeSlots = s.ChargeSlots,
                };
                try
                {
                    AccessIdal.AddStation(newS);
                }
                catch (DO.IDExistsException)
                {

                    throw new IDExistsException("Station");
                }
            }
        }

        /// <summary>
        /// the func get an id and info of a ststion and updates it
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="numOfChargingSlots"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Updatestation(int id, string name, int numOfChargingSlots)
        {
            lock (AccessIdal)
            {
                try
                {
                    if (!AccessIdal.CheckStation(id))
                        throw new DO.BadIdException(id, "this station doesnt exist");
                    DO.Station s = AccessIdal.GetStation(id);
                    if (name != "")
                        s.Name = name;
                    //Station t=get
                    //int n = AccessIdal.NumOfChargingNow(id);
                    //if (numOfChargingSlots >=n)###############doing problems!
                    s.ChargeSlots = numOfChargingSlots/* - n*/;
                    AccessIdal.UpdateStation(s);
                }
                catch (DO.BadIdException)
                {

                    throw new BadIdException(id, "station");
                }
            }
        }

        /// <summary>
        /// the func get a station id and returns the station the id belongs to
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int id)
        {
            lock (AccessIdal)
            {
                DO.Station s;
                try
                {
                    s = AccessIdal.GetStation(id);
                }
                catch (DO.BadIdException)
                {

                    throw new BadIdException(id, "this station doesnt exists");
                }
                Station sb = new Station()
                {
                    Id = id,
                    StationLocation = new Location() { Lattitude = s.Lattitude, Longitude = s.Longitude },
                    Name = s.Name,
                    ChargeSlots = s.ChargeSlots,
                    DronesinCharge = new List<DroneCharge>()

                };

                foreach (DO.DroneCharge item in GetAllDroneCharges())
                {
                    if (AccessIdal.GetDroneCharge(item.DroneId).StationId == sb.Id)
                    {
                        DroneCharge dc = new DroneCharge() { DroneId = item.DroneId };
                        dc.Battery = GetDrone(item.DroneId).Battery;
                        sb.DronesinCharge.Add(dc);
                    }
                }
                return sb;
            }
        }

        /// <summary>
        ///  the functions returs all the stations
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetStationsforNoEmpty()
        {
            lock (AccessIdal)
            {
                return from sic in AccessIdal.GetALLStationsBy(sic => sic.ChargeSlots != 0)
                       let crs = AccessIdal.GetStation(sic.Id)
                       select GetStation(crs.Id);
            }
        }

        /// <summary>
        /// the func returns all of the stations
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetAllStations()
        {
            lock (AccessIdal)
            {
                return from item in AccessIdal.GetALLStation()
                       orderby item.Id
                       select GetStation(item.Id);
            }
        }
    }
}