﻿using System;
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
        IEnumerable<BO.Parcel> pp;
        public ParcelWindow(BlApi.IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            pp  = _bl.GetAllParcels();
            parcelListView.DataContext =pp;
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
                parcelListView.ItemsSource = bl.GetAllParcels();
            else
                parcelListView.ItemsSource = bl.GetParcelBy(p);

        }

        private void parcelListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Parcel p = parcelListView.SelectedItem as BO.Parcel;
            if (p != null)
                new ParcelShow(p, bl, "").Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            nextid = pp.Last().Id + 1;
            new ParcelShow(bl, nextid).Show();
        }

        private void PrioritySelector_LostTouchCapture(object sender, TouchEventArgs e)
        {

        }

        private void PrioritySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Predicate<BO.Parcel> p;
            if (WeightSelector.SelectedIndex != -1) //if the weight selector is selected as well
                p = s => s.Weight == (BO.WeightCategories)WeightSelector.SelectedIndex && s.Priority == (BO.Priorities)PrioritySelector.SelectedIndex;
            else
                p = s => s.Priority == (BO.Priorities)PrioritySelector.SelectedIndex;
            parcelListView.ItemsSource = bl.GetParcelBy(p); //fill the list view
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Predicate<BO.Parcel> p;
            if (PrioritySelector.SelectedIndex != -1) //if the status selector is selected as well
                p = s => s.Weight == (BO.WeightCategories)WeightSelector.SelectedIndex && s.Priority == (BO.Priorities)PrioritySelector.SelectedIndex;
            else
                p = s => s.Weight == (BO.WeightCategories)WeightSelector.SelectedIndex;
            parcelListView.ItemsSource = bl.GetParcelBy(p);
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            parcelListView.ItemsSource = bl.GetAllParcels();
        }
    }
}
