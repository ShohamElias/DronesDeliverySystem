using System;


using System.Linq;
using System.Collections.Generic;
using IBL.BO;
namespace ConsoleUI_BL
{
    class Program
    {
        //the menue options for each case
        enum MenuOpt { Exit, Add, Update, ShowOne, ShowList, CalculateD}
        enum AddOpt { Exit, AddDrone, AddStation, AddParcel, AddCustomer }
        enum UpdateOpt { Exit, linkParcelToDrone, PickParcel, DeliveringParcel, DroneToCharge, EndingCharge, DroneModel, Station, Customer, }
        enum ShowONEOpt { Exit, ShowDrone, ShowStation, ShowParcel, ShowCustomer }
        enum ShowListOpt { Exit, ShowDrone, ShowStation, ShowParcel, ShowCustomer, UnmatchedParcels, EmptySlots }
        public static void PrintMenu()
        {
            IBL.BL bl = new();
            bool flag2;
            MenuOpt mo;
            int option, neededId;
            Console.WriteLine("1-5 \n 0: exit, 1: add, 2:update, 3:print one, 4:print list");
            bool flag = int.TryParse(Console.ReadLine(), out option);//getting a chosen action from the user
            mo = (MenuOpt)option;
            while (option < 6)//we have 6 differnt options of sactions, so whie the user chose one of them
            {
                try
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
                                    Console.WriteLine("id, model, maxweight");
                                    Drone d = new Drone();//creating an empty drone and getting inputs for values
                                    d.Id = Convert.ToInt32(Console.ReadLine());
                                    d.Model = Console.ReadLine();
                                    d.MaxWeight = (WeightCategories)Convert.ToInt32(Console.ReadLine());
                                    List<Station> StationsList = bl.GetStationsforNoEmpty().ToList();
                                    foreach (Station item in StationsList)
                                    {
                                        Console.WriteLine(item);
                                    }
                                    Console.WriteLine("choose a station id");
                                    neededId = Convert.ToInt32(Console.ReadLine());
                                    bl.AddDrone(d, neededId);//sending to the func to add to the list################### why theres no regular add??????
                                    break;
                                case AddOpt.AddStation://adding a station
                                    Console.WriteLine("id, name, long, lat, chargeslots");
                                    Station s = new Station();//creating an empty station and getting inputs for values
                                    s.StationLocation = new Location();
                                    s.Id = int.Parse(Console.ReadLine());
                                    s.Name = Console.ReadLine();
                                    s.StationLocation.Lattitude = double.Parse(Console.ReadLine());
                                    s.StationLocation.Longitude = double.Parse(Console.ReadLine());
                                    s.ChargeSlots = Convert.ToInt32(Console.ReadLine());
                                    bl.AddStation(s);//sending to the func to add to the list
                                    break;
                                case AddOpt.AddParcel://adding a parcel
                                    Console.WriteLine("id, trget id, weight, priority, sender id");
                                    Parcel per = new Parcel();//creating an empty parcel and getting inputs for values
                                    per.Sender = new CustomerInParcel();
                                    per.Target = new CustomerInParcel();
                                    per.Id = Convert.ToInt32(Console.ReadLine());
                                    per.Target.Id = Convert.ToInt32(Console.ReadLine());
                                    per.Weight = (WeightCategories)Convert.ToInt32(Console.ReadLine());
                                    per.Priority = (Priorities)Convert.ToInt32(Console.ReadLine());
                                    per.Sender.Id = Convert.ToInt32(Console.ReadLine());
                                    bl.AddParcel(per);//sending to the func to add to the list
                                    break;
                                case AddOpt.AddCustomer://adding a customer
                                    Console.WriteLine("id, name, phone, lat, long");
                                    Customer cus = new Customer();//creating an empty customer and getting inputs for values
                                    cus.CustLocation = new Location();
                                    cus.Id = Convert.ToInt32(Console.ReadLine());
                                    cus.Name = Console.ReadLine();
                                    cus.Phone = Console.ReadLine();
                                    cus.CustLocation.Lattitude = double.Parse(Console.ReadLine());
                                    cus.CustLocation.Longitude = double.Parse(Console.ReadLine());
                                    bl.AddCustomer(cus);//sending to the func to add to the list
                                    break;
                                default:
                                    break;
                            }
                            break;

                        case MenuOpt.Update:
                            Console.WriteLine("0: exit, 1: parceltodrone, 2:pickparcel, 3:deliver parcel , 4:drone to charge, 5: Ending charge, 6: DroneModel, 7: Station, 8: Customer");
                            UpdateOpt upp;
                            flag = int.TryParse(Console.ReadLine(), out option);
                            upp = (UpdateOpt)option;
                            int parcelid, Id, stationId, time;
                            string nameChange;
                            switch (upp)//update options:
                            {
                                case UpdateOpt.Exit:
                                    break;
                                case UpdateOpt.linkParcelToDrone://linking a parcel to a drone (the parcel will be sent by it)
                                    Console.WriteLine("please enter drone id");
                                    Id = Convert.ToInt32(Console.ReadLine());
                                    bl.LinkDroneToParcel(Id);//sending the id to the funct that will link them
                                    break;
                                case UpdateOpt.PickParcel://updating a parcel to be picked
                                    Console.WriteLine("please enter drone id");
                                    parcelid = Convert.ToInt32(Console.ReadLine());//getting the id drone
                                    bl.PickParcel(parcelid);//sending to the func to update it
                                    break;
                                case UpdateOpt.DeliveringParcel://updating a parcel to be delivered
                                    Console.WriteLine("please enter drone id");
                                    parcelid = Convert.ToInt32(Console.ReadLine());
                                    bl.DeliveringParcel(parcelid);//sending to the func to update it
                                    break;
                                case UpdateOpt.DroneToCharge://sending a chosen drone to charge
                                    Console.WriteLine("please enter drone id");
                                    Id = Convert.ToInt32(Console.ReadLine());
                                    bl.DroneToCharge(Id);//sending to the func to update it
                                    break;
                                case UpdateOpt.EndingCharge://ending the charge of a drone
                                    Console.WriteLine("please enter drone id");
                                    Id = Convert.ToInt32(Console.ReadLine());
                                  //  time = Convert.ToInt32(Console.ReadLine());//what time means???##########
                                    bl.EndCharging(Id);//sending the chosen parcel's id to the func to update it
                                    break;
                                case UpdateOpt.DroneModel://sending a chosen drone to change its model
                                    Console.WriteLine("please enter drone id and the new model");
                                    Id = Convert.ToInt32(Console.ReadLine());
                                    nameChange = Console.ReadLine();
                                    bl.UpdateDrone(Id, nameChange);//sending to the func to update it
                                    break;
                                case UpdateOpt.Station:
                                    Console.WriteLine("please enter station id, name and number of charge slots");
                                    Id = Convert.ToInt32(Console.ReadLine());
                                    nameChange = Console.ReadLine();
                                    time = Convert.ToInt32(Console.ReadLine());//time= chargingslots
                                    bl.Updatestation(Id, nameChange, time);//sending the chosen parcel's id to the func to update it
                                    break;
                                case UpdateOpt.Customer:
                                    Console.WriteLine("please enter customer id. name, phone ");
                                    Id = Convert.ToInt32(Console.ReadLine());
                                    nameChange = Console.ReadLine();
                                    time = Convert.ToInt32(Console.ReadLine());//time= phone
                                    bl.UpdateCustomer(Id, nameChange, time.ToString());//sending the chosen parcel's id to the func to update it
                                    break;
                                default:
                                    break;
                            }
                            break;

                        case MenuOpt.ShowOne:
                            Console.WriteLine("1-4  \n 1:Drone, 2:Station, 3:Parcel, 4:Customer");
                            ShowONEOpt soo;
                            int id;
                            flag = int.TryParse(Console.ReadLine(), out option);
                            soo = (ShowONEOpt)option;
                            switch (soo)//printing options:
                            {
                                case ShowONEOpt.Exit:
                                    break;
                                case ShowONEOpt.ShowDrone://printing a chosen drone
                                    Console.WriteLine("please enter Drone id:");
                                    id = int.Parse(Console.ReadLine());
                                    Console.WriteLine(bl.GetDrone(id));
                                    break;
                                case ShowONEOpt.ShowStation://printing a chosen station
                                    Console.WriteLine("please enter station id:");
                                    id = int.Parse(Console.ReadLine());
                                    Console.WriteLine(bl.GetStation(id));
                                    break;
                                case ShowONEOpt.ShowParcel://printing a chosen parcel
                                    Console.WriteLine("please enter parcel id:");
                                    id = int.Parse(Console.ReadLine());
                                    Console.WriteLine(bl.GetParcel(id));
                                    break;
                                case ShowONEOpt.ShowCustomer://printing a chosen customer
                                    Console.WriteLine("please enter customer id:");
                                    id = int.Parse(Console.ReadLine());
                                    Console.WriteLine(bl.GetCustomer(id));
                                    break;
                                default:
                                    break;
                            }

                            break;
                        case MenuOpt.ShowList:
                            Console.WriteLine("1-6");
                            Console.WriteLine("1-6  \n 1:Drone List, 2: Station List, 3:Parcel List, 4:Customer List, 5: Unmmached parcels List, 6: Stations with empty slots list");

                            ShowListOpt slo;
                            flag = int.TryParse(Console.ReadLine(), out option);
                            slo = (ShowListOpt)option;
                            switch (slo)//list printing options:
                            {
                                case ShowListOpt.Exit:
                                    break;
                                case ShowListOpt.ShowDrone://printing the drones list
                                    List<Drone> temp = bl.GetAllDrones().ToList();
                                    foreach (Drone item in temp)
                                    {
                                        Console.WriteLine(item);
                                    }
                                    break;
                                case ShowListOpt.ShowStation://printing the stations list
                                    List<Station> Stations = bl.GetAllStations().ToList();
                                    foreach (Station item in Stations)
                                    {
                                        Console.WriteLine(item);
                                    }
                                    break;
                                case ShowListOpt.ShowParcel://printing the parcels list
                                      List<Parcel> ListParcel = bl.GetAllParcels().ToList();
                                    foreach (Parcel item in ListParcel)
                                    {
                                        Console.WriteLine(item);
                                    }
                                    break;
                                case ShowListOpt.ShowCustomer://printing the customers list
                                    List<Customer> ListCustomer = bl.GetAllCustomers().ToList();
                                    foreach (Customer item in ListCustomer)
                                    {
                                        Console.WriteLine(item);
                                    }
                                    break;
                                case ShowListOpt.UnmatchedParcels://printing all the unmatced parcels 
                                    List<Parcel> ListP = bl.GetAllUnmachedParcels().ToList();
                                    foreach (Parcel item in ListP)
                                    {
                                        //if (item.DroneParcel.Id == 0)
                                            Console.WriteLine(item);
                                    }
                                    break;
                                case ShowListOpt.EmptySlots://printing all the stations that has empty slots
                                    List<Station> StationsList = bl.GetStationsforNoEmpty().ToList();
                                    foreach (Station item in StationsList)
                                    {
                                            Console.WriteLine(item);
                                    }
                                    break;
                                default:
                                    break;
                            }
                            break;

                   
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
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