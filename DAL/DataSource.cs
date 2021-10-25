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
        }

        internal static Drone[] drones = new Drone[10];
        internal static Station [] stations = new Station[5];
        internal static Customer[] customers = new Customer[100];
        internal static Parcel[] parcels = new Parcel[1000];
        internal static Random rand = new Random(DateTime.Now.Millisecond);//not sure its internal

        //internal static List<Drone> dronelist = new List<Drone>();
        //internal static List<Station> stationsList = new List<Station>();
        //internal static List<Customer> customersList = new List<Customer>();
        //internal static List<Parcel> parcelsList = new List<Parcel>();

        private static void createDrone(int num)
        {
            for (int i=0; i < num; i++)
            {
                drones[Config.dronesI++] = new Drone()
                {
                    Id = rand.Next(1000, 10001),
                    Model = "",
                    MaxWeight = (WeightCategories)rand.Next(3),
                    Status = (DroneStatuses)rand.Next(3),
                    Battery = rand.Next(101)
                };
            }
        }

        private static void createParcel(int num)
        {
            for (int i = 0; i < num; i++)
            {
                parcels[Config.parcelsI++] = new Parcel()
                {
                    Id = rand.Next(1000, 10001),
                    SenderId = 0,
                    TargetId = 0,
                    Weight = (WeightCategories)rand.Next(3),
                    Priority = (Priorities)rand.Next(2),
                    Requested = DateTime.Now,
                    DroneId = rand.Next(5),
                    Scheduled = DateTime.Today,
                    PickedUp = DateTime.Now,
                    Delivered = DateTime.Now
                };
            }
        }

        private static void createStation(int num)
        {
            for (int i = 0; i < num; i++)
            {
                stations[Config.stationsI++] = new Station()
                {
                    Id = rand.Next(1000, 10001),
                    Name = " ",
                    Longitude = 0,
                    Lattitude = 0,
                    ChargeSlots = rand.Next(11)
                };
            }
        }

        private static void creacustomer(int num)
        {
            for (int i = 0; i < num; i++)
            {
                customers[Config.customersI++] = new Customer()
                {
                    Id = rand.Next(100000000, 1000000000),
                    Name = "",
                    Phone = "",
                    Longitude = 0,
                    Lattitude = 0
                };
            }
        }
        public static void  Intialize()
        {
             //שדה סטטי של המחלקה
            
            createDrone(5);
            createStation(2);
            createParcel(10);
            creacustomer(10);
        }
    }
}
