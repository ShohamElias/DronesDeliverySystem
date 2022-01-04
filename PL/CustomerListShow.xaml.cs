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
    /// Interaction logic for CustomerListShow.xaml
    /// </summary>
    public partial class CustomerListShow : Window
    {
        BlApi.IBL bl;
        static bool flag = true;
        public CustomerListShow(BlApi.IBL _bl)
        {
            InitializeComponent();
            textBox.Text = "";
            CustomerListView.DataContext = _bl.GetAllCustomers();
            bl = _bl;
            flag = false;
        }

        
        
        private void CustomerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Customer c = CustomerListView.SelectedItem as BO.Customer;
            if (c != null)
                new CustomerShowWindow(c, bl, "Update").ShowDialog();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            new CustomerShowWindow(bl).Show();
        }

        private void Window_Activated_1(object sender, EventArgs e)
        {
            CustomerListView.ItemsSource = bl.GetAllCustomers();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (flag)
                return;
            string txtOrig = textBox.Text;
            string upper = txtOrig.ToUpper();
            string lower = txtOrig.ToLower();
            Predicate<BO.Customer> p;
            p = s => s.Name.StartsWith(lower) || s.Name.StartsWith(upper) || s.Name.Contains(txtOrig);
            CustomerListView.ItemsSource = bl.GetCustomerBy(p);
            
        }
    }
}