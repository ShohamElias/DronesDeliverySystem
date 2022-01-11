using System;
using DalApi;
using System.Xml.Linq;
using DO;
using System.Collections.Generic;
using System.Linq;

namespace Dal
{
    sealed partial class DLXML : IDal
    {
        #region singelton
        static readonly DLXML instance = new DLXML();
        static DLXML() { }
        DLXML() { }
        public static DLXML Instance { get => instance; }
        #endregion

        #region DS DLXML Files
        string customerPath = @"customerXML.xml";
        string dronechargePath = @"drone-chargeXML.xml";
        string dronePath = @"droneXML.xml";
        string parcelPath = @"parcelXML.xml";
        string stationPath = @"stationXML.xml";
        #endregion


        #region Drone 
        public void AddDrone(DO.Drone d)
        {
            List<DO.Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dronePath);
            Drone dd = ListDrone.Find(s => s.Id == d.Id);

            if (d.Id == dd.Id)
                throw new DO.IDExistsException(d.Id, "Duplicate student ID");

            ListDrone.Add(d); //no need to Clone()

            XMLTools.SaveListToXMLSerializer(ListDrone, dronePath);
        }

        public DO.Drone GetDrone(int id)
        {
            if (!CheckDrone(id))
                throw new DO.BadIdException(id, "Drone id doesnt exist: ");

            List<DO.Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dronePath);
            Drone dd = ListDrone.Find(s => s.Id == id);
            return dd;

        }
        public bool CheckDrone(int id)
        {
            List<DO.Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dronePath);
            return ListDrone.Any(x => x.Id == id);
        }

        public IEnumerable<DO.Drone> GetALLDrone()
        {
            List<DO.Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dronePath);
            return from Drone in ListDrone
                   select Drone;
        }
        public void UpdateDrone(DO.Drone newD)
        {
            List<DO.Drone> DroneList = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dronePath);

            Drone d2 = DroneList.Find(x => x.Id == newD.Id); //finding the station by its id
            if (d2.Id != newD.Id)
                throw new DO.BadIdException(newD.Id, "This drone does not exist");
            DroneList.Remove(d2);
            DroneList.Add(newD);

            XMLTools.SaveListToXMLSerializer(DroneList, dronePath);
        }
        void LinkParcelToDrone(int parcelId, int droneId)
        {
            List<DO.Drone> DroneList = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dronePath);
            List<DO.Parcel> ParcelsList = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);

            Parcel p =ParcelsList.Find(x => x.Id == parcelId); //finding the parcel by its id
            if (p.Id != parcelId)
                throw new DO.BadIdException(parcelId, "This parcel id doesnt exists");
            Drone d = DroneList.Find(x => x.Id == droneId);//finding the drone
            if (d.Id != droneId)
                throw new DO.BadIdException(droneId, "This drone id doesnt exists");
            ParcelsList.Remove(p);
            p.DroneId = droneId; //adding the drone id to the parcel
            p.Scheduled = DateTime.Now;
            ParcelsList.Add(p);

            XMLTools.SaveListToXMLSerializer(ParcelsList, parcelPath);

        }
        void DroneToCharge(int droneId, int stationId)
        {
            List<DO.Drone> DroneList = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dronePath);
            List<DO.DroneCharge> DChargeList = XMLTools.LoadListFromXMLSerializer<DO.DroneCharge>(dronechargePath);
            List<DO.Station> StationsList = XMLTools.LoadListFromXMLSerializer<DO.Station>(stationPath);

            Drone d = DroneList.Find(x => x.Id == droneId);//finding the drone
            if (d.Id != droneId)
                throw new DO.BadIdException(droneId, "This drone id doesnt exists");
            Station s = StationsList.Find(x => x.Id == stationId);//finding the station
            if (s.Id != stationId)
                throw new DO.BadIdException(stationId, "This station id doesnt exists");
            StationsList.Remove(s);
            s.ChargeSlots--;
            StationsList.Add(s);
            DroneCharge dc = new DroneCharge()//creating a dronecharge object
            {
                DroneId = droneId,
                StationId = stationId
            };
            DChargeList.Add(dc);//adding it to the list

            XMLTools.SaveListToXMLSerializer(StationsList, stationPath);
            XMLTools.SaveListToXMLSerializer(DChargeList, dronechargePath);
        }

        void EndingCharge(int droneId)
        {
            List<DO.Drone> DroneList = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dronePath);
            List<DO.DroneCharge> DChargeList = XMLTools.LoadListFromXMLSerializer<DO.DroneCharge>(dronechargePath);
            List<DO.Station> StationsList = XMLTools.LoadListFromXMLSerializer<DO.Station>(stationPath);

            Drone d = DroneList.Find(x => x.Id == droneId);//finding the drone
            if (d.Id != droneId)
                throw new DO.BadIdException(droneId, "This drone id doesnt exists");
            DroneCharge dc = DChargeList.Find(x => x.DroneId == droneId);//finding the dronecharge object
            Station sta1 =StationsList.Find(x => x.Id == dc.StationId);//finsding the station he was charged at
            sta1.ChargeSlots++;//adding an empty charge slot
            DChargeList.Remove(dc);//removing from the list

            XMLTools.SaveListToXMLSerializer(StationsList, stationPath);
            XMLTools.SaveListToXMLSerializer(DChargeList, dronechargePath);
        }

        public int GetChargeRate()
        {
            return  chargeRate;
        }


        #endregion
    }
}
