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
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        BlApi.IBL bl;
        public ParcelWindow(BlApi.IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            parcelListView.DataContext = _bl.GetAllParcels();
        }

        private void parcelDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            parcelListView.DataContext = bl.GetAllParcels();

        }

        private void parcelListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Parcel p = parcelListView.SelectedItem as BO.Parcel;
            if (p != null)
                new ParcelShow(p, bl).Show();
        }
    }
}
