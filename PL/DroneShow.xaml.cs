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
        bool updateflag = false;
        public DroneShow(IBL.IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            //textBox1.Text =" ";
            AddUpdateButton.Content = "Add";
            idtextbox.Text = "";
            modelTextbox.Text = "";
            LattextBox.Text = "";
            Lontextbox.Text = "";
            BatteryTextBox.Text = "";

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
            ChargingButton.Visibility = Visibility.Visible;
            DeliveryButton.Visibility = Visibility.Visible;
            idtextbox.Text = "";
            modelTextbox.Text = "";
            LattextBox.Text = "";
            Lontextbox.Text = "";
            BatteryTextBox.Text = "";


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
            label7.Visibility = Visibility.Hidden;
            WeightLable.Visibility = Visibility.Hidden;


            // textBox1.Text = " ";
            StatusSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            // IBL.BO.Station s = bl.GetStationsforNoEmpty().First();
            if (AddUpdateButton.Content.Equals("Add"))
            {
                bl.AddDrone(new IBL.BO.Drone()
                {
                    Id = Convert.ToInt32(idtextbox.Text.ToString()),
                    Battery = Convert.ToInt32(BatteryTextBox.Text.ToString()),
                    Model = modelTextbox.Text.ToString(),
                    MaxWeight = (IBL.BO.WeightCategories)Convert.ToInt32(WeightSelector.SelectedIndex.ToString()),
                    Status = (IBL.BO.DroneStatuses)Convert.ToInt32(StatusSelector.SelectedIndex.ToString()),
                    CurrentParcel = null,
                    CurrentLocation = null


                }, 0);
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
                    idtextbox.Text = d.Id.ToString();
                }
                else
                {
                    updateflag = false;
                    bl.UpdateDrone(d.Id, modelTextbox.Text);
                    this.Close();
                }
              //  idtextbox.ena
            }
            
        }

        private void DeliveryButton_Click(object sender, RoutedEventArgs e)
        {
            if (d.Status == IBL.BO.DroneStatuses.Available)
                bl.LinkDroneToParcel(d.Id);
            else if (d.Status == IBL.BO.DroneStatuses.Delivery)
            {
                IBL.BO.Parcel p = bl.GetParcel(d.CurrentParcel.Id);
                if (p.PickedUp == null)
                    bl.PickParcel(p.Id);
                else if (p.Delivered == null)
                    bl.DeliveringParcel(p.Id);
            }
        }

        private void ChargingButton_Click(object sender, RoutedEventArgs e)
        {
            if (d.Status == IBL.BO.DroneStatuses.Available)
                bl.DroneToCharge(d.Id);
            else if (d.Status == IBL.BO.DroneStatuses.Maintenance)
                bl.EndCharging(d.Id);
            else
            { //massagebox
            }
            this.Close(); 
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
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
            this.Close();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
           
        }

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{

        //}
    }
}
