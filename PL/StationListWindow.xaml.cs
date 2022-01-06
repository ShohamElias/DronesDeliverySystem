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
            stationListView.DataContext = _bl.GetAllStations();
            bl = _bl;
                                                 
        }

        private void stationListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Station s = stationListView.SelectedItem as BO.Station;
            if (s != null)
                new StationShoWindow(bl,s).Show();
        }

        private void addbutton_Click(object sender, RoutedEventArgs e)
        {
            new StationShoWindow(bl).Show();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            stationListView.DataContext =bl.GetAllStations();

        }
    }
}
