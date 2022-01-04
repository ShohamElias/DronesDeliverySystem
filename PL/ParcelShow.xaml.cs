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
    /// Interaction logic for ParcelShow.xaml
    /// </summary>
    public partial class ParcelShow : Window
    {
        BlApi.IBL bl;

        public ParcelShow(BlApi.IBL _bl)
        {

        }

        public ParcelShow(BO.Parcel p, BlApi.IBL _bl)
        {
            InitializeComponent();
            parcelGrid.DataContext = p;
            addUpdateButton.Content = "Update";
            bl = _bl;
            if (p.PickedUp == null)
            {
                pickedUpDatePicker.Visibility = Visibility.Hidden;
                pickedUpLable.Visibility = Visibility.Hidden;
            }
            else
            {
                pickedUpDatePicker.Visibility = Visibility.Visible;
                pickedUpLable.Visibility = Visibility.Visible;
            }
            if (p.Requested == null)
            {
                requestedDatePicker.Visibility = Visibility.Hidden;
                requestedlLable.Visibility = Visibility.Hidden;
            }
            else
            {
                requestedDatePicker.Visibility = Visibility.Visible;
                requestedlLable.Visibility = Visibility.Visible;
            }
            if (p.Delivered == null)
            {
                deliveredDatePicker.Visibility = Visibility.Hidden;
                deliveredLable.Visibility = Visibility.Hidden;
            }
            else
            {
                deliveredDatePicker.Visibility = Visibility.Visible;
                deliveredLable.Visibility = Visibility.Visible;
            }
            weightComboBox.ItemsSource= Enum.GetValues(typeof(BO.WeightCategories));
            priorityComboBox.ItemsSource= Enum.GetValues(typeof(BO.Priorities));
            senderComboBox.ItemsSource = bl.GetAllCustomers();
            targetComboBox.ItemsSource = bl.GetAllCustomers();
            BO.Customer c = bl.GetCustomer(p.Sender.Id);
            IEnumerable<BO.Customer> cc = bl.GetAllCustomers();
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

        private void addUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (addUpdateButton.Content == "Add")
            {
                bl.AddParcel((BO.Parcel) parcelGrid.DataContext); 
            }
            else if(addUpdateButton.Content == "Update")
            {
                BO.Parcel p = new BO.Parcel()
                {
                    Id=int.Parse(idTextBox.Text),
                    Weight=(BO.WeightCategories)weightComboBox.SelectedItem,
                    Priority=(BO.Priorities)priorityComboBox.SelectedItem,
                   
                };
                if(senderComboBox.SelectedIndex!=-1)
                     
                bl.UpdateParcel((BO.Parcel)parcelGrid.DataContext);
            }
        }
    }
}
