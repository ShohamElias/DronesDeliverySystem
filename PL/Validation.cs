using System;
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
using System.Globalization;

namespace PL
{

    
        public class Validation
        {

            public bool IsnumberChar(string c) //the func checks if the string is a number
            {
                for (int i = 0; i < c.Length; i++)
                {
                    if ((c[i] < '0' || c[i] > '9') && (c[i] != '\b'))
                        return false;

                }
                return true;
            }
            public bool IsnumberCharLoc(string c) //the func checks if the string is a right input to location
            {
                for (int i = 0; i < c.Length; i++)
                {
                    if ((c[i] < '0' || c[i] > '9') && (c[i] != '\b') && (c[i] != '.'))
                        return false;

                }
                return true;
            }
        }
    
}
