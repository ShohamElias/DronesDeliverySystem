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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PL;


namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BlApi.IBL bl;
        public MainWindow()
        {
            InitializeComponent();
            bl = BlApi.BlFactory.GetBL();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new CustomerList(bl).Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(bl).Show();
        }

        private void stationList_Click(object sender, RoutedEventArgs e)
        {
            new StationListWindow(bl).Show();
        }

        private void Userbutton_Click(object sender, RoutedEventArgs e)
        {
            new UserWindow(bl).Show();
        }
        private void Customer_Click(object sender, RoutedEventArgs e)
        {
            new DronesListShow(bl).Show();
        }
    }
}
