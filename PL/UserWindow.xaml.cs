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
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        BlApi.IBL bl;
        public UserWindow(BlApi.IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
        }

        private void Loginbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Customer c = bl.GetCustomer(int.Parse(Id.Text.ToString()));
                if (!passwordBox.Password.Equals(c.password))
                {
                    throw new BO.BadIdException(c.Id, "The id or the password were wrong, try again");    
                }
                new CustomerShowWindow(c, bl, "User").Show();
                this.Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "Error Occurred", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewUserbutton_Click(object sender, RoutedEventArgs e)
        {
            new CustomerShowWindow(bl).Show();
            this.Close();
        }
    }
}
