using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    public interface IDal
    {
        #region Customer
        void AddCustomer(DO.Customer cus);
        public void UpdateCustomer(DO.Customer newD);

        public DO.Customer GetCustomer(int id);
        public bool CheckCustomer(int id);
        double CustomerDistance(double lat, double lon1, int id);
        string ShowOneCustomer(int _id);
        IEnumerable<DO.Customer> ListCustomer();
        public IEnumerable<DO.Customer> GetALLCustomer();
        #endregion

        #region Drone
        void AddDrone(DO.Drone d);
        public DO.Drone GetDrone(int id);
        public bool CheckDrone(int id);
        void LinkParcelToDrone(int parcelId, int droneId);
        void DroneToCharge(int droneId, int stationId);
        void EndingCharge(int droneId);
        string ShowOneDrone(int _id);
        public void UpdateDrone(DO.Drone newD);
        double[] ElectricityUse();
        IEnumerable<DO.Drone> ListDrone();
        public IEnumerable<DO.Drone> GetALLDrone();

        #endregion

        #region Parcel
        void AddParcel(DO.Parcel per);
        public void UpdateParcel(DO.Parcel newD);
        public DO.Parcel GetParcel(int id);
        public bool CheckParcel(int id);
        void PickParcel(int parcelId);
        void DeliveringParcel(int parcelId);
        string ShowOneParcel(int _id);
        IEnumerable<DO.Parcel> ListParcel();
        public IEnumerable<DO.Parcel> GetALLParcel();
        #endregion

        #region Station
        double StationDistance(double lat, double lon1, int id);
        public void UpdateStation(DO.Station newD);
        void AddStation(DO.Station s);
        public DO.Station GetStation(int id);
        public bool CheckStation(int id);
        string ShowOneStation(int _id);
        IEnumerable<DO.Station> ListStation();
        public IEnumerable<DO.Station> GetALLStation();
        int NumOfChargingNow(int x);

        #endregion

    }
}
