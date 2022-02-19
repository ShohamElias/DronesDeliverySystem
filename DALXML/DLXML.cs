using System;
using DalApi;
using System.Xml.Linq;
using DO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;


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
        string configPath = @"data-config.xml";
        #endregion


        #region Drone 
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(DO.Drone d)
        {
            List<DO.Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dronePath);
            Drone dd = ListDrone.Find(s => s.Id == d.Id);

            if (d.Id == dd.Id)
                throw new DO.IDExistsException(d.Id, "Duplicate student ID");

            ListDrone.Add(d); //no need to Clone()

            XMLTools.SaveListToXMLSerializer(ListDrone, dronePath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public DO.Drone GetDrone(int id)
        {
            if (!CheckDrone(id))
                throw new DO.BadIdException(id, "Drone id doesnt exist: ");

            List<DO.Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dronePath);
            Drone dd = ListDrone.Find(s => s.Id == id);
            return dd;

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool CheckDrone(int id)
        {
            List<DO.Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dronePath);
            return ListDrone.Any(x => x.Id == id);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DO.Drone> ListDrone()
        {
            List<DO.Drone> Listdrone = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dronePath);
            return Listdrone.ToList();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DO.Drone> GetALLDrone()
        {
            List<DO.Drone> ListDrone = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dronePath);
            return from Drone in ListDrone
                   select Drone;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void LinkParcelToDrone(int parcelId, int droneId)
        {
            List<DO.Drone> DroneList = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dronePath);
            List<DO.Parcel> ParcelsList = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);

            Parcel p = ParcelsList.Find(x => x.Id == parcelId); //finding the parcel by its id
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DroneToCharge(int droneId, int stationId)
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void EndingCharge(int droneId)
        {
            List<DO.Drone> DroneList = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dronePath);
            List<DO.DroneCharge> DChargeList = XMLTools.LoadListFromXMLSerializer<DO.DroneCharge>(dronechargePath);
            List<DO.Station> StationsList = XMLTools.LoadListFromXMLSerializer<DO.Station>(stationPath);

            Drone d = DroneList.Find(x => x.Id == droneId);//finding the drone
            if (d.Id != droneId)
                throw new DO.BadIdException(droneId, "This drone id doesnt exists");
            DroneCharge dc = DChargeList.Find(x => x.DroneId == droneId);//finding the dronecharge object
            Station sta1 = StationsList.Find(x => x.Id == dc.StationId);//finsding the station he was charged at
            sta1.ChargeSlots++;//adding an empty charge slot
            DChargeList.Remove(dc);//removing from the list

            XMLTools.SaveListToXMLSerializer(StationsList, stationPath);
            XMLTools.SaveListToXMLSerializer(DChargeList, dronechargePath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public double GetChargeRate()
        {
            double[] i = ElectricityUse();
            return i[4];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] ElectricityUse()
        {
            return XMLTools.LoadListFromXMLElement(configPath).Element("BatteryUsages").Elements()
                .Select(e => Convert.ToDouble(e.Value)).ToArray();
        }

        #endregion

        #region Drone Charge

        [MethodImpl(MethodImplOptions.Synchronized)]
        public DO.DroneCharge GetDroneCharge(int id)
        {
            XElement dronechargeElement = XMLTools.LoadListFromXMLElement(dronechargePath);
            DroneCharge dc = (from dce in dronechargeElement.Elements()
                              where int.Parse(dce.Element("DroneId").Value) == id
                              select new DroneCharge()
                              {
                                  DroneId = Int32.Parse(dce.Element("DroneId").Value),
                                  StationId = Int32.Parse(dce.Element("StationId").Value)
                              }).FirstOrDefault();
            if (dc.DroneId != id)
                throw new DO.BadIdException(id, "This drone does not exist in drone charges list: ");
            return dc;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDroneCharge(DO.DroneCharge dc)
        {
            XElement droneChargeElem = XMLTools.LoadListFromXMLElement(dronechargePath);
            XElement d = (from p in droneChargeElem.Elements()
                          where int.Parse(p.Element("DroneId").Value) == dc.DroneId
                          select p).FirstOrDefault();
            if (d != null)
                throw new DO.IDExistsException(dc.DroneId, "This drone id already in charge");
            XElement dronechar = new XElement("DroneCharge", new XElement("DroneId", dc.DroneId),
                                               new XElement("StationId", dc.StationId));
            droneChargeElem.Add(dronechar);

            XMLTools.SaveListToXMLElement(droneChargeElem, dronechargePath);

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool CheckDC(int id)
        {
            XElement dronechargeElement = XMLTools.LoadListFromXMLElement(dronechargePath);
            XElement d = (from p in dronechargeElement.Elements()
                          where int.Parse(p.Element("DroneId").Value) == id
                          select p).FirstOrDefault();
            return d != null;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneCharge(DO.DroneCharge dc)
        {
            XElement droneChargeElem = XMLTools.LoadListFromXMLElement(dronePath);
            XElement d = (from p in droneChargeElem.Elements()
                          where int.Parse(p.Element("DroneId").Value) == dc.DroneId
                          select p).FirstOrDefault();
            if (d == null)
                throw new DO.BadIdException(dc.DroneId, "This drone does not exist in drone charges list");
            d.Element("DroneId").Value = dc.DroneId.ToString();
            d.Element("StationId").Value = dc.StationId.ToString();

            XMLTools.SaveListToXMLElement(droneChargeElem, dronechargePath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDroneCharge(int id)
        {
            XElement droneChargeElem = XMLTools.LoadListFromXMLElement(dronePath);
            XElement d = (from p in droneChargeElem.Elements()
                          where int.Parse(p.Element("DroneId").Value) == id
                          select p).FirstOrDefault();
            if (d == null)
                throw new DO.BadIdException(id, "This drone does not exist in drone charges list");

            d.Remove();
            XMLTools.SaveListToXMLElement(droneChargeElem, dronechargePath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DO.DroneCharge> GetALLDroneCharges()
        {
            XElement droneChargeElem = XMLTools.LoadListFromXMLElement(dronechargePath);
            return (from p in droneChargeElem.Elements()
                    select new DroneCharge()
                    {
                        DroneId = Int32.Parse(p.Element("DroneId").Value),
                        StationId = Int32.Parse(p.Element("StationId").Value)

                    });
        }
        #endregion

        #region Station

        [MethodImpl(MethodImplOptions.Synchronized)]
        public DO.Station GetStation(int id)
        {
            XElement StationElement = XMLTools.LoadListFromXMLElement(stationPath);
            Station dc = (from dce in StationElement.Elements()
                          where int.Parse(dce.Element("Id").Value) == id
                          select new Station()
                          {
                              Id = int.Parse(dce.Element("Id").Value),
                              Name = (dce.Element("Name").Value).ToString(),
                              Longitude = double.Parse(dce.Element("Longitude").Value),
                              Lattitude = double.Parse(dce.Element("Lattitude").Value),
                              ChargeSlots = int.Parse(dce.Element("ChargeSlots").Value)
                          }).FirstOrDefault();
            if (dc.Id != id)
                throw new DO.BadIdException(id, "This station does not exist");
            return dc;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(DO.Station s)
        {
            XElement stationElem = XMLTools.LoadListFromXMLElement(stationPath);
            XElement d = (from p in stationElem.Elements()
                          where int.Parse(p.Element("Id").Value) == s.Id
                          select p).FirstOrDefault();
            if (d != null)
                throw new DO.IDExistsException(s.Id, "This station id already exist");
            XElement stat = new XElement("DroneCharge", new XElement("Id", s.Id), new XElement("Name", s.Name), new XElement("Longitude", s.Longitude),
            new XElement("Lattitude", s.Lattitude), new XElement("ChargeSlots", s.ChargeSlots));
            stationElem.Add(stat);

            XMLTools.SaveListToXMLElement(stationElem, stationPath);

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool CheckStation(int id)
        {
            XElement stationElement = XMLTools.LoadListFromXMLElement(stationPath);
            XElement d = (from p in stationElement.Elements()
                          where int.Parse(p.Element("Id").Value) == id
                          select p).FirstOrDefault();
            return d != null;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStation(DO.Station newD)
        {
            XElement stationElement = XMLTools.LoadListFromXMLElement(stationPath);
            XElement d = (from p in stationElement.Elements()
                          where int.Parse(p.Element("Id").Value) == newD.Id
                          select p).FirstOrDefault();
            if (d == null)
                throw new DO.BadIdException(newD.Id, "This station does not exist");
            d.Element("Id").Value = newD.Id.ToString();
            d.Element("Name").Value = newD.Name.ToString();
            d.Element("Longitude").Value = newD.Longitude.ToString();
            d.Element("Lattitude").Value = newD.Lattitude.ToString();
            d.Element("ChargeSlots").Value = newD.ChargeSlots.ToString();

            XMLTools.SaveListToXMLElement(stationElement, stationPath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DO.Station> GetALLStation()
        {
            XElement stationElement = XMLTools.LoadListFromXMLElement(stationPath);
            return (from p in stationElement.Elements()
                    select new Station()
                    {
                        Id = Int32.Parse(p.Element("Id").Value),
                        Name = (p.Element("Name").Value).ToString(),
                        Longitude = double.Parse(p.Element("Longitude").Value),
                        Lattitude = double.Parse(p.Element("Lattitude").Value),
                        ChargeSlots = int.Parse(p.Element("ChargeSlots").Value)
                    });
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DO.Station> GetALLStationsBy(Predicate<DO.Station> P)
        {
            XElement stationElement = XMLTools.LoadListFromXMLElement(stationPath);
            return (from p in stationElement.Elements()
                    where P(GetStation(Int32.Parse(p.Element("Id").Value)))
                    select new Station()
                    {
                        Id = Int32.Parse(p.Element("Id").Value),
                        Name = (p.Element("Name").Value).ToString(),
                        Longitude = double.Parse(p.Element("Longitude").Value),
                        Lattitude = double.Parse(p.Element("Lattitude").Value),
                        ChargeSlots = int.Parse(p.Element("ChargeSlots").Value)
                    });
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DO.Station> ListStation()
        {
            XElement stationElement = XMLTools.LoadListFromXMLElement(stationPath);
            List<DO.Station> statList = GetALLStation().ToList();
            return statList;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int NumOfChargingNow(int id)
        {
            List<DO.DroneCharge> DChargeList = GetALLDroneCharges().ToList();
            int num = 0;
            foreach (DroneCharge item in DChargeList)
            {
                if (item.StationId == id)
                    num++;
            }
            return num;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public double StationDistance(double lat, double lon1, int id)
        {
            List<DO.Station> statList = GetALLStation().ToList();
            Station d = statList.Find(x => x.Id == id);//finding the customer
            if (d.Id != id)
                throw new DO.BadIdException(id, "This station id doesnt exists");
            return getDistanceFromLatLonInKm(lat, lon1, d.Lattitude, d.Longitude);//sending to the func to calculate
        }

        #endregion

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(DO.Customer cus)
        {
            List<DO.Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<DO.Customer>(customerPath);
            Customer dd = ListCustomer.Find(s => s.Id == cus.Id);

            if (cus.Id == dd.Id)
                throw new DO.IDExistsException(cus.Id, "Duplicate customer ID");

            ListCustomer.Add(cus); //no need to Clone()

            XMLTools.SaveListToXMLSerializer(ListCustomer, customerPath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public DO.Customer GetCustomer(int id)
        {
            if (!CheckCustomer(id))///
                throw new DO.BadIdException(id, "Customer id doesnt exist: ");

            List<DO.Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<DO.Customer>(customerPath);
            Customer dd = ListCustomer.Find(s => s.Id == id);
            return dd;

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool CheckCustomer(int id)
        {
            List<DO.Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<DO.Customer>(customerPath);
            return ListCustomer.Any(x => x.Id == id);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DO.Customer> GetALLCustomer()
        {
            List<DO.Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<DO.Customer>(customerPath);
            return from Customer in ListCustomer
                   select Customer;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public double CustomerDistance(double lat, double lon1, int id)
        {
            List<DO.Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<DO.Customer>(customerPath);
            Customer d = ListCustomer.Find(x => x.Id == id);//finding the customer
            if (d.Id != id)
                throw new DO.BadIdException(id, "This id doesnt exists");
            return getDistanceFromLatLonInKm(lat, lon1, d.Lattitude, d.Longitude);//sending to the func to calculate
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DO.Customer> ListCustomer()
        {
            List<DO.Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<DO.Customer>(customerPath);
            return ListCustomer.ToList();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(DO.Customer newD)
        {
            List<DO.Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<DO.Customer>(customerPath);
            Customer d2 = ListCustomer.Find(x => x.Id == newD.Id); //finding the station by its id
            if (d2.Id != newD.Id)
                throw new DO.BadIdException(newD.Id, "This Customer does not exist");
            ListCustomer.Remove(d2);
            ListCustomer.Add(newD);
            XMLTools.SaveListToXMLSerializer(ListCustomer, customerPath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public double getDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371; // Radius of the earth in km
            double dLat = Deg2rad(lat2 - lat1);  // deg2rad below
            double dLon = Deg2rad(lon2 - lon1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(Deg2rad(lat1)) * Math.Cos(Deg2rad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2);///calculating by the formula
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c; // Distance in km
            return d;
        }

        /// <summary>
        /// the function gets a number in dergrees and converts it to radians
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>
        public double Deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddParcel(DO.Parcel per)
        {
            List<DO.Parcel> ParcelList = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);
            Parcel dd = ParcelList.Find(s => s.Id == per.Id);

            if (per.Id == dd.Id)
                throw new DO.IDExistsException(per.Id, "Duplicate Parcel ID");

            ParcelList.Add(per); //no need to Clone()
            XElement parcelidroot = XMLTools.LoadListFromXMLElement(configPath);
            parcelidroot.Element("RowNumbers").Element("NewParcelId").Value = per.Id.ToString();
            XMLTools.SaveListToXMLElement(parcelidroot, configPath);
            XMLTools.SaveListToXMLSerializer(ParcelList, parcelPath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public DO.Parcel GetParcel(int id)
        {
            if (!CheckParcel(id))
                throw new DO.BadIdException(id, "Parcel id doesnt exist: ");

            List<DO.Parcel> ParcelList = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);
            Parcel dd = ParcelList.Find(s => s.Id == id);
            return dd;

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool CheckParcel(int id)
        {
            List<DO.Parcel> ParcelList = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);
            return ParcelList.Any(x => x.Id == id);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DO.Parcel> GetALLParcel()
        {
            List<DO.Parcel> ParcelList = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);
            return from Parcel in ParcelList
                   select Parcel;
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DO.Parcel> ListParcel()
        {
            List<DO.Parcel> ParcelList = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);
            return ParcelList.ToList();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcel(DO.Parcel newD)
        {
            List<DO.Parcel> ParcelList = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);
            Parcel d2 = ParcelList.Find(x => x.Id == newD.Id); //finding the Parcel by its id
            if (d2.Id != newD.Id)
                throw new DO.BadIdException(newD.Id, "This Parcel does not exist");
            ParcelList.Remove(d2);
            ParcelList.Add(newD);
            XMLTools.SaveListToXMLSerializer(ParcelList, parcelPath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DO.Parcel> GetALLParcelsBy(Predicate<DO.Parcel> P)
        {
            List<DO.Parcel> ParcelList = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);
            return from d in ParcelList
                   where P(d)
                   select d;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveParcel(int id)
        {
            if (!CheckParcel(id))
                throw new BadIdException(id, "this parcel doesnt exist");
            List<DO.Parcel> ParcelList = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);
            ParcelList.Remove(GetParcel(id));
            XMLTools.SaveListToXMLSerializer(ParcelList, parcelPath);

        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int getParcelMax()
        {
            int id = Convert.ToInt32(XMLTools.LoadListFromXMLElement(configPath).Element("RowNumbers").Value);
            return id;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DO.Parcel> GetAllUnMachedParcel()
        {
            return from item in GetALLParcel()
                   where item.DroneId == 0
                   select GetParcel(item.Id);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PickParcel(int parcelId)
        {
            List<DO.Parcel> ParcelList = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);
            Parcel p = ParcelList.Find(x => x.Id == parcelId); //finding the parcel by its id
            if (p.Id != parcelId)
                throw new BadIdException(parcelId, "This parcel id doesnt exists");
            ParcelList.Remove(p);
            p.PickedUp = DateTime.Now;
            ParcelList.Add(p);
            XMLTools.SaveListToXMLSerializer(ParcelList, parcelPath);

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeliveringParcel(int parcelId)
        {
            List<DO.Parcel> ParcelList = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);
            Parcel p = ParcelList.Find(x => x.Id == parcelId); //finding the parcel by its id
            if (p.Id != parcelId)
                throw new DO.BadIdException(parcelId, "This parcel id doesnt exists");
            ParcelList.Remove(p);
            p.Delivered = DateTime.Now;
            ParcelList.Add(p);
            XMLTools.SaveListToXMLSerializer(ParcelList, parcelPath);
        }
    
    }
}
