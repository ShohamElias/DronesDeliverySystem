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
    /// Interaction logic for DroneShow.xaml
    /// </summary>
    public partial class DroneShow : Window
    {
        BlApi.IBL bl;
        BO.Drone d;
        IEnumerable<BO.Station> s;
        bool updateflag = false;
        static bool closingwin = true;

        public DroneShow(BlApi.IBL _bl, BO.Drone _d,string s)
        {
            InitializeComponent();
            d = _d;
            AddUpdateButton.Visibility = Visibility.Hidden;
            ChargingButton.Visibility = Visibility.Hidden;
            DeliveryButton.Visibility = Visibility.Hidden;

            idtextbox.Text = d.Id.ToString();
            modelTextbox.Text = d.Model.ToString();
            LattextBox.Text = d.CurrentLocation.Lattitude.ToString();
            Lontextbox.Text = d.CurrentLocation.Longitude.ToString();
            BatteryTextBox.Text = Convert.ToInt64(d.Battery).ToString();
            modelTextbox.Text = d.Model;

            StatusSelector.ItemsSource = Enum.GetValues(typeof(BO.DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            StatusSelector.SelectedItem = d.Status;
            WeightSelector.SelectedItem = d.MaxWeight;

            idtextbox.IsReadOnly = true;         
            modelTextbox.IsReadOnly = true;
            LattextBox.IsReadOnly = true;
            Lontextbox.IsReadOnly = true;
            BatteryTextBox.IsReadOnly = true;
            WeightSelector.IsReadOnly = true;
            StatusSelector.IsReadOnly = true;
            modelTextbox.IsReadOnly = true;

            WeightSelector.IsEnabled = false;
            StatusSelector.IsEnabled = false;

        }
        public DroneShow(BlApi.IBL _bl) //window opens as "ADD" opstion
        {
            InitializeComponent();
            bl = _bl;
            closingwin = true;

            s = bl.GetStationsforNoEmpty();

            AddUpdateButton.Content = "Add"; //making every thing visible
            idtextbox.Text = "";
            modelTextbox.Text = "";
            LattextBox.Text = "";
            Lontextbox.Text = "";
            BatteryTextBox.Text = "";
            idtextbox.IsEnabled = true;
            LattextBox.IsEnabled = true;
            Lontextbox.IsEnabled = true;
            BatteryTextBox.IsEnabled = true;
            StatusSelector.IsEnabled = true;

            stationCombo.Visibility = Visibility.Hidden; //the option buttons are hidden
            stationLable.Visibility = Visibility.Hidden;
            ChargingButton.Visibility = Visibility.Hidden;
            DeliveryButton.Visibility = Visibility.Hidden;
            StatusSelector.ItemsSource = Enum.GetValues(typeof(BO.DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));

        }

        public DroneShow(BlApi.IBL _bl, BO.Drone _d) //window opens as "UPDATE" / option window
        {
            InitializeComponent();
            closingwin = true;
            bl = _bl;
            d = _d;
            AddUpdateButton.Content = "Update";
            if (_d.Status == (BO.DroneStatuses)2) //id the drone is currently charging
            {
                ChargingButton.Content = "Discharge Drone"; //change the charge/discharge button to discharge
                DeliveryButton.IsEnabled = false;//charging rn, cant delivere

            }
            else
                ChargingButton.Content = "Charge Drone";
            if (_d.Status == (BO.DroneStatuses)0)
                DeliveryButton.Content = "Pick a Parcel";
            else
                DeliveryButton.Content = "Next delivery step";

            ChargingButton.Visibility = Visibility.Visible; //hiding everything except the option buttons
            DeliveryButton.Visibility = Visibility.Visible;


            idtextbox.Text = d.Id.ToString();
            modelTextbox.Text = d.Model.ToString();
            LattextBox.Text = d.CurrentLocation.Lattitude.ToString();
            Lontextbox.Text = d.CurrentLocation.Longitude.ToString();
            BatteryTextBox.Text = Convert.ToInt64(d.Battery).ToString();

            idtextbox.IsEnabled = false;
            LattextBox.IsEnabled = false;
            Lontextbox.IsEnabled = false;
            BatteryTextBox.IsEnabled = false;
            stationCombo.IsEnabled = false;

            stationCombo.Visibility = Visibility.Hidden;
            stationLable.Visibility = Visibility.Hidden;

            idtextbox.Visibility = Visibility.Hidden;
            modelTextbox.Visibility = Visibility.Hidden;
            LattextBox.Visibility = Visibility.Hidden;
            Lontextbox.Visibility = Visibility.Hidden;
            BatteryTextBox.Visibility = Visibility.Hidden;
            StatusSelector.Visibility = Visibility.Hidden;
            WeightSelector.Visibility = Visibility.Hidden;
            idLable.Visibility = Visibility.Hidden;
            modelLable.Visibility = Visibility.Hidden;
            statuslablle.Visibility = Visibility.Hidden;
            Batterylable.Visibility = Visibility.Hidden;
            latlable.Visibility = Visibility.Hidden;

            WeightLable.Visibility = Visibility.Hidden;



            StatusSelector.ItemsSource = Enum.GetValues(typeof(BO.DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            StatusSelector.SelectedItem = d.Status;
            WeightSelector.SelectedItem = d.MaxWeight;

            StatusSelector.IsEnabled = false;
            WeightSelector.IsEnabled = false;

            stationCombo.Visibility = Visibility.Hidden;
            stationLable.Visibility = Visibility.Hidden;

            LattextBox.Visibility = Visibility.Hidden;
            Lontextbox.Visibility = Visibility.Hidden;
            label7.Visibility = Visibility.Hidden;
            latlable.Visibility = Visibility.Hidden;

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
        private void button_Click(object sender, RoutedEventArgs e) //add/update button click
        {
            try
            {
                if (AddUpdateButton.Content.Equals("Add")) //if we are adding a new drone
                {
                    string c = BatteryTextBox.Text;
                    if (double.Parse(c) > 100 || double.Parse(c) < 0) //validation for the battery
                    {
                        throw new BO.BadInputException("battery should be between 0-100");
                        c = "100";
                        idtextbox.Text = c;
                    }

                    int stid = 0;
                    BO.Drone db = new BO.Drone() //creating a new drone bty the filled text boxes and comboboxes
                    {
                        Id = Convert.ToInt32(idtextbox.Text.ToString()),
                        Battery = double.Parse(BatteryTextBox.Text.ToString()),
                        Model = modelTextbox.Text.ToString(),
                        MaxWeight = (BO.WeightCategories)Convert.ToInt32(WeightSelector.SelectedIndex.ToString()),
                        Status = (BO.DroneStatuses)Convert.ToInt32(StatusSelector.SelectedIndex.ToString()),
                        CurrentParcel = null,
                        CurrentLocation = null
                    };
                    if (stationCombo.SelectedIndex != -1) //if we selected a station
                    {
                        stid = s.ElementAt(stationCombo.SelectedIndex).Id;
                    }
                    else
                        db.CurrentLocation = new BO.Location() { Lattitude = double.Parse(LattextBox.Text.ToString()), Longitude = double.Parse(Lontextbox.Text.ToString()) };
                    bl.AddDrone(db, stid);
                    MessageBox.Show("Successfully completed the task.");
                    this.Close();
                }
                else
                {
                    if (updateflag == false) //if we pressed the update button in order to change the name (before changing it) 
                    { //showing all the drone values
                        ChargingButton.Visibility = Visibility.Hidden;
                        DeliveryButton.Visibility = Visibility.Hidden;
                        idtextbox.Visibility = Visibility.Visible;
                        idLable.Visibility = Visibility.Visible;
                        modelTextbox.Visibility = Visibility.Visible;
                        modelLable.Visibility = Visibility.Visible;
                        updateflag = true;

                        LattextBox.Visibility = Visibility.Visible;
                        Lontextbox.Visibility = Visibility.Visible;
                        BatteryTextBox.Visibility = Visibility.Visible;
                        StatusSelector.Visibility = Visibility.Visible;
                        WeightSelector.Visibility = Visibility.Visible;
                        idLable.Visibility = Visibility.Visible;
                        modelLable.Visibility = Visibility.Visible;
                        statuslablle.Visibility = Visibility.Visible;
                        Batterylable.Visibility = Visibility.Visible;
                        latlable.Visibility = Visibility.Visible;
                        label7.Visibility = Visibility.Visible;
                        WeightLable.Visibility = Visibility.Visible;
                    }
                    else //we changed the name and now we wanr an actual update
                    {
                        updateflag = false;
                        bl.UpdateDrone(d.Id, modelTextbox.Text);
                        MessageBox.Show("Successfully completed the task.");
                        this.Close();
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void DeliveryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //the next step of delivery is decided y the status of the drone or the parcel in the drone
                if (d.Status == BO.DroneStatuses.Available) 
                    bl.LinkDroneToParcel(d.Id);
                else if (d.Status == BO.DroneStatuses.Delivery)
                {
                    BO.Parcel p = bl.GetParcel(d.CurrentParcel.Id);
                    if (p.PickedUp == null)
                        bl.PickParcel(d.Id);
                    else if (p.Delivered == null)
                        bl.DeliveringParcel(d.Id);
                }
                MessageBox.Show("Successfully completed the task.");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            this.Close();
        }

        private void ChargingButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (d.Status == BO.DroneStatuses.Available) //if the drone is available
                    bl.DroneToCharge(d.Id); //charge it
                else if (d.Status == BO.DroneStatuses.Maintenance) //if the drone is charging
                    bl.EndCharging(d.Id); //end it
                else if (d.Status == BO.DroneStatuses.Delivery) //drones on delivey cand be charged
                    throw new BO.WrongDroneStatException(d.Id, "cant charge a drone on delivery");
                MessageBox.Show("Successfully completed the task.");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }

            this.Close();
        }

        

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusSelector.SelectedIndex == 2) //if we selectes maintance
            { //hide the location texts boxes and make the station combobox visible
                stationCombo.Visibility = Visibility.Visible;
                stationLable.Visibility = Visibility.Visible;
                stationCombo.ItemsSource = s;
                latlable.Visibility = Visibility.Hidden;
                label7.Visibility = Visibility.Hidden;
                LattextBox.Visibility = Visibility.Hidden;
                Lontextbox.Visibility = Visibility.Hidden;

            }
            else
            { //exactly the opposit
                LattextBox.Visibility = Visibility.Visible;
                Lontextbox.Visibility = Visibility.Visible;
                latlable.Visibility = Visibility.Visible;
                label7.Visibility = Visibility.Visible;
                stationCombo.Visibility = Visibility.Hidden;
                stationLable.Visibility = Visibility.Hidden;
            }
        }
       

        private void idtextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string c = idtextbox.Text;
            try //validatoin for id
            {
                if (!IsnumberChar(idtextbox.Text.ToString()))
                    throw new BO.BadInputException(c, "ID can include only numbers");

            }
            catch (Exception ex)
            {
                c = c.Remove(c.Length - 1);
                idtextbox.Text = c;
                MessageBox.Show(ex.ToString());
            }

        }

        private void BatteryTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string c = BatteryTextBox.Text;
            try //validatoin for battery
            {
                if (!IsnumberCharLoc(BatteryTextBox.Text.ToString()))
                    throw new BO.BadInputException(c, "battery can include only numbers");
                if (c.Length > 0 && (int.Parse(c) > 100 || int.Parse(c) < 0))
                    throw new BO.BadInputException("battery should be between 0-100");
            }
            catch (Exception ex)
            {
                c = "0";
                BatteryTextBox.Text = c;
                MessageBox.Show(ex.ToString());
            }
        }

        private void LattextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string c = LattextBox.Text;
            try //validatoin for lattitude
            {
                if (!IsnumberCharLoc(LattextBox.Text.ToString()))
                    throw new BO.BadInputException(c, "location can include only numbers");

            }
            catch (Exception ex)
            {
                c = "0";
                LattextBox.Text = c;
                MessageBox.Show(ex.ToString());
            }
        }

        private void Lontextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string c = Lontextbox.Text;
            try//validatoin for longtitude
            {
                if (!IsnumberCharLoc(Lontextbox.Text.ToString()))
                    throw new BO.BadInputException(c, "location can include only numbers");

            }
            catch (Exception ex)
            {
                c = "0";
                Lontextbox.Text = c;
                MessageBox.Show(ex.ToString());
            }
        }
       

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = closingwin;
        }
        private void button_Click_3(object sender, RoutedEventArgs e)
        {
            closingwin = false;
            WeightSelector.IsEnabled = true;
            StatusSelector.IsEnabled = true;
            this.Close();
        }

        
    }
}
