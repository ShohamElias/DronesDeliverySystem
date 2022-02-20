using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Globalization;
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
        public DronesListShow(BlApi.IBL _bl) //constructor
        {
            InitializeComponent();
            bl = _bl;
            DronesListView.ItemsSource = _bl.GetAllDronesToList();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(BO.DroneStatuses)); //filling the comboboxes
            WeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusSelector.SelectedItem == null)
            {
                DronesListView.ItemsSource = bl.GetAllDronesToList();
                return;
            }

            Predicate<BO.DroneToList> p;
            if (WeightSelector.SelectedIndex != -1) //if the weight selector is selected as well
                p = s => s.MaxWeight == (BO.WeightCategories)WeightSelector.SelectedIndex && s.Status == (BO.DroneStatuses)StatusSelector.SelectedIndex;
            else
                p = s => s.Status == (BO.DroneStatuses)StatusSelector.SelectedIndex;
            DronesListView.ItemsSource = bl.GetAllDronesToListBy(p); //fill the list view
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Predicate<BO.DroneToList> p;
            if (WeightSelector.SelectedItem == null)
            {
                DronesListView.ItemsSource = bl.GetAllDrones();
                return;
            }
            if (StatusSelector.SelectedIndex != -1) //if the status selector is selected as well
                p = s => s.MaxWeight == (BO.WeightCategories)WeightSelector.SelectedIndex && s.Status == (BO.DroneStatuses)StatusSelector.SelectedIndex;
            else
                p = s => s.MaxWeight == (BO.WeightCategories)WeightSelector.SelectedIndex;
            DronesListView.ItemsSource = bl.GetAllDronesToListBy(p);

        }

        private void AddDrone_Click(object sender, RoutedEventArgs e)
        {
            new DroneShow(bl).ShowDialog(); //opening the other window

        }


        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListViewItem item = sender as ListViewItem;
            BO.DroneToList d = DronesListView.SelectedItem as BO.DroneToList;
            if (d != null)
                new DroneShow(bl, bl.GetDrone(d.Id)).Show(); //opening the options widow
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            Predicate<BO.DroneToList> p = null;
            if (StatusSelector.SelectedIndex != -1 && WeightSelector.SelectedIndex != -1) //if both of the combo boxes are selected
                p = s => s.MaxWeight == (BO.WeightCategories)WeightSelector.SelectedIndex && s.Status == (BO.DroneStatuses)StatusSelector.SelectedIndex; //create the predicate
            else if (WeightSelector.SelectedIndex != -1) //if only one is selected
                p = s => s.MaxWeight == (BO.WeightCategories)WeightSelector.SelectedIndex; //create the predicate
            else if (StatusSelector.SelectedIndex != -1)
                p = s => s.Status == (BO.DroneStatuses)StatusSelector.SelectedIndex;//create the predicate
            GroupBy.IsEnabled = true;
            if (p == null) //if the comboboxes werent selected
                DronesListView.ItemsSource = bl.GetAllDronesToList();
            else
                DronesListView.ItemsSource = bl.GetAllDronesToListBy(p);

        }

        private void clearButton_Click(object sender, RoutedEventArgs e)//clearing the selectors and grouping
        {
            GroupBy.IsEnabled = true;
            StatusSelector.SelectedValue = null;
            WeightSelector.SelectedValue = null;
            DronesListView.ItemsSource = bl.GetAllDronesToList();

        }


        private void GroupByClick_1(object sender, RoutedEventArgs e)//grouping on pl level
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DronesListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Status");
            view.GroupDescriptions.Add(groupDescription);
            GroupBy.IsEnabled = false;
        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


    }
    public class BatteryConverter : IValueConverter
    {
        //convert from source property type to target property type
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Double boolValue = (Double)value;
            if (boolValue > 20)
            {
                return "Lime"; //Visibility.Collapsed;
            }
            else
            {
                return "Red";
            }
        }
        //convert from target property type to source property type
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
