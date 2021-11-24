using System;
using IBL.BO;
//using IDAL.DO;
//using DalObject;
using System.Linq;
using System.Collections.Generic;

namespace IBL
{
    public partial class BL: IBL.IBL
    {
        // public IDAL.IDal dl;
        public IDAL.IDal AccessIdal;

        public BL()
        {
            AccessIdal = new DalObject.DalObject();
        }
    }
}
