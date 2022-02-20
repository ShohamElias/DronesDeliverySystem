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
        Validation valid= new Validation();

        public CustomerShowWindow(BO.Customer c, BlApi.IBL _bl, string typeWindow)//SHOW WINDOW
        {
            InitializeComponent();
            bl = _bl;
            closingwin = true;
            customerGrid.DataContext = c;
            Predicate<BO.Parcel> p;
            PasswordtextBox.Text = "";
            p = s => s.Sender.Id == c.Id;//predicate, getting parcels sent by this customer
            SentList.ItemsSource = bl.GetParcelBy(p);
            p = s => s.Target.Id == c.Id && s.Delivered!=null;//getting all the parcels delivered to this customer
            ReceivedList.ItemsSource = bl.GetParcelBy(p);
            addUpdateButton.Content = typeWindow;
            if (typeWindow == "User")//if user then can apdate checkbox
            {
                addUpdateButton.Content = "Update";
                SendButton.Visibility = Visibility.Visible;
            }
            PasswordtextBox.Visibility = Visibility.Hidden;
             passwordlabel.Visibility = Visibility.Hidden;
            
            if (typeWindow == "show")
            {
                

                addUpdateButton.Visibility = Visibility.Collapsed;
                idTextBox.IsReadOnly = true;
                LatitudeTextBox.IsReadOnly = true;
                LongtitudeTextBox.IsReadOnly = true;
                phoneTextBox.IsReadOnly = true;
                nameTextBox.IsReadOnly = true;
                SendButton.Visibility = Visibility.Collapsed;
            }

            
        }

        public CustomerShowWindow(BlApi.IBL _bl)//ADD WINDOW
        {
            InitializeComponent();
            bl = _bl;
            closingwin = true;
            addUpdateButton.Content = "Add";
            SendButton.Visibility = Visibility.Collapsed;
            idTextBox.Text = "";
            nameTextBox.Text = "";
            phoneTextBox.Text = "";
            LongtitudeTextBox.Text = "";
            LatitudeTextBox.Text = "";
            ReceivedList.Visibility = Visibility.Hidden;
            SentList.Visibility = Visibility.Hidden;
            SentParcels.Visibility = Visibility.Hidden;
            RecievedParcels.Visibility = Visibility.Hidden;
            this.Height = 350;
            this.Width = 418;
            addUpdateButton.Margin = new Thickness(295, 106, 0, 0);//relocation
            cancelButton.Margin = new Thickness(295, 142, 0, 0);

        }

        private void addUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (addUpdateButton.Content == "Add")//if we want to add
                {
                    BO.Customer cust = new BO.Customer();
                    if (idTextBox.Text == "" || nameTextBox.Text == "" || phoneTextBox.Text == "" || LongtitudeTextBox.Text == "" || LatitudeTextBox.Text == "")//checking all fields
                    {
                        MessageBox.Show("Please fill all the fields!");
                        return;
                    }
                    cust.Id = int.Parse(idTextBox.Text.ToString());
                    cust.Name = nameTextBox.Text.ToString();
                    cust.Phone = phoneTextBox.Text.ToString();
                    cust.password = PasswordtextBox.Text;
                    cust.CustLocation = new BO.Location() { Lattitude = double.Parse(LongtitudeTextBox.Text.ToString()), Longitude = double.Parse(LatitudeTextBox.Text.ToString()) };//creating
                    bl.AddCustomer(cust);
                }
                else if (addUpdateButton.Content == "Update")//update option
                {
                    bl.UpdateCustomer(int.Parse(idTextBox.Text), nameTextBox.Text, phoneTextBox.Text);
                }
                MessageBox.Show("Successfully completed the task.");
                closingwin = false;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void SentList_MouseDoubleClick(object sender, MouseButtonEventArgs e)//clicing on a parcel
        {
            BO.Parcel c = SentList.SelectedItem as BO.Parcel;
            if (c != null)
                new ParcelShow(c, bl, "sender").ShowDialog();
        }

        private void ReceivedList_MouseDoubleClick(object sender, MouseButtonEventArgs e)//clicking on a parcel
        {
            BO.Parcel c = ReceivedList.SelectedItem as BO.Parcel;
            if (c != null)
                new ParcelShow(c, bl, "target").ShowDialog();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = closingwin;//let close
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            closingwin = false;
            Close();
        }


        private void idTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string c = idTextBox.Text;
            try //validatoin for id
            {
                if (!valid.IsnumberChar(idTextBox.Text.ToString()))
                    throw new BO.BadInputException(c, "ID can include only numbers");

            }
            catch (Exception ex)
            {
                c = c.Remove(c.Length - 1);
                idTextBox.Text = c;
                MessageBox.Show(ex.ToString(), "Error Occurred", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LongtitudeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string c = LongtitudeTextBox.Text;
            try//validatoin for longtitude
            {
                if (!valid.IsnumberCharLoc(LongtitudeTextBox.Text.ToString()))
                    throw new BO.BadInputException(c, "location can include only numbers");
                if (Convert.ToDouble(c)>35 ||  Convert.ToDouble(c) < 30)
                    throw new BO.BadInputException(c, "location is out of range!");

            }
            catch (Exception ex)
            {
                c = "0";
                LongtitudeTextBox.Text = c;
                MessageBox.Show(ex.ToString(), "Error Occurred", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LatitudeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string c = LatitudeTextBox.Text;
            try //validatoin for lattitude
            {
                if (!valid.IsnumberCharLoc(LatitudeTextBox.Text.ToString()))
                    throw new BO.BadInputException(c, "location can include only numbers");
                if (Convert.ToDouble(c) > 35 || Convert.ToDouble(c) < 30)
                    throw new BO.BadInputException(c, "location is out of range!");

            }
            catch (Exception ex)
            {
                c = "0";
                LatitudeTextBox.Text = c;
                MessageBox.Show(ex.ToString(), "Error Occurred", MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)//sending a parcel fron this customer
        {
            new ParcelShow(bl, int.Parse(idTextBox.Text)).Show();
        }

        private void Window_Activated(object sender, EventArgs e)//reloading
        {
            if (addUpdateButton.Content != "Add")
            {
                Predicate<BO.Parcel> p;
                int c = int.Parse(idTextBox.Text.ToString());
                p = s => s.Sender.Id == c;
                SentList.ItemsSource = bl.GetParcelBy(p);
                p = s => s.Target.Id == c && s.Delivered != null;
                ReceivedList.ItemsSource = bl.GetParcelBy(p);
            }
           
        }

        private void Cbutton_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
