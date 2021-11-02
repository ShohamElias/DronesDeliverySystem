using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public class DataSource
    {
        internal class Config
        {
            internal static int dronesI = 0;
            internal static int stationsI = 0;
            internal static int customersI = 0;
            internal static int parcelsI = 0;
            public static int countIdParcel = 1;
           // public static int availableStations = 0;

        }

        //internal static Drone[] drones = new Drone[10];
        //internal static Station [] stations = new Station[5];
        //internal static Customer[] customers = new Customer[100];
        //internal static Parcel[] parcels = new Parcel[1000];
        internal static Random rand = new Random(DateTime.Now.Millisecond);//random
        //lists for the objects
        internal static List<Drone> DroneList = new List<Drone>();
        internal static List<Station> StationsList = new List<Station>();
        internal static List<Customer> CustomersList = new List<Customer>();
        internal static List<Parcel> ParcelsList = new List<Parcel>();
        internal static List<DroneCharge> DChargeList = new List<DroneCharge>();


        private static void createDrone(int num)//the function creates a given number of drones and adds them to the list
        {
            for (int i = 0; i < num; i++)
            {
                Drone d = new Drone()//creating the drone
                {
                    Id = rand.Next(1000, 10001),
                    Model = "",
                    MaxWeight = (WeightCategories)rand.Next(3),
                    Status = (DroneStatuses)rand.Next(3),
                    Battery = rand.Next(101)
                };
                dronelist.Add(d);
            }
        }

        private static void createParcel(int num)//the function creates a given number of parcels and adds them to the list
        {
            for (int i = 0; i < num; i++)
            {
                Parcel p = new Parcel()//creating the parcel
                {
                    Id = rand.Next(1000, 10001),
                    SenderId = 0,
                    TargetId = 0,
                    Weight = (WeightCategories)rand.Next(3),
                    Priority = (Priorities)rand.Next(2),
                    Requested = DateTime.Now,
                    DroneId = rand.Next(5),
                    Scheduled = DateTime.Now,
                    PickedUp = DateTime.Now,
                    Delivered = DateTime.Now
                };
                parcelsList.Add(p);
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
                stationsList.Add(s);
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
                    Name = "name",
                    Phone = "un",
                    Lattitude = rand.Next(30, 33) + ((double)rand.Next(0, 1000000) / 1000000),
                    Longitude = rand.Next(34, 36) + ((double)rand.Next(0, 1000000) / 1000000),
                };
                customersList.Add(c);
            }
        }
       
       
        public static void  Intialize()
        {
             //שדה סטטי של המחלקה
            
            createStation(2);
            createDrone(5);
            createParcel(10);
            creacustomer(10);
        }
    }
}
