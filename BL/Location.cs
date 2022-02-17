using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
    {
        public class Location
        {
            public double Longitude { get; set; }
            public double Lattitude { get; set; }

        //public override string ToString()
        //{
        //    double degrees = (Lattitude);//converting to sexagesimal
        //    double minutes = (degrees - (int)degrees) * 60;//converting the minutes by multiplying by the base number
        //    double seconds = (minutes - (int)minutes) * 60;//converting the seconds by multiplying them by the base number
        //    string lati = (int)degrees + "° " + (int)minutes + "'" + (((int)(seconds * 1000)) / (float)1000)//the final outcome
        //            + "\"";

        //    double degrees2 = (Longitude);//converting to sexagesimal
        //    double minutes2 = (degrees2 - (int)degrees2) * 60;//converting the minutes by multiplying by the base number
        //    double seconds2 = (minutes2 - (int)minutes2) * 60;//converting the seconds by multiplying by the base number
        //    string longi = (int)degrees2 + "° " + (int)minutes2 + "'" + (((int)(seconds2 * 1000)) / (float)1000)
        //            + "\"";
        //    return $" Longitude = {longi}, Lattitude = {lati},";
        //}
    }



}

