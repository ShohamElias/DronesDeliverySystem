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
        double CustomerDistance(double lat, double lon1, int id);
        string ShowOneCustomer(int _id);
        IEnumerable<DO.Customer> ListCustomer();
        #endregion

        #region Drone
        void AddDrone(DO.Drone d);
        void LinkParcelToDrone(int parcelId, int droneId);
        void DroneToCharge(int droneId, int stationId);
        void EndingCharge(int droneId);
        string ShowOneDrone(int _id);
        IEnumerable<DO.Drone> ListDrone();

        #endregion

        #region Parcel
        void AddParcel(DO.Parcel per);
        void PickParcel(int parcelId);
        void DeliveringParcel(int parcelId);
        string ShowOneParcel(int _id);
        IEnumerable<DO.Parcel> ListParcel();
        #endregion

        #region Station
        double StationDistance(double lat, double lon1, int id);
        void AddStation(DO.Station s);
        string ShowOneStation(int _id);
        IEnumerable<DO.Station> ListStation();

        #endregion

    }
}
