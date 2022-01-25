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
    /// Interaction logic for StationShoWindow.xaml
    /// </summary>
    public partial class StationShoWindow : Window
    {
        BlApi.IBL bl;
        bool updateflag;
        BO.Station ss;
        Validation valid = new Validation();
        public StationShoWindow(BlApi.IBL _bl,BO.Station s)
        {
            InitializeComponent();
            stationGrid.DataContext = s;
            droneChargeListView.DataContext = s.DronesinCharge;
            bl = _bl;
            updateflag = false;
            stationGrid.IsEnabled = false;
            addupdatebutton.Content = "Update";
            ss = s;
            droneChargeListView.Visibility = Visibility.Visible;
        }
        public StationShoWindow(BlApi.IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            addupdatebutton.Content = "Add";
            stationGrid.IsEnabled = true;
            droneChargeListView.Visibility = Visibility.Hidden;
        }

        private void droneChargeListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.DroneCharge dc = droneChargeListView.SelectedItem as BO.DroneCharge;
            if (dc != null)
                new DroneShow(bl, bl.GetDrone(dc.DroneId), " ").Show();
        }

        private void addupdatebutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (addupdatebutton.Content == "Update")
                {
                    if (updateflag)
                    {
                        bl.Updatestation(int.Parse(idTextBox.Text), nameTextBox.Text, int.Parse(ChargeSlots.Text));
                        this.Close();
                    }
                    else
                    {
                        updateflag = true;
                        stationGrid.IsEnabled = true;
                        idTextBox.IsEnabled = false;
                        droneChargeListView.IsEnabled = true;
                    }
                }
                else
                {
                    if (idTextBox.Text == "" || nameTextBox.Text == "" || ChargeSlots.Text == "" || LongtitudeTextBox.Text == "" || LatitudeTextBox.Text=="" )
                    {
                        MessageBox.Show("Please fill all the fields!" ,"Error Occurred", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    BO.Station s = new BO.Station()
                    {
                        Id = int.Parse(idTextBox.Text.ToString()),
                        Name = nameTextBox.Text.ToString(),
                        ChargeSlots = int.Parse(ChargeSlots.Text.ToString()),
                        StationLocation = new BO.Location() { Lattitude = double.Parse(LatitudeTextBox.Text.ToString()), Longitude = double.Parse(LongtitudeTextBox.Text.ToString()) }
                    };
                    bl.AddStation(s);
                    this.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if (droneChargeListView.Visibility == Visibility.Visible)
            {
                ss = bl.GetStation(ss.Id);
                droneChargeListView.DataContext =ss.DronesinCharge;
                
            }
        }

        private void droneChargeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        
        private void LongtitudeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string c = LongtitudeTextBox.Text;
            try//validatoin for longtitude
            {
                if (!valid.IsnumberCharLoc(LongtitudeTextBox.Text.ToString()))
                    throw new BO.BadInputException(c, "location can include only numbers");

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

            }
            catch (Exception ex)
            {
                c = "0";
                LatitudeTextBox.Text = c;
                MessageBox.Show(ex.ToString(), "Error Occurred", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private void ChargeSlots_TextChanged(object sender, TextChangedEventArgs e)
        {
            string c = ChargeSlots.Text;
            try //validatoin for id
            {
                if (!valid.IsnumberChar(ChargeSlots.Text.ToString()))
                    throw new BO.BadInputException(c, "Charge Slots can be only a number");

            }
            catch (Exception ex)
            {
                c = "0";
                ChargeSlots.Text = c;
                MessageBox.Show(ex.ToString(), "Error Occurred", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
