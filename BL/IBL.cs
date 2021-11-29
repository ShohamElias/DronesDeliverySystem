using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    public interface IBL
    {
        #region Drone
        public BO.Drone GetDrone(int id);
         IEnumerable<BO.Drone> GetAllDrones();
        public void AddDrone(BO.Drone d, int x);

        #endregion
        #region Custoner
        public void AddCustomer(BO.Customer d);
        public void UpdateCustomer(int cusid, string cusName, string cusPhone);
        public IEnumerable<BO.Customer> GetAllCustomers();

        #endregion

        #region Parcel
        public void AddParcel(BO.Parcel p);
        //public void UpdateCustomer(int cusid, string cusName, string cusPhone);
        public IEnumerable<BO.Parcel> GetAllParcels();

        #endregion
    }
}
