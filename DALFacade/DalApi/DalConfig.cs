﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DalApi
{
    
    class DalConfig
    {
        internal static string DalName;
        internal static Dictionary<string, string> Dalpackeges;
        static DalConfig()
        {
            XElement dalconfig = XElement.Load(@"xml\dal-config.xml");
            DalName = dalconfig.Element("dal").Value;
            
            Dalpackeges = (from pkg in dalconfig.Element("dal-packages").Elements()
                           select pkg).ToDictionary(p => "" + p.Name, p => p.Value);
            
        }
    }


}