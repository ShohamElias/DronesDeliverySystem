using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DalApi
{
    public  class DalFactory
    {
        public static IDal GetDal()
        {
            string daltype = DalConfig.DalName;
            string dalpkg = DalConfig.Dalpackeges[daltype];
            if (dalpkg == null) throw new DO.DalConfigException($"Package {daltype} is not found in packages list in dal-config.xml");
        
            try { Assembly.Load(dalpkg); }
            catch (Exception) { throw new DO.DalConfigException("Failed to load the dal-config.xml file"); }

            Type type = Type.GetType($"Dal.{dalpkg},{dalpkg}");
            if (type == null) throw new DO.DalConfigException($"Class {dalpkg} was not found in the {dalpkg}.dll");

            IDal idal = (IDal)type.GetProperty("Instance",
                BindingFlags.Public | BindingFlags.Static).GetValue(null);
            if (idal == null) throw new DO.DalConfigException($"Class {dalpkg} is not singelton or wrong property name for Instance");

            return idal;
        }
    }
}
