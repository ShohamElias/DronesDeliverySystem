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
        bool closingwin = true;
        BO.Parcel updatep; 
        public ParcelShow(BlApi.IBL _bl,int id)
        {
            InitializeComponent();
            int num = 0;
            addUpdateButton.Content = "Add";
            closingwin = false;
            bl = _bl;
            weightComboBox.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            priorityComboBox.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            weightComboBox.IsEnabled = true;
            priorityComboBox.IsEnabled = true;
            senderComboBox.IsEnabled = true;
            targetComboBox.IsEnabled = true;
            cc = bl.GetAllCustomers();
            try
            {
                bl.GetCustomer(id);
                Predicate<BO.Customer> p;
                p = s => s.Id == id; //create the predicate
                senderComboBox.ItemsSource = bl.GetCustomerBy(p);
                senderComboBox.IsReadOnly = true;
            }
            catch (Exception)
            {

                senderComboBox.ItemsSource = cc;
            }
            targetComboBox.ItemsSource = cc;
            requestedDatePicker.Visibility = Visibility.Collapsed;
            requestedlLable.Visibility = Visibility.Collapsed;
            pickedUpDatePicker.Visibility = Visibility.Collapsed;
            pickedUpLable.Visibility = Visibility.Collapsed;
            deliveredDatePicker.Visibility = Visibility.Collapsed;
            deliveredLable.Visibility = Visibility.Collapsed;
            Removebutton.Visibility = Visibility.Collapsed;
            droneLable.Visibility = Visibility.Collapsed;
            droneParcelComboBox.Visibility = Visibility.Collapsed;
            schedualLable.Visibility = Visibility.Collapsed;
            scheduledDatePicker.Visibility = Visibility.Collapsed;
            droneinparcelButton.Visibility = Visibility.Hidden;
            senderButton.Visibility = Visibility.Hidden;
            targetButton.Visibility = Visibility.Hidden;

            num = bl.GetNextParcel();
            iDlabel.Content = num+1;
            iDlabel.Visibility = Visibility.Visible;
            idTextBox.Visibility = Visibility.Collapsed;
        }

        public ParcelShow(BO.Parcel p, BlApi.IBL _bl, string typeCust)
        {
            InitializeComponent();
            closingwin = true;
            parcelGrid.DataContext = p;
            updatep = p;
            addUpdateButton.Content = "Cancel";
            bl = _bl;
            if (p.PickedUp == null)
            {
                pickedUpDatePicker.Visibility = Visibility.Collapsed;
                pickedUpLable.Visibility = Visibility.Collapsed;
            }
            else
            {
                //pickedUpDatePicker.Visibility = Visibility.Visible;
                //pickedUpLable.Visibility = Visibility.Visible;
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
            if (p.Scheduled == null)
            {
                schedualLable.Visibility = Visibility.Collapsed;
                scheduledDatePicker.Visibility = Visibility.Collapsed;
            }
            else
            {
                scheduledDatePicker.Visibility = Visibility.Visible;
                schedualLable.Visibility = Visibility.Visible;
            }
            if (p.DroneParcel.Id==0)
            {
                droneLable.Visibility = Visibility.Collapsed;
                droneLable.Visibility = Visibility.Collapsed;
                droneinparcelButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                Removebutton.IsEnabled = false;
                Removebutton.Opacity = 0.5;
                dd = bl.GetAllDrones();
                droneParcelComboBox.ItemsSource = null;
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

            if (typeCust == "sender"|| typeCust=="target")
            {
                Cbutton.Visibility = Visibility.Visible;
                senderButton.Visibility = Visibility.Hidden;
                targetButton.Visibility = Visibility.Hidden;
                addUpdateButton.Content = "Update";
                if (pickedUpDatePicker != null)
                {
                    isPorD.Visibility = Visibility.Visible;
                    IsActionCheck.Visibility = Visibility.Visible;
                }
                if(typeCust == "sender")
                {
                    IsActionCheck.IsChecked = p.IsPicked;
                    isPorD.Content = "Parcel picked?";
                }
                else
                {
                    IsActionCheck.IsChecked = p.IsDelivered;
                    isPorD.Content = "Parcel Delivered?";
                }
            }
        }

        

        private void addUpdateButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (addUpdateButton.Content == "Add")
            {
                BO.Parcel p = new BO.Parcel();
                
                p.Id = bl.GetNextParcel()+1;
                if (targetComboBox.SelectedItem==senderComboBox.SelectedItem)
                {
                    MessageBox.Show("The sender cant be the same as the target");
                    return;
                }
                if (priorityComboBox.SelectedIndex != -1)
                    p.Priority = (BO.Priorities)priorityComboBox.SelectedItem;
                if (weightComboBox.SelectedIndex != -1)
                    p.Weight = (BO.WeightCategories)weightComboBox.SelectedItem;
                if (senderComboBox.SelectedIndex!=-1)
                   p.Sender = new BO.CustomerInParcel() {Id=cc.ElementAt(senderComboBox.SelectedIndex).Id,CustomerName= cc.ElementAt(senderComboBox.SelectedIndex).Name };
               if(targetComboBox.SelectedIndex!=-1)
                    p.Target = new BO.CustomerInParcel() { Id = cc.ElementAt(targetComboBox.SelectedIndex).Id, CustomerName = cc.ElementAt(targetComboBox.SelectedIndex).Name };
                p.Requested = DateTime.Now;
                bl.AddParcel(p);
                MessageBox.Show("Successfully completed the task.");
                closingwin = false;
                this.Close();
            }
            else if (addUpdateButton.Content == "Cancel")
            {
                closingwin = false;
                this.Close();
            }
            else
            {
                if(isPorD.Content == "Parcel picked?")
                {
                    updatep.IsPicked = IsActionCheck.IsChecked;
                }
                else
                    updatep.IsDelivered = IsActionCheck.IsChecked;
                bl.UpdateParcel(updatep);
                MessageBox.Show("Successfully completed the task.");
                closingwin = false;
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

        private void Removebutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.RemoveParcel(int.Parse(idTextBox.Text.ToString()));
                MessageBox.Show("Successfully completed the task.");
                closingwin = false;
                this.Close();
            }
            catch(Exception el)
            {
                Console.WriteLine(el);
            }
            
        }

        private void Cbutton_Click(object sender, RoutedEventArgs e)
        {
            closingwin = false;
            this.Close();
        }
    }
}
