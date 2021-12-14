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
        public DroneShow(IBL.IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            StatusSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            bl.AddDrone(new IBL.BO.Drone()
            {
                Id = Convert.ToInt32(textBox.Text.ToString()),
                Battery = Convert.ToInt32(textBox.Text.ToString()),
                Model = textBox.Text.ToString(),
                MaxWeight = (IBL.BO.WeightCategories)Convert.ToInt32(WeightSelector.SelectedIndex.ToString()),
                Status = (IBL.BO.DroneStatuses)Convert.ToInt32(StatusSelector.SelectedIndex.ToString()),
                CurrentParcel = null,
                CurrentLocation= null


            }, 0); ;//station id, how???
            this.Close();
        }
    }
}
