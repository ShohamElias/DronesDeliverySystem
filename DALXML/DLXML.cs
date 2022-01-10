using System;
using DalApi;
using System.Xml.Linq;
using DO;
using System.Collections.Generic;
using System.Linq;

namespace Dal
{
    sealed class DLXML:IDal
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

            if (d.Id==dd.Id)
                throw new DO.IDExistsException(d.Id, "Duplicate student ID");
 
            ListDrone.Add(d); //no need to Clone()

            XMLTools.SaveListToXMLSerializer(ListDrone, dronePath);
        }

        public DO.Drone GetDrone(int id)
        {
            if (!CheckDrone(id))
                throw new DO.BadIdException(id, "Drone id doesnt exist: ");

            List<DO.Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dronePath);
            Drone dd = ListDrone.Find(s => s.Id ==id);
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
        #endregion
    }
}
