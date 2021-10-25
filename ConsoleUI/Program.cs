using System;


using IDAL.DO;

namespace ConsoleUI
{
    class Program
    {
        enum MenuOpt { Exit, Add, Update, ShowOne, ShowList }
        enum AddOpt { Exit, AddDrone, AddStation, AddParcel, AddCustomer }
        enum UpdateOpt { Exit, AddDrone, AddStation, AddParcel, AddCustomer }
        enum ShowONEOpt { Exit, AddDrone, AddStation, AddParcel, AddCustomer }
        enum ShowListOpt { Exit, AddDrone, AddStation, AddParcel, AddCustomer }
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

                case MenuOpt.Add: AddOpt addO;
                     flag = int.TryParse(Console.ReadLine(), out option);
                    addO = (AddOpt)option;
                    switch (addO)
                    {
                        case AddOpt.Exit:
                            break;
                        case AddOpt.AddDrone:
                            break;
                        case AddOpt.AddStation:
                            break;
                        case AddOpt.AddParcel:
                            break;
                        case AddOpt.AddCustomer:
                            break;
                        default:
                            break;
                    }
                    break;

                case MenuOpt.Update: 
                    break;
                case MenuOpt.ShowOne:
                    break;
                case MenuOpt.ShowList:
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
