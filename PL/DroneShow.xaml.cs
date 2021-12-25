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
    /// Interaction logic for DroneShow.xaml
    /// </summary>
    public partial class DroneShow : Window
    {
        IBL.IBL bl;
        IBL.BO.Drone d;
        IEnumerable<IBL.BO.Station> s;
      //  Predicate<IBL.BO.Station> p;
        bool updateflag = false;

        
        public DroneShow(IBL.IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            //textBox1.Text =" ";
           // p = l => l.ChargeSlots > 0;
             s = bl.GetStationsforNoEmpty();
           
            AddUpdateButton.Content = "Add";
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

            stationCombo.Visibility = Visibility.Hidden;
            stationLable.Visibility = Visibility.Hidden;
            ChargingButton.Visibility = Visibility.Hidden;
            DeliveryButton.Visibility = Visibility.Hidden;
            StatusSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
        
        }

        public DroneShow(IBL.IBL _bl, IBL.BO.Drone _d)
        {
            InitializeComponent();
            bl = _bl;
            d = _d;
            AddUpdateButton.Content = "Update";
            if (_d.Status == (IBL.BO.DroneStatuses)2) 
            {
                ChargingButton.Content = "Discharge Drone";
                DeliveryButton.IsEnabled = false;

            }
            else
                ChargingButton.Content = "Charge Drone";
            if (_d.Status == (IBL.BO.DroneStatuses)0)
                DeliveryButton.Content = "Pick a Parcel";
            else
                DeliveryButton.Content = "Next delivery step";

            ChargingButton.Visibility = Visibility.Visible;
            DeliveryButton.Visibility = Visibility.Visible;


            idtextbox.Text =d.Id.ToString();
            modelTextbox.Text = d.Model.ToString();
            LattextBox.Text = d.CurrentLocation.Lattitude.ToString();
            Lontextbox.Text = d.CurrentLocation.Longitude.ToString();
            BatteryTextBox.Text = d.Battery.ToString();
           
            idtextbox.IsEnabled = false;
            LattextBox.IsEnabled = false;
            Lontextbox.IsEnabled = false;
            BatteryTextBox.IsEnabled = false;
            stationCombo.IsEnabled = false;

            stationCombo.Visibility = Visibility.Hidden;
            stationLable.Visibility = Visibility.Hidden;
            //stationCombo.Visibility = Visibility.Hidden;
            //stationLable.Visibility = Visibility.Hidden;

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


            // textBox1.Text = " ";
            StatusSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AddUpdateButton.Content.Equals("Add"))
                {
                    string c = BatteryTextBox.Text;
                    if (double.Parse(c) > 100 || double.Parse(c) < 0)
                    {
                       throw new IBL.BO.BadInputException("battery should be between 0-100");
                       c = "100";
                       idtextbox.Text = c;
                    }

                    int stid = 0;
                    IBL.BO.Drone db = new IBL.BO.Drone()
                    {
                        Id = Convert.ToInt32(idtextbox.Text.ToString()),
                        Battery = double.Parse(BatteryTextBox.Text.ToString()),
                        Model = modelTextbox.Text.ToString(),
                        MaxWeight = (IBL.BO.WeightCategories)Convert.ToInt32(WeightSelector.SelectedIndex.ToString()),
                        Status = (IBL.BO.DroneStatuses)Convert.ToInt32(StatusSelector.SelectedIndex.ToString()),
                        CurrentParcel = null,
                        CurrentLocation = null
                    };
                    if (stationCombo.SelectedIndex != -1)
                    {
                        stid = s.ElementAt(stationCombo.SelectedIndex).Id;
                    }
                    else
                        db.CurrentLocation = new IBL.BO.Location() { Lattitude = double.Parse(LattextBox.Text.ToString()), Longitude = double.Parse(Lontextbox.Text.ToString()) };
                    bl.AddDrone(db, stid);
                    MessageBox.Show("Successfully completed the task.");
                    this.Close(); ;//station id, how???
                }
                else
                {
                    if (updateflag == false)
                    {
                        ChargingButton.Visibility = Visibility.Hidden;
                        DeliveryButton.Visibility = Visibility.Hidden;
                        idtextbox.Visibility = Visibility.Visible;
                        idLable.Visibility = Visibility.Visible;
                        modelTextbox.Visibility = Visibility.Visible;
                        modelLable.Visibility = Visibility.Visible;
                        updateflag = true;
                        //idtextbox.Text = d.Id.ToString();
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
                    else
                    {
                        updateflag = false;
                        bl.UpdateDrone(d.Id, modelTextbox.Text);
                        MessageBox.Show("Successfully completed the task.");
                        this.Close();
                    }
                    //  idtextbox.ena
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
                if (d.Status == IBL.BO.DroneStatuses.Available)
                    bl.LinkDroneToParcel(d.Id);
                else if (d.Status == IBL.BO.DroneStatuses.Delivery)
                {
                    IBL.BO.Parcel p = bl.GetParcel(d.CurrentParcel.Id);
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
                if (d.Status == IBL.BO.DroneStatuses.Available)
                    bl.DroneToCharge(d.Id);
                else if (d.Status == IBL.BO.DroneStatuses.Maintenance)
                    bl.EndCharging(d.Id);
                MessageBox.Show("Successfully completed the task.");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }

            this.Close(); 
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (d.Status == IBL.BO.DroneStatuses.Available)
                    bl.LinkDroneToParcel(d.Id);
                else if (d.Status == IBL.BO.DroneStatuses.Delivery)
                {
                    IBL.BO.Parcel p = bl.GetParcel(d.CurrentParcel.Id);
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
           
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(StatusSelector.SelectedIndex==2)
            {
                stationCombo.Visibility = Visibility.Visible;
                stationLable.Visibility = Visibility.Visible;
                stationCombo.ItemsSource = s;
                latlable.Visibility = Visibility.Hidden;
                label7.Visibility = Visibility.Hidden;
                LattextBox.Visibility = Visibility.Hidden;
                Lontextbox.Visibility = Visibility.Hidden;
                
            }
            else
            {
                LattextBox.Visibility = Visibility.Visible;
                Lontextbox.Visibility = Visibility.Visible;
                latlable.Visibility = Visibility.Visible;
                label7.Visibility = Visibility.Visible;
                stationCombo.Visibility = Visibility.Hidden;
                stationLable.Visibility = Visibility.Hidden;
            }
        }
        public bool IsnumberChar(string c)
        {
            for (int i = 0; i < c.Length; i++)
            {
                if ((c[i] < '0' || c[i] > '9') && (c[i] != '\b'))
                    return false;

            }
            return true;
        }
        public bool IsnumberCharLoc(string c)
        {
            for (int i = 0; i < c.Length; i++)
            {
                if ((c[i] < '0' || c[i] > '9') && (c[i] != '\b') && (c[i] != '.'))
                    return false;

            }
            return true;
        }

        private void idtextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string c=idtextbox.Text;
            try
            {
                if (!IsnumberChar(idtextbox.Text.ToString()))
                    throw new IBL.BO.BadInputException(c,"ID can include only numbers");
               
            }
            catch (Exception ex)
            {
                c= c.Remove(c.Length - 1);
                idtextbox.Text = c;
                MessageBox.Show(ex.ToString());
            }

        }

        private void idtextbox_TextInput(object sender, TextCompositionEventArgs e)
        {
           

        }

        private void BatteryTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string c = BatteryTextBox.Text;
            try
            {
                if (!IsnumberCharLoc(BatteryTextBox.Text.ToString()))
                    throw new IBL.BO.BadInputException(c,"battery can include only numbers");
                if(c.Length>0 &&(int.Parse(c)>100 || int.Parse(c) < 0))
                    throw new IBL.BO.BadInputException("battery should be between 0-100");
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
            try
            {
                if (!IsnumberCharLoc(LattextBox.Text.ToString()))
                    throw new IBL.BO.BadInputException(c,"location can include only numbers");
                
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
            try
            {
                if (!IsnumberCharLoc(Lontextbox.Text.ToString()))
                    throw new IBL.BO.BadInputException(c, "location can include only numbers");

            }
            catch (Exception ex)
            {
                c = "0";
                Lontextbox.Text = c;
                MessageBox.Show(ex.ToString());
            }
        }

        private void button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{

        //}
    }
}
