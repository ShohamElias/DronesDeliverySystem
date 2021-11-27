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
        public void AddDrone(int id, string model, BO.WeightCategories w, int stationId);

        #endregion
        #region Custoner
        public void AddCustomer(BO.Customer d);
        public void UpdateCustomer(int cusid, string cusName, string cusPhone);
        public IEnumerable<BO.Customer> GetAllCustomers();

        #endregion

        #region Parcel
        public void AddParcel(int sender, int target, BO.WeightCategories w, BO.Priorities p);
        //public void UpdateCustomer(int cusid, string cusName, string cusPhone);
        public IEnumerable<BO.Parcel> GetAllParcels();

        #endregion
    }
}
