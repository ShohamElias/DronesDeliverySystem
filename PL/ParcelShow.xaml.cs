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
    /// Interaction logic for ParcelShow.xaml
    /// </summary>
    public partial class ParcelShow : Window
    {
        BlApi.IBL bl;
        IEnumerable<BO.Customer> cc;
        IEnumerable<BO.Drone> dd;
        int parcelid;
        bool closingwin = true;
        public ParcelShow(BlApi.IBL _bl,int id)
        {
            InitializeComponent();
            parcelid = id;
            idTextBox.IsEnabled = true;          
            idTextBox.Text = id.ToString();
            idTextBox.IsReadOnly = true;
            addUpdateButton.Content = "Add";
            bl = _bl;
            weightComboBox.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            priorityComboBox.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            cc = bl.GetAllCustomers();
            senderComboBox.ItemsSource = cc;
            targetComboBox.ItemsSource = cc;
            requestedDatePicker.Visibility = Visibility.Collapsed;
            requestedlLable.Visibility = Visibility.Collapsed;
            pickedUpDatePicker.Visibility = Visibility.Collapsed;
            pickedUpLable.Visibility = Visibility.Collapsed;
            deliveredDatePicker.Visibility = Visibility.Collapsed;
            deliveredLable.Visibility = Visibility.Collapsed;
            
            droneLable.Visibility = Visibility.Collapsed;
            droneParcelComboBox.Visibility = Visibility.Collapsed;
            schedualLable.Visibility = Visibility.Collapsed;
            scheduledDatePicker.Visibility = Visibility.Collapsed;

            droneinparcelButton.Visibility = Visibility.Hidden;
            senderButton.Visibility = Visibility.Hidden;
            targetButton.Visibility = Visibility.Hidden;
        }

        public ParcelShow(BO.Parcel p, BlApi.IBL _bl)
        {
            InitializeComponent();
            closingwin = true;
            parcelGrid.DataContext = p;
            addUpdateButton.Content = "Cancel";
            bl = _bl;
            if (p.PickedUp == null)
            {
                pickedUpDatePicker.Visibility = Visibility.Collapsed;
                pickedUpLable.Visibility = Visibility.Collapsed;
            }
            else
            {
                pickedUpDatePicker.Visibility = Visibility.Visible;
                pickedUpLable.Visibility = Visibility.Visible;
            }
            if (p.Requested == null)
            {
                requestedDatePicker.Visibility = Visibility.Collapsed;
                requestedlLable.Visibility = Visibility.Collapsed;
            }
            else
            {
                requestedDatePicker.Visibility = Visibility.Visible;
                requestedlLable.Visibility = Visibility.Visible;
            }
            if (p.Delivered == null)
            {
                deliveredDatePicker.Visibility = Visibility.Collapsed;
                deliveredLable.Visibility = Visibility.Collapsed;
            }
            else
            {
                deliveredDatePicker.Visibility = Visibility.Visible;
                deliveredLable.Visibility = Visibility.Visible;
            }
            if (p.DroneParcel.Id==0)
            {
                droneLable.Visibility = Visibility.Collapsed;
                droneLable.Visibility = Visibility.Collapsed;
                droneinparcelButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                dd = bl.GetAllDrones();
                droneParcelComboBox.ItemsSource = dd;
                int _index = 0;
                foreach (var item in dd)
                {
                    if (item.Id == p.DroneParcel.Id)
                        break;
                    _index++;

                }
                droneParcelComboBox.SelectedIndex = _index;
                droneinparcelButton.IsEnabled = true;

            }
            weightComboBox.ItemsSource= Enum.GetValues(typeof(BO.WeightCategories));
            priorityComboBox.ItemsSource= Enum.GetValues(typeof(BO.Priorities));
            this.cc = bl.GetAllCustomers();
            senderComboBox.ItemsSource = this.cc;
            targetComboBox.ItemsSource = this.cc;
            BO.Customer c = bl.GetCustomer(p.Sender.Id);
            int index =0;
            foreach (var item in cc)
            {
                if (p.Sender.Id == item.Id)
                    break;
                index++;
            }
            senderComboBox.SelectedIndex = index;
            index = 0;
            foreach (var item in cc)
            {
                if (p.Target.Id == item.Id)
                    break;
                index++;
            }
            targetComboBox.SelectedIndex = index;
            if(p.DroneParcel.Id==0)
            {
                droneParcelComboBox.Visibility = Visibility.Collapsed;
                droneLable.Visibility = Visibility.Collapsed;
            }

        }

        

        private void addUpdateButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (addUpdateButton.Content == "Add")
            {
                BO.Parcel p = new BO.Parcel();
                p.Id = parcelid;
                
                if (priorityComboBox.SelectedIndex != -1)
                    p.Priority = (BO.Priorities)priorityComboBox.SelectedItem;
                if (weightComboBox.SelectedIndex != -1)
                    p.Weight = (BO.WeightCategories)weightComboBox.SelectedItem;
                if (senderComboBox.SelectedIndex!=-1)
                   p.Sender = new BO.CustomerInParcel() {Id=cc.ElementAt(senderComboBox.SelectedIndex).Id,CustomerName= cc.ElementAt(senderComboBox.SelectedIndex).Name };
               if(targetComboBox.SelectedIndex!=-1)
                    p.Target = new BO.CustomerInParcel() { Id = cc.ElementAt(targetComboBox.SelectedIndex).Id, CustomerName = cc.ElementAt(targetComboBox.SelectedIndex).Name };
                bl.AddParcel(p);
                this.Close();
            }
            else if (addUpdateButton.Content == "Cancel")
            {
                this.Close();
            }
        }

        private void droneinparcelButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneShow(bl, dd.ElementAt(droneParcelComboBox.SelectedIndex),"show").Show();
        }

        private void senderButton_Click(object sender, RoutedEventArgs e)
        {
            //BO.Customer c = cc.ElementAt(senderComboBox.SelectedIndex);

            new CustomerShowWindow(cc.ElementAt(senderComboBox.SelectedIndex), bl, "show").Show();

        }

        private void targetButton_Click(object sender, RoutedEventArgs e)
        {
            new CustomerShowWindow(cc.ElementAt(targetComboBox.SelectedIndex), bl, "show").Show();

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
    }
}
