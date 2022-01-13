using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using System.Xml.Linq;
using DO;

namespace Dal
{
    sealed partial class DLXML : IDal
    {
        public void AddCustomer(DO.Customer cus)
        {
            List<DO.Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<DO.Customer>(customerPath);
            Customer dd = ListCustomer.Find(s => s.Id == cus.Id);

            if (cus.Id == dd.Id)
                throw new DO.IDExistsException(cus.Id, "Duplicate customer ID");

            ListCustomer.Add(cus); //no need to Clone()

            XMLTools.SaveListToXMLSerializer(ListCustomer, customerPath);
        }

        public DO.Customer GetCustomer(int id)
        {
           if (!CheckCustomer(id))///
                throw new DO.BadIdException(id, "Customer id doesnt exist: ");

            List<DO.Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<DO.Customer>(customerPath);
            Customer dd = ListCustomer.Find(s => s.Id == id);
            return dd;

        }
        public bool CheckCustomer(int id)
        {
            List<DO.Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<DO.Customer>(customerPath);
            return ListCustomer.Any(x => x.Id == id);
        }

        public IEnumerable<DO.Customer> GetALLCustomer()
        {
            List<DO.Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<DO.Customer>(customerPath);
            return from Customer in ListCustomer
                   select Customer;
        }

        public double CustomerDistance(double lat, double lon1, int id)
        {
            List<DO.Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<DO.Customer>(customerPath);
            Customer d = ListCustomer.Find(x => x.Id == id);//finding the customer
            if (d.Id != id)
                throw new DO.BadIdException(id, "This id doesnt exists");
            return getDistanceFromLatLonInKm(lat, lon1, d.Lattitude, d.Longitude);//sending to the func to calculate
        }
        public IEnumerable<DO.Customer> ListCustomer()
        {
            List<DO.Customer> ListCustomer = XMLTools.LoadListFromXMLSerializer<DO.Customer>(customerPath);
            return ListCustomer.ToList();
        }
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




        public void AddParcel(DO.Parcel per)
        {
            List<DO.Parcel> ParcelList = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);
            Parcel dd = ParcelList.Find(s => s.Id == per.Id);

            if (per.Id == dd.Id)
                throw new DO.IDExistsException(per.Id, "Duplicate Parcel ID");

            ParcelList.Add(per); //no need to Clone()
            XElement parcelidroot = XMLTools.LoadListFromXMLElement(configPath);
            parcelidroot.Element("RowNumbers").Element("NewParcelId").Value = per.Id.ToString();
            //  XMLTools.LoadListFromXMLElement(configPath).Element("RowNumbers").Element("NewParcelId").SetValue(++per.Id);
            XMLTools.SaveListToXMLElement(parcelidroot, configPath);
            XMLTools.SaveListToXMLSerializer(ParcelList, parcelPath);
        }

        public DO.Parcel GetParcel(int id)
        {
            if (!CheckParcel(id))
                throw new DO.BadIdException(id, "Parcel id doesnt exist: ");

            List<DO.Parcel> ParcelList = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);
            Parcel dd = ParcelList.Find(s => s.Id == id);
            return dd;

        }
        public bool CheckParcel(int id)
        {
            List<DO.Parcel> ParcelList = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);
            return ParcelList.Any(x => x.Id == id);
        }

        public IEnumerable<DO.Parcel> GetALLParcel()
        {
            List<DO.Parcel> ParcelList = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);
            return from Parcel in ParcelList
                   select Parcel;
        }


        public IEnumerable<DO.Parcel> ListParcel()
        {
            List<DO.Parcel> ParcelList = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);
            return ParcelList.ToList();
        }
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

        public IEnumerable<DO.Parcel> GetALLParcelsBy(Predicate<DO.Parcel> P)
        {
            List<DO.Parcel> ParcelList = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);
            return from d in ParcelList
                   where P(d)
                   select d;
        }
        public void RemoveParcel(int id)
        {
            if (!CheckParcel(id))
                throw new BadIdException(id, "this parcel doesnt exist");
            List<DO.Parcel> ParcelList = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(parcelPath);
            ParcelList.Remove(GetParcel(id));
            XMLTools.SaveListToXMLSerializer(ParcelList, parcelPath);

        }
        public int getParcelMax()
        {
            int id= Convert.ToInt32(XMLTools.LoadListFromXMLElement(configPath).Element("RowNumbers").Value);
            return id;
        }
        public IEnumerable<DO.Parcel> GetAllUnMachedParcel()
        {
            return from item in GetALLParcel()
                   where item.DroneId == 0
                   select GetParcel(item.Id);
        }

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
