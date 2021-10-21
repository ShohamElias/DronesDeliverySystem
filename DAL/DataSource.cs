using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    class DataSource
    {

        internal class Config
        {
            internal static int dronesI = 0;
            internal static int stationsI = 0;
            internal static int customersI = 0;
            internal static int parcelsI = 0;
        }

        internal static Drone[] drones = new Drone[10];
        internal static Station [] stations = new Station[5];
        internal static Customer[] customers = new Customer[100];
        internal static Parcel[] parcels = new Parcel[1000];
        static Random rand = new Random(DateTime.Now.Millisecond);

        public static void  Intialize()
        {
             //שדה סטטי של המחלקה
            stations[0] = new Station();
            stations[1] = new Station();
            Config.stationsI += 2;
            for (int i=0; i<5; i++)
            {
                drones[i] = new Drone()
                {
                    Id = Config.dronesI++,
                    Model = "",
                    MaxWeight = (WeightCategories)rand.Next(3),
                    Status = (DroneStatuses)rand.Next(3),
                    Battery = rand.Next(101)
                };
            }
            for (int i = 0; i < 10; i++)
            {
                parcels[i] = new Parcel()
                {
                    Id = Config.parcelsI++,
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
            for (int i = 0; i < 5; i++)
            {
                customers[i] = new Customer()
                {
                    Id = Config.customersI++,
                    Name = "",
                    Phone = "",
                    Longitude = 0,
                    Lattitude = 0
                };
               
            }
        }

    }
}
