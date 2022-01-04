using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for CustomerShowWindow.xaml
    /// </summary>
    public partial class CustomerShowWindow : Window
    {
        BlApi.IBL bl;
        static bool closingwin = true;

        public CustomerShowWindow(BO.Customer c, BlApi.IBL _bl, string typeWindow)
        {
            InitializeComponent();
            bl = _bl;
            closingwin = true;
            customerGrid.DataContext = c;
            Predicate<BO.Parcel> p;
            p = s => s.Sender.Id == c.Id;
            SentList.ItemsSource = bl.GetParcelBy(p);
            p = s => s.Target.Id == c.Id && s.Delivered!=null;
            ReceivedList.ItemsSource = bl.GetParcelBy(p);
            addUpdateButton.Content = typeWindow;
            if (typeWindow == "show")
            {
                addUpdateButton.Visibility = Visibility.Collapsed;
                idTextBox.IsReadOnly = true;
                LatitudeTextBox.IsReadOnly = true;
                LongtitudeTextBox.IsReadOnly = true;
                phoneTextBox.IsReadOnly = true;
                nameTextBox.IsReadOnly = true;
            }

        }

        public CustomerShowWindow(BlApi.IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            closingwin = true;
            addUpdateButton.Content = "Add";
            ReceivedList.Visibility = Visibility.Hidden;
            SentList.Visibility = Visibility.Hidden;
            SentParcels.Visibility = Visibility.Hidden;
            RecievedParcels.Visibility = Visibility.Hidden;

        }

        private void addUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (addUpdateButton.Content == "Add")
            {
                BO.Customer cust= new BO.Customer();
                cust.Id = int.Parse(idTextBox.Text);
                cust.Name= nameTextBox.Text.ToString();
                cust.Phone = phoneTextBox.Text.ToString();
                cust.CustLocation = new BO.Location() { Lattitude = double.Parse(LongtitudeTextBox.Text.ToString()), Longitude = double.Parse(LatitudeTextBox.Text.ToString()) };
                bl.AddCustomer(cust);
            }
            else if (addUpdateButton.Content == "Update")
            {
                bl.UpdateCustomer(int.Parse(idTextBox.Text), nameTextBox.Text, phoneTextBox.Text);
            }
            MessageBox.Show("Successfully completed the task.");
            closingwin = false;
            this.Close();
        }

        private void SentList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Parcel c = SentList.SelectedItem as BO.Parcel;
            if (c != null)
                new ParcelShow(c, bl, "sender").ShowDialog();
        }

        private void ReceivedList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Parcel c = ReceivedList.SelectedItem as BO.Parcel;
            if (c != null)
                new ParcelShow(c, bl, "target").ShowDialog();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = closingwin;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            closingwin = false;
            Close();
        }

        public bool IsnumberChar(string c) //the func checks if the string is a number
        {
            for (int i = 0; i < c.Length; i++)
            {
                if ((c[i] < '0' || c[i] > '9') && (c[i] != '\b'))
                    return false;

            }
            return true;
        }
        public bool IsnumberCharLoc(string c) //the func checks if the string is a right input to location
        {
            for (int i = 0; i < c.Length; i++)
            {
                if ((c[i] < '0' || c[i] > '9') && (c[i] != '\b') && (c[i] != '.'))
                    return false;

            }
            return true;
        }

        private void idTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string c = idTextBox.Text;
            try //validatoin for id
            {
                if (!IsnumberChar(idTextBox.Text.ToString()))
                    throw new BO.BadInputException(c, "ID can include only numbers");

            }
            catch (Exception ex)
            {
                c = c.Remove(c.Length - 1);
                idTextBox.Text = c;
                MessageBox.Show(ex.ToString());
            }
        }

        private void LongtitudeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string c = LongtitudeTextBox.Text;
            try//validatoin for longtitude
            {
                if (!IsnumberCharLoc(LongtitudeTextBox.Text.ToString()))
                    throw new BO.BadInputException(c, "location can include only numbers");

            }
            catch (Exception ex)
            {
                c = "0";
                LongtitudeTextBox.Text = c;
                MessageBox.Show(ex.ToString());
            }
        }

        private void LatitudeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string c = LatitudeTextBox.Text;
            try //validatoin for lattitude
            {
                if (!IsnumberCharLoc(LatitudeTextBox.Text.ToString()))
                    throw new BO.BadInputException(c, "location can include only numbers");

            }
            catch (Exception ex)
            {
                c = "0";
                LatitudeTextBox.Text = c;
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
