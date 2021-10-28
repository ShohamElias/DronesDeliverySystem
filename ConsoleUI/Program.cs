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
        void PrintMenu()
        {
            
            MenuOpt mo;
            int option;
            bool flag = int.TryParse(Console.ReadLine(), out option);
            mo = (MenuOpt)option;
            switch (mo)
            {
                case MenuOpt.Exit:
                    break;
                case MenuOpt.Add:
                AddOpt addO;
                    flag = int.TryParse(Console.ReadLine(), out option);
                    addO = (AddOpt)option;
                    switch (addO)
                    {
                        case AddOpt.Exit:
                            break;
                        case AddOpt.AddDrone:
                           //?????
                            break;
                        case AddOpt.AddStation:
                            Station s = new Station();
                            s.Id = Console.Read();
                            s.Name = Console.ReadLine();
                            s.Lattitude = Console.Read();
                            s.Longitude = Console.Read();
                            s.ChargeSlots = Console.Read();
                            DalObject.DalObject.addStation(s);
                            break;
                        case AddOpt.AddParcel:
                            Parcel per = new Parcel();
                            per.Id =Console.Read();
                            per.TargetId = Console.Read();
                            //  per.Weight = Console.ReadLine();
                            // per.Priority = Console.Read();
                            //  per.Requested = Console.Read();
                            per.DroneId = Console.Read();
                            per.SenderId = Console.Read();
                            //per.Scheduled = Console.Read();
                            //per.PickedUp = Console.Read();
                            //per.Delivered = Console.Read();
                            DalObject.DalObject.addParcel(per);
                            break;
                        case AddOpt.AddCustomer:
                            Customer cus = new Customer();
                            cus.Id = Console.Read();
                            cus.Name= Console.ReadLine();
                            cus.Lattitude = Console.Read();
                            cus.Longitude = Console.Read();
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
                             parcelid = Console.Read();
                             droneId = Console.Read();
                            DalObject.DalObject.linkParcelToDrone(parcelid, droneId);
                            break;
                        case UpdateOpt.PickParcel:
                             parcelid = Console.Read();
                            DalObject.DalObject.pickParcel(parcelid);
                            break;
                        case UpdateOpt.DeliveringParcel:
                            parcelid = Console.Read();
                            DalObject.DalObject.deliveringParcel(parcelid);
                            break;
                        case UpdateOpt.DroneToCharge:
                            parcelid = Console.Read();
                            stationId = Console.Read();
                            DalObject.DalObject.droneToCharge(parcelid, stationId);
                            break;
                        case UpdateOpt.EndingCharge:
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
        }
        static void Main(string[] args)
        {
        }
    }
}
  