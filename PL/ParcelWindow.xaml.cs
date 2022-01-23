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
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        BlApi.IBL bl;
        int nextid;
        int wish;
        IEnumerable<BO.Parcel> pp;

        public ParcelWindow(BlApi.IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            pp  = _bl.GetAllParcels();
            parcelListView.DataContext = bl.GetAllParcelsToList();
            PrioritySelector.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
        }
        private void Window_Activated(object sender, EventArgs e)
        {
            Predicate<BO.Parcel> p = null;
            if (PrioritySelector.SelectedIndex != -1 && WeightSelector.SelectedIndex != -1) //if both of the combo boxes are selected
                p = s => s.Weight == (BO.WeightCategories)WeightSelector.SelectedIndex && s.Priority == (BO.Priorities)PrioritySelector.SelectedIndex; //create the predicate
            else if (WeightSelector.SelectedIndex != -1) //if only one is selected
                p = s => s.Weight == (BO.WeightCategories)WeightSelector.SelectedIndex; //create the predicate
            else if (PrioritySelector.SelectedIndex != -1)
                p = s => s.Priority == (BO.Priorities)PrioritySelector.SelectedIndex;//create the predicate

            if (p == null) //if the comboboxes werent selected
                parcelListView.ItemsSource = bl.GetAllParcelsToList();
            else
                parcelListView.ItemsSource = bl.GetAllParcelsToListBy(p);
          
        }

        private void parcelListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.ParcelToList p = parcelListView.SelectedItem as BO.ParcelToList;
            if (p != null)
                new ParcelShow(bl.GetParcel(p.Id), bl, "").Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new ParcelShow(bl, 0).Show();
        }

        private void PrioritySelector_LostTouchCapture(object sender, TouchEventArgs e)
        {

        }

        private void PrioritySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PrioritySelector.SelectedItem == null)
            {
                parcelListView.ItemsSource = bl.GetAllParcels();
                return;
            }
            Predicate<BO.Parcel> p;
            if (WeightSelector.SelectedIndex != -1) //if the weight selector is selected as well
                p = s => s.Weight == (BO.WeightCategories)WeightSelector.SelectedIndex && s.Priority == (BO.Priorities)PrioritySelector.SelectedIndex;
            else
                p = s => s.Priority == (BO.Priorities)PrioritySelector.SelectedIndex;
            parcelListView.ItemsSource = from item in bl.GetParcelBy(p)
                                         select bl.GetParcelToList(item.Id); //fill the list view
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WeightSelector.SelectedItem == null)
            {
                parcelListView.ItemsSource = bl.GetAllParcels();
                return;
            }
            Predicate<BO.Parcel> p;
            if (PrioritySelector.SelectedIndex != -1) //if the status selector is selected as well
                p = s => s.Weight == (BO.WeightCategories)WeightSelector.SelectedIndex && s.Priority == (BO.Priorities)PrioritySelector.SelectedIndex;
            else
                p = s => s.Weight == (BO.WeightCategories)WeightSelector.SelectedIndex;
            parcelListView.ItemsSource = from item in bl.GetParcelBy(p)
                                         select bl.GetParcelToList(item.Id);
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            WeightSelector.SelectedItem = null;
            PrioritySelector.SelectedItem = null;

            parcelListView.ItemsSource = bl.GetAllParcelsToList();
            GroupByPrio.IsEnabled = true;
            GroupByPrio.Opacity = 1;

        }


        private void GroupByClick_1(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(parcelListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("SenderName");
            view.GroupDescriptions.Add(groupDescription);
            GroupBySender.IsEnabled = false;
            GroupBySender.Opacity = 0.5;
        }

        private void GroupByPrioClick_1(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(parcelListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Priority");
            view.GroupDescriptions.Add(groupDescription);
            GroupByPrio.IsEnabled = false;
            GroupByPrio.Opacity = 0.5;

        }

        private void GroupByTargetClick_1(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(parcelListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("TargetName");
            view.GroupDescriptions.Add(groupDescription);
            GroupByTarget.IsEnabled = false;
            GroupByTarget.Opacity = 0.5;

        }
    }
}
