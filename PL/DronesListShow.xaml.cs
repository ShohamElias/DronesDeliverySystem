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
    /// Interaction logic for DronesListShow.xaml
    /// </summary>
    public partial class DronesListShow : Window
    {
        IBL.IBL bl;
        public DronesListShow(IBL.IBL _bl)
        {
            InitializeComponent();
            DronesListView.ItemsSource = _bl.GetAllDrones();
            bl = _bl;
            StatusSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string str = StatusSelector.SelectedIndex.ToString();
            int choice = Convert.ToInt32(str);
            //DronesListView.ItemsSource = bl.GetDroneBy()//#########

        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string str = WeightSelector.SelectedIndex.ToString();
            int choice = Convert.ToInt32(str);
            //DronesListView.ItemsSource = bl.GetDroneBy()//#########
        }

        private void AddDrone_Click(object sender, RoutedEventArgs e)
        {
            new DroneShow(bl).Show();
        }
    }
}
