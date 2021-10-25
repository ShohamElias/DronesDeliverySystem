using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public class DalObject
    {
        public DalObject()
        {
            DataSource.Intialize();
        }

        public void addDrone(int id, string model, WeightCategories maxWeight, DroneStatuses status , double battery)//static?
        {
            Drone d = new Drone();
            d.Id = id;
            d.MaxWeight = maxWeight;
            d.Model = model;
            d.Status = status;
            d.Battery = battery;
        }
    }
}
