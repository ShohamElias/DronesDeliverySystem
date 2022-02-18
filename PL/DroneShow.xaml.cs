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
        Validation valid= new Validation();
        bool updateflag = false;
        static bool closingwin = true;
        string type="f";
        BackgroundWorker worker;

        public DroneShow(BlApi.IBL _bl, BO.Drone _d,string s)//view only! option window
        {
            InitializeComponent();
            d = _d;
            bl = _bl;
            AddUpdateButton.Visibility = Visibility.Hidden;
            ChargingButton.Visibility = Visibility.Hidden;
            DeliveryButton.Visibility = Visibility.Hidden;
            idtextbox.Text = d.Id.ToString();
            modelTextbox.Text = d.Model.ToString();
            latitudeLabel3.Content = d.CurrentLocation.Lattitude.ToString();
            longitudeLabel2.Content = d.CurrentLocation.Longitude.ToString();
            batteryPrecentage.Content = Convert.ToInt64(d.Battery).ToString()+"%";
            modelTextbox.Text = d.Model;
            
            StatusSelector.ItemsSource = Enum.GetValues(typeof(BO.DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            StatusSelector.SelectedItem = d.Status;
            WeightSelector.SelectedItem = d.MaxWeight;
            stationCombo.Visibility = Visibility.Hidden;
            stationLable.Visibility = Visibility.Hidden;
            idtextbox.IsReadOnly = true;         
            modelTextbox.IsReadOnly = true;
            HideAndShow();
            if ((int)d.Status == 0)
            {
                doingLabel.Content = "";
            }
            else if ((int)d.Status == 1)
            {
                doingLabel.Content = "Delivering Parcel:";
                ppp.Content = d.CurrentParcel.Id;
            }
            else
                ppp.Content = bl.GetDroneChargeStation(d.Id);
            modelTextbox.IsReadOnly = true;
            WeightSelector.IsEnabled = false;
            StatusSelector.IsEnabled = false;
        }
 
        public DroneShow(BlApi.IBL _bl) //window opens as "ADD" opstion
        {
            InitializeComponent();
            bl = _bl;
            closingwin = true;
            this.Height = 400;
            AddUpdateButton.Margin =new Thickness(218, 267, 0, 0);
            AddUpdateButton.Width *= 2;
            s = bl.GetStationsforNoEmpty();
            type = "Add";
            AddUpdateButton.Content = "Add"; //making every thing visible
            idtextbox.Text = "";
            modelTextbox.Text = "";
            LattextBox.Text = "";
            Lontextbox.Text = "";
            BatteryTextBox.Text = "";
            RBattery.Visibility = Visibility.Collapsed;
            batteryPrecentage.Visibility = Visibility.Collapsed;
            
            //the option buttons are hidden
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
            ppp.Content = d.CurrentParcel.Id;
            if (_d.Status == (BO.DroneStatuses)2) //id the drone is currently charging
            {
                ppp.Content = bl.GetDroneChargeStation(d.Id);
                ChargingButton.Content = "Discharge Drone"; //change the charge/discharge button to discharge
                DeliveryButton.IsEnabled = false;//charging rn, cant delivere
                DeliveryButton.Opacity = 0.5;
            }
            else
            {
                ChargingButton.Content = "Charge Drone";
            }
            if (_d.Status == (BO.DroneStatuses)0)
            {
                ppp.Content = "FREE";
                doingLabel.Content = "";
                DeliveryButton.Content = "Pick a Parcel";
            }
            else if (_d.Status == (BO.DroneStatuses)1)
            {
                doingLabel.Content = "Delivering Parcel:";
                DeliveryButton.Content = "Next delivery step";
            }

            ChargingButton.Visibility = Visibility.Visible; //hiding everything except the option buttons
            DeliveryButton.Visibility = Visibility.Visible;
            stationCombo.Visibility = Visibility.Hidden;
            stationLable.Visibility = Visibility.Hidden;
            idtextbox.Text = d.Id.ToString();
            modelTextbox.Text = d.Model.ToString();
            latitudeLabel3.Content = LocationString.ToStringLoc(d.CurrentLocation.Lattitude);
            longitudeLabel2.Content = LocationString.ToStringLoc(d.CurrentLocation.Longitude);
            batteryPrecentage.Content = Convert.ToInt64(d.Battery).ToString()+"%";

            idtextbox.IsEnabled = false;
            modelTextbox.IsEnabled = false;

            StatusSelector.ItemsSource = Enum.GetValues(typeof(BO.DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            StatusSelector.SelectedItem = d.Status;
            WeightSelector.SelectedItem = d.MaxWeight;
            StatusSelector.IsEnabled = false;
            WeightSelector.IsEnabled = false;
            HideAndShow();

            simulatorButton.Visibility = Visibility.Visible;//simulator is available
        }



        private void button_Click(object sender, RoutedEventArgs e) //add/update button click
        {
            try
            {
                if (AddUpdateButton.Content.Equals("Add")) //if we are adding a new drone
                {
                    if (idtextbox.Text == "" || modelTextbox.Text == "" || BatteryTextBox.Text == "" || StatusSelector.SelectedIndex == -1 || WeightSelector.SelectedIndex == -1)
                    {
                        MessageBox.Show("Please fill all the fields!");
                        return;
                    }                  

                    int stid = 0;
                    BO.Drone db = new BO.Drone() //creating a new drone by the filled text boxes and comboboxes
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
                    closingwin = false;
                    this.Close();
                }
                else
                {
                    if (updateflag == false) //if we pressed the update button in order to change the name (before changing it) 
                    { //showing all the drone values
                        
                        updateflag = true;
                        idtextbox.IsEnabled = true;
                        modelTextbox.IsEnabled = true;
                        
                    }
                    else //we changed the name and now we want an actual update
                    {
                        updateflag = false;
                        bl.UpdateDrone(d.Id, modelTextbox.Text);
                        MessageBox.Show("Successfully completed the task.");
                        closingwin = false;
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
            closingwin = false;
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
            closingwin = false;
            this.Close();
        }

        

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((StatusSelector.SelectedIndex == 2)&& type=="Add") //if we selectes maintance
            { 
                //hide the location texts boxes and make the station combobox visible
                stationCombo.Visibility = Visibility.Visible;
                stationLable.Visibility = Visibility.Visible;
                stationCombo.ItemsSource = s;
                latlable.Visibility = Visibility.Hidden;
                lable7.Visibility = Visibility.Hidden;
                LattextBox.Visibility = Visibility.Hidden;
                Lontextbox.Visibility = Visibility.Hidden;

            }
            else if (CancelSimBtn.Visibility != Visibility.Visible)
            { //exactly the opposit
                LattextBox.Visibility = Visibility.Visible;
                Lontextbox.Visibility = Visibility.Visible;
                latlable.Visibility = Visibility.Visible;
                lable7.Visibility = Visibility.Visible;
                
            }
        }

        private void idtextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string c = idtextbox.Text;
            try //validatoin for id
            {
                if (!valid.IsnumberChar(idtextbox.Text.ToString()))
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
                if (!valid.IsnumberCharLoc(BatteryTextBox.Text.ToString()))
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
            //string c = LattextBox.Text;
            //try //validatoin for lattitude
            //{
            //    if (!valid.IsnumberCharLoc(LattextBox.Text.ToString()))
            //        throw new BO.BadInputException(c, "location can include only numbers");

            //}
            //catch (Exception ex)
            //{
            //    c = "0";
            //    LattextBox.Text = c;
            //    MessageBox.Show(ex.ToString());
            //}
        }

        private void Lontextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //string c = Lontextbox.Text;
            //try//validatoin for longtitude
            //{
            //    if (!valid.IsnumberCharLoc(Lontextbox.Text.ToString()))
            //        throw new BO.BadInputException(c, "location can include only numbers");
            //}
            //catch (Exception ex)
            //{
            //    c = "0";
            //    Lontextbox.Text = c;
            //    MessageBox.Show(ex.ToString());
            //}
        }
       

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = closingwin;
        }

        private void cancelButton(object sender, RoutedEventArgs e)
        {
            if (!simulatorButton.IsEnabled )//If we came by the cancel button and the simulator hasant stopped yet
            {
                worker.CancelAsync();
                return;
            }
            closingwin = false;
            WeightSelector.IsEnabled = true;
            StatusSelector.IsEnabled = true;
            this.Close();
        }
    
        private void simulator()
        {
             worker = new();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

        }
        public bool IsTimeRun()
        {
            return worker.CancellationPending;
        }
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            CancelSimBtn.Visibility = Visibility.Collapsed;
            simulatorButton.Visibility = Visibility.Visible;
            simulatorButton.IsEnabled = true;
            //ALWAYS STOPS WHEN FREE:
            ChargingButton.IsEnabled = true;
            DeliveryButton.IsEnabled = true;
            AddUpdateButton.IsEnabled = true;
            if (Cursor != Cursors.Wait)//canceld bc of cancel button => wanting to close window
            {
                cancelButton(sender, null);

            }//canceled bc of stop simulation button
            Cursor = Cursors.Arrow;

        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            BO.Drone dI = bl.GetDrone(d.Id);
            idtextbox.Text = dI.Id.ToString();
            modelTextbox.Text = dI.Model.ToString();
            latitudeLabel3.Content = LocationString.ToStringLoc(dI.CurrentLocation.Lattitude);
            longitudeLabel2.Content = LocationString.ToStringLoc(dI.CurrentLocation.Longitude);
            batteryPrecentage.Content = Convert.ToInt64(dI.Battery).ToString()+"%";
            modelTextbox.Text = dI.Model;
            doingLabel.Content = "";
            StatusSelector.SelectedItem = dI.Status;
            WeightSelector.SelectedItem = dI.MaxWeight;

            if (dI.Status == BO.DroneStatuses.Delivery)
            {
                ppp.Content = dI.CurrentParcel.Id.ToString();
                doingLabel.Content = "Delivering Parcel:";
            }
            else if (dI.Status == BO.DroneStatuses.Maintenance)
            {
                ppp.Content = bl.GetDroneChargeStation(dI.Id);
                doingLabel.Content = "Charging at Station:";

            }
            else
                ppp.Content = "FREE";

        }

        public void ReportProgressInSimulator()
        {
            worker.ReportProgress(0);
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            bl.simulator(d.Id, ReportProgressInSimulator, IsTimeRun);
        }

        private void simulatorButton_Click(object sender, RoutedEventArgs e)
        {
            simulator();
            worker.RunWorkerAsync();
            simulatorButton.IsEnabled = false;
            simulatorButton.Visibility = Visibility.Collapsed;
            CancelSimBtn.Visibility = Visibility.Visible;
            ChargingButton.IsEnabled = false;
            DeliveryButton.IsEnabled = false;
            AddUpdateButton.IsEnabled = false;
            
        }

        private void CancelSimBtn_Click(object sender, RoutedEventArgs e)
        {
            worker.CancelAsync();
            Cursor = Cursors.Wait;
        }

        private void HideAndShow()
        {
            Batterylable.Visibility = Visibility.Hidden;
            Lontextbox.Visibility = Visibility.Hidden;
            lable7.Visibility = Visibility.Hidden;
            latlable.Visibility = Visibility.Hidden;
            LattextBox.Visibility = Visibility.Hidden;
            BatteryTextBox.Visibility = Visibility.Hidden;

            RBattery.Visibility = Visibility.Visible;
            RLocation.Visibility = Visibility.Visible;
            RDoing.Visibility = Visibility.Visible;
            batteryPrecentage.Visibility = Visibility.Visible;
            ppp.Visibility = Visibility.Visible;
            BATTERYlabel1.Visibility = Visibility.Visible;
            locationLabel2.Visibility = Visibility.Visible;
            longitudeLabel2.Visibility = Visibility.Visible;
            latitudeLabel3.Visibility = Visibility.Visible;
            doingLabel.Visibility = Visibility.Visible;
        }
    }
}
