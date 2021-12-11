using System;


using IDAL.DO;
using DalObject;
using System.Linq;
using System.Collections.Generic;
//Shoham Elias 325289338
//Adi Rozental 212726442
//targil 1
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
            DalObject.DalObject dal = new();
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
                    case MenuOpt.Add://if chose to add
                        Console.WriteLine("1-5 \n 0: exit, 1: drone, 2:station, 3:parcel , 4:customer");
                        AddOpt addO;
                        flag = int.TryParse(Console.ReadLine(), out option);//checking its an int type
                        addO = (AddOpt)option;
                        Console.WriteLine("add");
                        switch (addO)//checking ehat he chose to add
                        {
                            case AddOpt.Exit: return;//if wants to exit
                            case AddOpt.AddDrone://adding a drone
                                Console.WriteLine("id, model, status, maxweight, battery,");
                                Drone d = new Drone();//creating an empty drone and getting inputs for values
                                d.Id = Convert.ToInt32(Console.ReadLine());
                                d.Model= Console.ReadLine();
                                //d.Status= (DroneStatuses)Convert.ToInt32(Console.ReadLine());######################
                                d.MaxWeight= (WeightCategories)Convert.ToInt32(Console.ReadLine());
                                //d.Battery= Convert.ToInt32(Console.ReadLine());###########################
                                dal.AddDrone(d);//sending to the func to add to the list
                                break;
                            case AddOpt.AddStation://adding a station
                                Station s = new Station();//creating an empty station and getting inputs for values
                                Console.WriteLine("id, name, lang, lat, charge slot number,");
                                s.Id = (Convert.ToInt32(Console.ReadLine()));
                                s.Name = Console.ReadLine();
                                s.Lattitude = Convert.ToInt64(Console.ReadLine());
                                s.Longitude = Convert.ToInt64(Console.ReadLine());
                                s.ChargeSlots = Convert.ToInt32(Console.ReadLine());
                               dal.AddStation(s);//sending to the func to add to the list
                                break;
                            case AddOpt.AddParcel://adding a parcel
                                Console.WriteLine("id, trget id, weight, priority, sender id");
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
                                dal.AddParcel(per);//sending to the func to add to the list
                                break;
                            case AddOpt.AddCustomer://adding a customer
                                Console.WriteLine("id, name, lat, long , phone");
                                Customer cus = new Customer();//creating an empty customer and getting inputs for values
                                cus.Id = Convert.ToInt32(Console.ReadLine());
                                cus.Name = Console.ReadLine();
                                cus.Lattitude = Convert.ToInt64(Console.ReadLine());
                                cus.Longitude = Convert.ToInt64(Console.ReadLine());
                                cus.Phone = Console.ReadLine();
                               dal.AddCustomer(cus);//sending to the func to add to the list
                                break;
                            default:
                                break;
                        }
                        break;

                    case MenuOpt.Update:
                        Console.WriteLine("0: exit, 1: parceltodrone, 2:pickparcel, 3:deliver parcel , 4:drone to charge, 5: Ending charge");
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
                                dal.LinkParcelToDrone(parcelid, droneId);//sending the id to the funct that will link them
                                break;
                            case UpdateOpt.PickParcel://updating a parcel to be picked
                                parcelid = Convert.ToInt32(Console.ReadLine());//getting the id parcel
                                dal.PickParcel(parcelid);//sending to the func to update it
                                break;
                            case UpdateOpt.DeliveringParcel://updating a parcel to be delivered
                                parcelid = Convert.ToInt32(Console.ReadLine());
                                dal.DeliveringParcel(parcelid);//sending to the func to update it
                                break;
                            case UpdateOpt.DroneToCharge://sending a chosen drone to charge
                                parcelid = Convert.ToInt32(Console.ReadLine());
                                stationId = Convert.ToInt32(Console.ReadLine());
                                dal.DroneToCharge(parcelid, stationId);//sending to the func to update it
                                break;
                            case UpdateOpt.EndingCharge://ending the charge of a drone
                                parcelid = Convert.ToInt32(Console.ReadLine());
                                dal.EndingCharge(parcelid);//sending the chosen parcel's id to the func to update it
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
                                id = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine(dal.ShowOneDrone(id));
                                break;
                            case ShowONEOpt.ShowStation://printing a chosen station
                                id = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine(dal.ShowOneStation(id));
                                break;
                            case ShowONEOpt.ShowParcel://printing a chosen parcel
                                id = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine(dal.ShowOneParcel(id));
                                break;
                            case ShowONEOpt.ShowCustomer://printing a chosen customer
                                id = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine(dal.ShowOneCustomer(id));
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
                                List<Drone> temp = dal.ListDrone().ToList();
                                foreach (Drone item in temp)
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            case ShowListOpt.ShowStation://printing the stations list
                                List<Station> Stations = dal.ListStation().ToList();
                                foreach (Station item in Stations)
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            case ShowListOpt.ShowParcel://printing the parcels list
                                List<Parcel> ListParcel = dal.ListParcel().ToList();
                                foreach (Parcel item in ListParcel)
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            case ShowListOpt.ShowCustomer://printing the customers list
                                List<Customer> ListCustomer = dal.ListCustomer().ToList();
                                foreach (Customer item in ListCustomer)
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            case ShowListOpt.UnmatchedParcels://printing all the unmatced parcels 
                                List<Parcel> ListP = dal.ListParcel().ToList();
                                foreach (Parcel item in ListP)
                                {
                                    if(item.DroneId==0)
                                       Console.WriteLine(item);
                                }
                                break;
                            case ShowListOpt.EmptySlots://printing all the stations that has empty slots
                                List<Station> StationsList = dal.ListStation().ToList();
                                foreach (Station item in StationsList)
                                {
                                    if(item.ChargeSlots!=0)
                                       Console.WriteLine(item);
                                }
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
                                dal.CustomerDistance(lat, lon, idS);
                                break;
                            case CalcOPT.stationD://distance from location to station
                                Console.WriteLine("choose station, id, and insert lat and lon");
                                idS = int.Parse(Console.ReadLine());//getting station id
                                 lat = double.Parse(Console.ReadLine());//getting location
                                 lon = double.Parse(Console.ReadLine());
                                Console.WriteLine( (float)dal.StationDistance(lat, lon, idS));
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
            PrintMenu();            
        }
    }
}
  