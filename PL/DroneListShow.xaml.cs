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
    /// Interaction logic for DroneListShow.xaml
    /// </summary>
    public partial class DroneListShow : Window
    {
        IBL.IBL bl;
        public DroneListShow()
        {
            InitializeComponent();
        }

        public DroneListShow(IBL.IBL _bl)
        {
            InitializeComponent();
             bl = _bl;
            DronesListView.ItemsSource = bl.GetAllDrones();
        }
    }
}