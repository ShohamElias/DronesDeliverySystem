using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public static class DalFactory
    {
        public static IDal GetDal(string type)
        {
            switch (type)
            {
                case "List":
                    return DalObject.DalObject.Instance;
                default: throw new DO.BadGetException("ERROR");
                    break;
            }
        }
    }
}
