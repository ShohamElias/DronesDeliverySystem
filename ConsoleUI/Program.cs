using System;


using IDAL.DO;
using DalObject;

namespace ConsoleUI
{
    class Program
    {
        enum MenuOpt { Exit, Add, Update, ShowOne, ShowList }
        enum AddOpt { Exit, AddDrone, AddStation, AddParcel, AddCustomer }
        enum UpdateOpt { Exit, linkParcelToDrone, PickParcel, DeliveringParcel, DroneToCharge, EndingCharge }
        enum ShowONEOpt { Exit, ShowDrone, ShowStation, ShowParcel, ShowCustomer }
        enum ShowListOpt { Exit, ShowDrone, ShowStation, ShowParcel, ShowCustomer, UnmatchedParcels, EmptySlots }
        public static void PrintMenu()
        {
            bool flag2;
            MenuOpt mo;
            int option;
            Console.WriteLine("1-3");
            bool flag = int.TryParse(Console.ReadLine(), out option);
            mo = (MenuOpt)option;
            while (option < 5)
            {
                switch (mo)
                {
                    case MenuOpt.Exit:
                        break;
                    case MenuOpt.Add:
                        AddOpt addO;
                        flag = int.TryParse(Console.ReadLine(), out option);
                        addO = (AddOpt)option;
                        Console.WriteLine("add");
                        switch (addO)
                        {
                            case AddOpt.Exit:
                                break;
                            case AddOpt.AddDrone:
                                Drone d = new Drone();
                                d.Id = Convert.ToInt32(Console.ReadLine());
                                d.Model= Console.ReadLine();
                                d.Status= (DroneStatuses)Convert.ToInt32(Console.ReadLine());
                                d.MaxWeight= (WeightCategories)Convert.ToInt32(Console.ReadLine());
                                d.Battery= Convert.ToInt32(Console.ReadLine());
                                DalObject.DalObject.addDrone(d);
                                break;
                            case AddOpt.AddStation:
                                Station s = new Station();
                                s.Id = (Convert.ToInt32(Console.ReadLine()));
                                s.Name = Console.ReadLine();
                                s.Lattitude = Convert.ToInt64(Console.ReadLine());
                                s.Longitude = Convert.ToInt64(Console.ReadLine());
                                s.ChargeSlots = Convert.ToInt32(Console.ReadLine());
                                DalObject.DalObject.addStation(s);
                                break;
                            case AddOpt.AddParcel:
                                Parcel per = new Parcel();
                                per.Id = Convert.ToInt32(Console.ReadLine());
                                per.TargetId = Convert.ToInt32(Console.ReadLine());
                                per.Weight = (WeightCategories)Convert.ToInt32(Console.ReadLine());
                                per.Priority = (Priorities)Convert.ToInt32(Console.ReadLine());
                                per.Requested =DateTime.Now;
                                per.DroneId =0;
                                per.SenderId = Convert.ToInt32(Console.ReadLine());
                                per.Scheduled = DateTime.Today;
                                per.PickedUp = DateTime.Today;
                                per.Delivered = DateTime.Today;
                                DalObject.DalObject.addParcel(per);
                                break;
                            case AddOpt.AddCustomer:
                                Customer cus = new Customer();
                                cus.Id = Convert.ToInt32(Console.ReadLine());
                                cus.Name = Console.ReadLine();
                                cus.Lattitude = Convert.ToInt64(Console.ReadLine());
                                cus.Longitude = Convert.ToInt64(Console.ReadLine());
                                cus.Phone = Console.ReadLine();
                                DalObject.DalObject.addCustomer(cus);
                                break;
                            default:
                                break;
                        }
                        break;

                    case MenuOpt.Update:
                        UpdateOpt upp;
                        flag = int.TryParse(Console.ReadLine(), out option);
                        upp = (UpdateOpt)option;
                        int parcelid, droneId, stationId;
                        switch (upp)
                        {
                            case UpdateOpt.Exit:
                                break;
                            case UpdateOpt.linkParcelToDrone:
                                parcelid = Convert.ToInt32(Console.ReadLine());
                                droneId = Convert.ToInt32(Console.ReadLine());
                                DalObject.DalObject.linkParcelToDrone(parcelid, droneId);
                                break;
                            case UpdateOpt.PickParcel:
                                parcelid = Convert.ToInt32(Console.ReadLine());
                                DalObject.DalObject.pickParcel(parcelid);
                                break;
                            case UpdateOpt.DeliveringParcel:
                                parcelid = Convert.ToInt32(Console.ReadLine());
                                DalObject.DalObject.deliveringParcel(parcelid);
                                break;
                            case UpdateOpt.DroneToCharge:
                                parcelid = Convert.ToInt32(Console.ReadLine());
                                stationId = Convert.ToInt32(Console.ReadLine());
                                DalObject.DalObject.droneToCharge(parcelid, stationId);
                                break;
                            case UpdateOpt.EndingCharge:
                                parcelid = Convert.ToInt32(Console.ReadLine());
                                DalObject.DalObject.EndingCharge(parcelid);
                                break;
                            default:
                                break;
                        }
                        break;
                    case MenuOpt.ShowOne:
                        ShowONEOpt soo;
                        int id;
                        flag = int.TryParse(Console.ReadLine(), out option);
                        soo = (ShowONEOpt)option;
                        switch (soo)
                        {
                            case ShowONEOpt.Exit:
                                break;
                            case ShowONEOpt.ShowDrone:
                                id = Console.Read();
                                Console.WriteLine(DalObject.DalObject.ShowOneDrone(id));
                                break;
                            case ShowONEOpt.ShowStation:
                                id = Console.Read();
                                Console.WriteLine(DalObject.DalObject.ShowOneStation(id));
                                break;
                            case ShowONEOpt.ShowParcel:
                                id = Console.Read();
                                Console.WriteLine(DalObject.DalObject.ShowOneParcel(id));
                                break;
                            case ShowONEOpt.ShowCustomer:
                                id = Console.Read();
                                Console.WriteLine(DalObject.DalObject.ShowOneCustomer(id));
                                break;
                            default:
                                break;
                        }

                        break;
                    case MenuOpt.ShowList:
                        Console.WriteLine("1-");
                        ShowListOpt slo;
                        flag = int.TryParse(Console.ReadLine(), out option);
                        slo = (ShowListOpt)option;
                        switch (slo)
                        {
                            case ShowListOpt.Exit:
                                break;
                            case ShowListOpt.ShowDrone:
                                DalObject.DalObject.ShowDrone();
                                break;
                            case ShowListOpt.ShowStation:
                                DalObject.DalObject.ShowStation();
                                break;
                            case ShowListOpt.ShowParcel:
                                DalObject.DalObject.ShowParcel();
                                break;
                            case ShowListOpt.ShowCustomer:
                                DalObject.DalObject.ShowCustomer();
                                break;
                            case ShowListOpt.UnmatchedParcels:
                                DalObject.DalObject.UnmatchedParcels();
                                break;
                            case ShowListOpt.EmptySlots:
                                DalObject.DalObject.ShowEmptySlots();
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
                Console.WriteLine("again");
                flag2 = int.TryParse(Console.ReadLine(), out option);
                mo = (MenuOpt)option;
            }
            
        }
        static void Main(string[] args)
        {
            DalObject.DalObject dl = new DalObject.DalObject();
            int num;
            PrintMenu();
            num = Console.Read();
            
        }
    }
}
  