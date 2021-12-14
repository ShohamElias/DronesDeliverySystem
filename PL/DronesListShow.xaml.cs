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
    /// Interaction logic for DronesListShow.xaml
    /// </summary>
    public partial class DronesListShow : Window
    {
        public DronesListShow(IBL.IBL _bl)
        {
            InitializeComponent();
            DronesListView.ItemsSource = _bl.GetAllDrones();
        }
    }
}
