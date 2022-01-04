using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationListWindow.xaml
    /// </summary>
    public partial class StationListWindow : Window
    {
        public StationListWindow(BlApi.IBL _bl)
        {
            InitializeComponent();
            stationsToListListView.DataContext = from item in _bl.GetAllStations()
                                                 select new BO.StationsToList()
                                                 {
                                                     Id = item.Id,
                                                     Name = item.Name,
                                                     StationLocation = item.StationLocation,
                                                     EmptyChargeSlots = item.ChargeSlots,
                                                     FullChargeSlots = item.DronesinCharge.Count
                                                 };
        }
    }
}
