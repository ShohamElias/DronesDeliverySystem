using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;
using System.Runtime.CompilerServices;

namespace Dal
{
    internal static class DataSource
    {
        internal class Config
        {
            public static int countIdParcel = 1000;
            internal static double empty = 1;
            internal static double light = 2;
            internal static double heavy = 5;
            internal static double medium = 3;
            internal static double chargeRate = 360;
           
        }


        internal static Random rand = new Random(DateTime.Now.Millisecond);//random
        //lists for the objects
        internal static List<Drone> DroneList = new List<Drone>();
        internal static List<Station> StationsList = new List<Station>();
        internal static List<Customer> CustomersList = new List<Customer>();
        internal static List<Parcel> ParcelsList = new List<Parcel>();
        internal static List<DroneCharge> DChargeList = new List<DroneCharge>();

         private static string customerPath = @"customerXML.xml";
        static  string dronechargePath = @"drone-chargeXML.xml";
        static string dronePath = @"droneXML.xml";
        static string parcelPath = @"parcelXML.xml";
        static string stationPath = @"stationXML.xml";
        static DataSource()
        {
            Intialize();
            //XMLTools.SaveListToXMLSerializer(DroneList, dronePath);
            //XMLTools.SaveListToXMLSerializer(StationsList, stationPath);
            //XMLTools.SaveListToXMLSerializer(CustomersList, customerPath);
            //XMLTools.SaveListToXMLSerializer(ParcelsList, parcelPath);
            //XMLTools.SaveListToXMLSerializer(DChargeList, dronechargePath);

        }

        private static void createDrone(int num)//the function creates a given number of drones and adds them to the list
        {
            for (int i = 0; i < num; i++)
            {
                Drone d = new Drone()//creating the drone
                {
                    Id = rand.Next(1000, 10001),
                    Model = "",
                    MaxWeight = (WeightCategories)rand.Next(3),
                };
                DroneList.Add(d);//adding to the list
            }
        }

        private static void createParcel(int num)//the function creates a given number of parcels and adds them to the list
        {
            for (int i = 0; i < num; i++)
            {
                Parcel p = new Parcel()//creating the parcel
                {
                    Id = Config.countIdParcel++,
                    SenderId = CustomersList[rand.Next(10)].Id,
                    Weight = (WeightCategories)rand.Next(3),
                    Priority = (Priorities)rand.Next(2),
                    Requested = DateTime.Now,
                    DroneId = 0,
                    Scheduled = null,
                    PickedUp = null,
                    Delivered = null,
                    IsDelivered = false,
                    IsPicked=false
                };
                do
                {
                    p.TargetId = CustomersList[rand.Next(10)].Id;

                } while(p.TargetId==p.SenderId);
                ParcelsList.Add(p);
            }
        }

        private static void createStation(int num)//the function creates a given number of stations and adds them to the list
        {
            for (int i = 0; i < num; i++)
            {
                Station s = new Station()//creating the station and getting random values
                {
                    Id = rand.Next(1000, 10001),
                    Name = " ",
                    Lattitude = rand.Next(30, 33) + ((double)rand.Next(0, 1000000) / 1000000),
                    Longitude = rand.Next(34, 36) + ((double)rand.Next(0, 1000000) / 1000000),
                    ChargeSlots = rand.Next(11)
                };
                StationsList.Add(s);
                //Config.availableStations++;
            }

        }

        private static void creacustomer(int num)//the function creates a given number of customers and adds them to the list
        {

            for (int i = 0; i < num; i++)
            {
                Customer c = new Customer()//creating the customer and getting random values
                {
                    Id = rand.Next(100000000, 1000000000),
                    Name = "name"+i,
                    Phone = "un",
                    Lattitude = rand.Next(30, 33) + ((double)rand.Next(0, 1000000) / 1000000),
                    Longitude = rand.Next(34, 36) + ((double)rand.Next(0, 1000000) / 1000000),
                    password = null
                };
                CustomersList.Add(c);
            }
        }
       
       
        public static void  Intialize()
        {
             //שדה סטטי של המחלקה
            
            //createStation(5);
            //createDrone(5);
            //creacustomer(10);
            //createParcel(5);

        }
    }
}
