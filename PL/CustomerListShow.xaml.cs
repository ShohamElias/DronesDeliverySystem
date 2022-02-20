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
    /// Interaction logic for CustomerListShow.xaml
    /// </summary>
    public partial class CustomerListShow : Window
    {
        BlApi.IBL bl;
        bool flag = true;
        public CustomerListShow(BlApi.IBL _bl)//builder
        {
            InitializeComponent();
            textBox.Text = "";
            CustomerListView.DataContext = from item in _bl.GetAllCustomers()//getting values for the listview
                                           select _bl.GetCustomerToList(item.Id);
            bl = _bl;
            flag = false;
           
        }

        
        
        private void CustomerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)//if clicked on a row
        {
            BO.CustomerToList c = CustomerListView.SelectedItem as BO.CustomerToList;
            if (c != null)
                new CustomerShowWindow(bl.GetCustomer(c.Id), bl, "Update").Show();//opening the customer window w its values
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            new CustomerShowWindow(bl).Show();//add option window
        }

        private void Window_Activated_1(object sender, EventArgs e)//back to window - loading again
        {
            CustomerListView.ItemsSource = from item in bl.GetAllCustomers()
                                           select bl.GetCustomerToList(item.Id);

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)//searchbox
        {
            if (flag)//if the window is not intialize yet
                return;
            string txtOrig = textBox.Text;
            string upper = txtOrig.ToUpper();
            string lower = txtOrig.ToLower();
            Predicate<BO.Customer> p;
            p = s => s.Name.StartsWith(lower) || s.Name.StartsWith(upper) || s.Name.Contains(txtOrig);
            CustomerListView.ItemsSource = from item in bl.GetCustomerBy(p)//getting all the customers that contains this letter
                                           select bl.GetCustomerToList(item.Id);

        }
    }
}
