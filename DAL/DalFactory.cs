using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public static class DalFactory
    {
        public static DalApi.IDal GetDal(string type)
        {
            switch (type)
            {
                case "List":
                    return Dal.DalObject.Instance;
                    break;
                default: 
                    throw new DO.BadGetException("ERROR");
                    break;
            }
        }
    }
}
