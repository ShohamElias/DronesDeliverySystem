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
        BlApi.IBL bl;
        public StationListWindow(BlApi.IBL _bl)
        {
            InitializeComponent();
            stationListView.DataContext = from item in _bl.GetAllStations()
                                          select new BO.StationsToList()
                                          {
                                              Id = item.Id,
                                              Name = item.Name,
                                              EmptyChargeSlots = item.ChargeSlots,
                                              FullChargeSlots = item.DronesinCharge.Count,
                                              StationLocation = item.StationLocation
                                          };
            bl = _bl;
                                                 
        }

        private void stationListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.StationsToList s = stationListView.SelectedItem as BO.StationsToList;
            if (s != null)
                new StationShoWindow(bl,bl.GetStation(s.Id)).Show();
        }

        private void addbutton_Click(object sender, RoutedEventArgs e)
        {
            new StationShoWindow(bl).Show();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            stationListView.DataContext =from item in bl.GetAllStations()
                                         select new BO.StationsToList()
                                         {
                                             Id=item.Id,
                                             Name=item.Name,
                                             EmptyChargeSlots=item.ChargeSlots,
                                             FullChargeSlots=item.DronesinCharge.Count,
                                             StationLocation=item.StationLocation
                                         };
            GroupBy.IsEnabled = true;

        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            Window_Activated( sender,  e);

        }


        private void GroupByClick_1(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(stationListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("EmptyChargeSlots");
            view.GroupDescriptions.Add(groupDescription);
            GroupBy.IsEnabled = false;
        }

        private void stationListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
