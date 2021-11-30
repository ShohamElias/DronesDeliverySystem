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
                                    Console.WriteLine("id, model, maxweight, STATION ID");
                                    Drone d = new Drone();//creating an empty drone and getting inputs for values
                                    d.Id = Convert.ToInt32(Console.ReadLine());
                                    d.Model = Console.ReadLine();
                                    d.MaxWeight = (WeightCategories)Convert.ToInt32(Console.ReadLine());
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
                            int parcelid, droneId, stationId, time;
                            string nameChange;
                            switch (upp)//update options:
                            {
                                case UpdateOpt.Exit:
                                    break;
                                case UpdateOpt.linkParcelToDrone://linking a parcel to a drone (the parcel will be sent by it)
                                                                 //parcelid = Convert.ToInt32(Console.ReadLine());
                                    droneId = Convert.ToInt32(Console.ReadLine());
                                    bl.LinkDroneToParcel(droneId);//sending the id to the funct that will link them
                                    break;
                                case UpdateOpt.PickParcel://updating a parcel to be picked
                                    parcelid = Convert.ToInt32(Console.ReadLine());//getting the id parcel
                                    bl.PickParcel(parcelid);//sending to the func to update it
                                    break;
                                case UpdateOpt.DeliveringParcel://updating a parcel to be delivered
                                    parcelid = Convert.ToInt32(Console.ReadLine());
                                    bl.DeliveringParcel(parcelid);//sending to the func to update it
                                    break;
                                case UpdateOpt.DroneToCharge://sending a chosen drone to charge
                                    droneId = Convert.ToInt32(Console.ReadLine());
                                    bl.DroneToCharge(droneId);//sending to the func to update it
                                    break;
                                case UpdateOpt.EndingCharge://ending the charge of a drone
                                    droneId = Convert.ToInt32(Console.ReadLine());
                                    time = Convert.ToInt32(Console.ReadLine());//what time means???##########
                                    bl.EndCharging(droneId, time);//sending the chosen parcel's id to the func to update it
                                    break;
                                case UpdateOpt.DroneModel://sending a chosen drone to charge
                                    droneId = Convert.ToInt32(Console.ReadLine());
                                    nameChange = Console.ReadLine();
                                    bl.UpdateDrone(droneId, nameChange);//sending to the func to update it
                                    break;
                                case UpdateOpt.Station://ending the charge of a drone
                                    droneId = Convert.ToInt32(Console.ReadLine());
                                    nameChange = Console.ReadLine();
                                    time = Convert.ToInt32(Console.ReadLine());//time= chargingslots
                                    bl.Updatestation(droneId, nameChange, time);//sending the chosen parcel's id to the func to update it
                                    break;
                                case UpdateOpt.Customer://ending the charge of a drone
                                    droneId = Convert.ToInt32(Console.ReadLine());
                                    nameChange = Console.ReadLine();
                                    time = Convert.ToInt32(Console.ReadLine());//time= chargingslots
                                    bl.UpdateCustomer(droneId, nameChange, time.ToString());//sending the chosen parcel's id to the func to update it
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
                                    id = int.Parse(Console.ReadLine());
                                    Console.WriteLine(bl.ShowOneDrone(id));
                                    break;
                                case ShowONEOpt.ShowStation://printing a chosen station
                                    id = int.Parse(Console.ReadLine());
                                    Console.WriteLine(bl.ShowOneStation(id));
                                    break;
                                case ShowONEOpt.ShowParcel://printing a chosen parcel
                                    id = int.Parse(Console.ReadLine());
                                    Console.WriteLine(bl.ShowOneParcel(id));
                                    break;
                                case ShowONEOpt.ShowCustomer://printing a chosen customer
                                    id = int.Parse(Console.ReadLine());
                                    Console.WriteLine(bl.ShowOneCustomer(id));
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
                                    List<Parcel> ListP = bl.GetAllParcels().ToList();
                                    foreach (Parcel item in ListP)
                                    {
                                        if (item.DroneParcel == null)
                                            Console.WriteLine(item);
                                    }
                                    break;
                                case ShowListOpt.EmptySlots://printing all the stations that has empty slots
                                    List<Station> StationsList = bl.GetAllStations().ToList();
                                    foreach (Station item in StationsList)
                                    {
                                        if (item.ChargeSlots != 0)
                                            Console.WriteLine(item);
                                    }
                                    break;
                                default:
                                    break;
                            }
                            break;

                        //case MenuOpt.CalculateD:
                        //    Console.WriteLine("1-2");
                        //    CalcOPT calc;
                        //    flag = int.TryParse(Console.ReadLine(), out option);
                        //    calc = (CalcOPT)option;
                        //    int idS;
                        //    double lat, lon;
                        //    switch (calc)//calc options:
                        //    {
                        //        case CalcOPT.Exit:
                        //            break;
                        //        case CalcOPT.CustomerS://distance from location to customer
                        //            Console.WriteLine("choose customer, id, and insert lat and lon");
                        //            idS = int.Parse(Console.ReadLine());//getting customer id
                        //            lat = double.Parse(Console.ReadLine());//getting location
                        //            lon = double.Parse(Console.ReadLine());
                        //            dal.CustomerDistance(lat, lon, idS);
                        //            break;
                        //        case CalcOPT.stationD://distance from location to station
                        //            Console.WriteLine("choose station, id, and insert lat and lon");
                        //            idS = int.Parse(Console.ReadLine());//getting station id
                        //             lat = double.Parse(Console.ReadLine());//getting location
                        //             lon = double.Parse(Console.ReadLine());
                        //            Console.WriteLine( (float)dal.StationDistance(lat, lon, idS));
                        //            break;

                        //        default:
                        //            break;
                        //    }
                        //    break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {

                    throw;
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