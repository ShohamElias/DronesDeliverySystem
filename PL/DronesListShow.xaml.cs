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
        BlApi.IBL bl;
        public DronesListShow(BlApi.IBL _bl)
        {
            InitializeComponent();
            DronesListView.ItemsSource = _bl.GetAllDrones();
            bl = _bl;

            StatusSelector.ItemsSource = Enum.GetValues(typeof(BO.DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string str = StatusSelector.SelectedIndex.ToString();
            int choice = Convert.ToInt32(str);
            //DronesListView.ItemsSource = bl.GetDroneBy()//#########
            Predicate<BO.Drone> p;
            if (WeightSelector.SelectedIndex != -1)
                p = s => s.MaxWeight == (BO.WeightCategories)WeightSelector.SelectedIndex && s.Status == (BO.DroneStatuses)StatusSelector.SelectedIndex;
            else
                p = s => s.Status == (BO.DroneStatuses)StatusSelector.SelectedIndex;
            DronesListView.ItemsSource = bl.GetDroneBy(p);
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string str = WeightSelector.SelectedIndex.ToString();
            int choice = Convert.ToInt32(str);

           
            Predicate<BO.Drone> p;
            if (StatusSelector.SelectedIndex != -1)
                p = s => s.MaxWeight == (BO.WeightCategories)WeightSelector.SelectedIndex && s.Status == (BO.DroneStatuses)StatusSelector.SelectedIndex;
            else
                p = s => s.MaxWeight == (BO.WeightCategories)WeightSelector.SelectedIndex;
            DronesListView.ItemsSource = bl.GetDroneBy(p);

        }

        private void AddDrone_Click(object sender, RoutedEventArgs e)
        {
            new DroneShow(bl).ShowDialog();

        }


        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //  IBL.BO.Drone d= DronesListView.InputHitTest(e.XButton1, e.XButton2);
            ListViewItem item = sender as ListViewItem;
            BO.Drone d = DronesListView.SelectedItem as BO.Drone;
            if (d != null)
                new DroneShow(bl, d).Show();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            Predicate<BO.Drone> p = null;
            if (StatusSelector.SelectedIndex != -1 && WeightSelector.SelectedIndex != -1)
                p = s => s.MaxWeight == (BO.WeightCategories)WeightSelector.SelectedIndex && s.Status == (BO.DroneStatuses)StatusSelector.SelectedIndex;
            else if (WeightSelector.SelectedIndex != -1)
                p = s => s.MaxWeight == (BO.WeightCategories)WeightSelector.SelectedIndex;
            else if (StatusSelector.SelectedIndex != -1)
                p = s => s.Status == (BO.DroneStatuses)StatusSelector.SelectedIndex;

            if (p == null)
                DronesListView.ItemsSource = bl.GetAllDrones();
            else
                DronesListView.ItemsSource = bl.GetDroneBy(p);

        }

        //private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}
    }
}