using System;


using IDAL.DO;
using DalObject;

namespace ConsoleUI
{
    class Program
    {
        //the menue options for each case
        enum MenuOpt { Exit, Add, Update, ShowOne, ShowList, CalculateD}
        enum AddOpt { Exit, AddDrone, AddStation, AddParcel, AddCustomer }
        enum UpdateOpt { Exit, linkParcelToDrone, PickParcel, DeliveringParcel, DroneToCharge, EndingCharge }
        enum ShowONEOpt { Exit, ShowDrone, ShowStation, ShowParcel, ShowCustomer }
        enum ShowListOpt { Exit, ShowDrone, ShowStation, ShowParcel, ShowCustomer, UnmatchedParcels, EmptySlots }
        enum CalcOPT { Exit,CustomerS, stationD}
        public static void PrintMenu()
        {
            bool flag2;
            MenuOpt mo;
            int option;
            Console.WriteLine("1-5 \n 0: exit, 1: add, 2:update, 3:print one, 4:print list, 5:calculate distance");
            bool flag = int.TryParse(Console.ReadLine(), out option);//getting a chosen action from the user
            mo = (MenuOpt)option;
            while (option < 6)//we have 6 differnt options of sactions, so whie the user chose one of them
            {
                switch (mo)
                {
                    case MenuOpt.Exit: return;//if chose to exit
                        break;
                    case MenuOpt.Add://if chose to add
                        AddOpt addO;
                        flag = int.TryParse(Console.ReadLine(), out option);//checking its an int type
                        addO = (AddOpt)option;
                        Console.WriteLine("add");
                        switch (addO)//checking ehat he chose to add
                        {
                            case AddOpt.Exit: return;//if wants to exit
                                break;
                            case AddOpt.AddDrone://adding a drone
                                Drone d = new Drone();//creating an empty drone and getting inputs for values
                                d.Id = Convert.ToInt32(Console.ReadLine());
                                d.Model= Console.ReadLine();
                                d.Status= (DroneStatuses)Convert.ToInt32(Console.ReadLine());
                                d.MaxWeight= (WeightCategories)Convert.ToInt32(Console.ReadLine());
                                d.Battery= Convert.ToInt32(Console.ReadLine());
                                DalObject.DalObject.addDrone(d);//sending to the func to add to the list
                                break;
                            case AddOpt.AddStation://adding a station
                                Station s = new Station();//creating an empty station and getting inputs for values
                                s.Id = (Convert.ToInt32(Console.ReadLine()));
                                s.Name = Console.ReadLine();
                                s.Lattitude = Convert.ToInt64(Console.ReadLine());
                                s.Longitude = Convert.ToInt64(Console.ReadLine());
                                s.ChargeSlots = Convert.ToInt32(Console.ReadLine());
                                DalObject.DalObject.addStation(s);//sending to the func to add to the list
                                break;
                            case AddOpt.AddParcel://adding a parcel
                                Parcel per = new Parcel();//creating an empty parcel and getting inputs for values
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
                                DalObject.DalObject.addParcel(per);//sending to the func to add to the list
                                break;
                            case AddOpt.AddCustomer://adding a customer
                                Customer cus = new Customer();//creating an empty customer and getting inputs for values
                                cus.Id = Convert.ToInt32(Console.ReadLine());
                                cus.Name = Console.ReadLine();
                                cus.Lattitude = Convert.ToInt64(Console.ReadLine());
                                cus.Longitude = Convert.ToInt64(Console.ReadLine());
                                cus.Phone = Console.ReadLine();
                                DalObject.DalObject.addCustomer(cus);//sending to the func to add to the list
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
                        switch (upp)//update options:
                        {
                            case UpdateOpt.Exit:
                                break;
                            case UpdateOpt.linkParcelToDrone://linking a parcel to a drone (the parcel will be sent by it)
                                parcelid = Convert.ToInt32(Console.ReadLine());
                                droneId = Convert.ToInt32(Console.ReadLine());
                                DalObject.DalObject.linkParcelToDrone(parcelid, droneId);//sending the id to the funct that will link them
                                break;
                            case UpdateOpt.PickParcel://updating a parcel to be picked
                                parcelid = Convert.ToInt32(Console.ReadLine());//getting the id parcel
                                DalObject.DalObject.pickParcel(parcelid);//sending to the func to update it
                                break;
                            case UpdateOpt.DeliveringParcel://updating a parcel to be delivered
                                parcelid = Convert.ToInt32(Console.ReadLine());
                                DalObject.DalObject.deliveringParcel(parcelid);//sending to the func to update it
                                break;
                            case UpdateOpt.DroneToCharge://sending a chosen drone to charge
                                parcelid = Convert.ToInt32(Console.ReadLine());
                                stationId = Convert.ToInt32(Console.ReadLine());
                                DalObject.DalObject.droneToCharge(parcelid, stationId);//sending to the func to update it
                                break;
                            case UpdateOpt.EndingCharge://ending the charge of a drone
                                parcelid = Convert.ToInt32(Console.ReadLine());
                                DalObject.DalObject.EndingCharge(parcelid);//sending the chosen parcel's id to the func to update it
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
                        switch (soo)//printing options:
                        {
                            case ShowONEOpt.Exit:
                                break;
                            case ShowONEOpt.ShowDrone://printing a chosen drone
                                id = Console.Read();
                                Console.WriteLine(DalObject.DalObject.ShowOneDrone(id));
                                break;
                            case ShowONEOpt.ShowStation://printing a chosen station
                                id = Console.Read();
                                Console.WriteLine(DalObject.DalObject.ShowOneStation(id));
                                break;
                            case ShowONEOpt.ShowParcel://printing a chosen parcel
                                id = Console.Read();
                                Console.WriteLine(DalObject.DalObject.ShowOneParcel(id));
                                break;
                            case ShowONEOpt.ShowCustomer://printing a chosen customer
                                id = Console.Read();
                                Console.WriteLine(DalObject.DalObject.ShowOneCustomer(id));
                                break;
                            default:
                                break;
                        }

                        break;
                    case MenuOpt.ShowList:
                        Console.WriteLine("1-6");
                        ShowListOpt slo;
                        flag = int.TryParse(Console.ReadLine(), out option);
                        slo = (ShowListOpt)option;
                        switch (slo)//list printing options:
                        {
                            case ShowListOpt.Exit:
                                break;
                            case ShowListOpt.ShowDrone://printing the drones list
                                DalObject.DalObject.ShowDrone();
                                break;
                            case ShowListOpt.ShowStation://printing the stations list
                                DalObject.DalObject.ShowStation();
                                break;
                            case ShowListOpt.ShowParcel://printing the parcels list
                                DalObject.DalObject.ShowParcel();
                                break;
                            case ShowListOpt.ShowCustomer://printing the customers list
                                DalObject.DalObject.ShowCustomer();
                                break;
                            case ShowListOpt.UnmatchedParcels://printing all the unmatced parcels 
                                DalObject.DalObject.UnmatchedParcels();
                                break;
                            case ShowListOpt.EmptySlots://printing all the stations that has empty slots
                                DalObject.DalObject.ShowEmptySlots();
                                break;
                            default:
                                break;
                        }
                        break;

                    case MenuOpt.CalculateD:
                        Console.WriteLine("1-2");
                        CalcOPT calc;
                        flag = int.TryParse(Console.ReadLine(), out option);
                        calc = (CalcOPT)option;
                        int idS;
                        double lat, lon;
                        switch (calc)//calc options:
                        {
                            case CalcOPT.Exit:
                                break;
                            case CalcOPT.CustomerS://distance from location to customer
                                Console.WriteLine("choose customer, id, and insert lat and lon");
                                idS = int.Parse(Console.ReadLine());//getting customer id
                                lat = double.Parse(Console.ReadLine());//getting location
                                lon = double.Parse(Console.ReadLine());
                                DalObject.DalObject.CustomerDistance(lat, lon, idS);
                                break;
                            case CalcOPT.stationD://distance from location to station
                                Console.WriteLine("choose station, id, and insert lat and lon");
                                idS = int.Parse(Console.ReadLine());//getting station id
                                 lat = double.Parse(Console.ReadLine());//getting location
                                 lon = double.Parse(Console.ReadLine());
                                Console.WriteLine( (float)DalObject.DalObject.StationDistance(lat, lon, idS));
                                break;
                            
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
                Console.WriteLine("again");//another action
                flag2 = int.TryParse(Console.ReadLine(), out option);
                mo = (MenuOpt)option;
            }
            
        }
        static void Main(string[] args)
        {
            DalObject.DalObject dl = new DalObject.DalObject();
            int num;
            PrintMenu();            
        }
    }
}
  