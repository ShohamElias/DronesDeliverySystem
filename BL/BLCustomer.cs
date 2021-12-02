using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using IBAL.BO;
using IBL.BO;
//using IDAL.DO;

namespace IBL
{
    public partial class BL 
    {
        private Customer customerDoBoAdapter(IDAL.DO.Customer customerDO)
        {
            Customer customerBO = new Customer();
            int id = customerDO.Id;
            IDAL.DO.Customer s;
            try //???
            {
                s = AccessIdal.GetCustomer(id);
            }
            catch (IDAL.DO.BadIdException)
            {

                throw new BadIdException("station");
            }
            s.CopyPropertiesTo(customerBO);
            customerDO.CopyPropertiesTo(customerBO);
            //stationBO.DronesinCharge= from sic in AccessIdal.GetALLDrone(sic=> sic.Id==Id )
            //                          let 
            return customerBO;

        }
        public void AddCustomer(Customer newCustomer)
        {
            
            IDAL.DO.Customer newCus = new IDAL.DO.Customer()
            {
                Id = newCustomer.Id,
                Name = newCustomer.Name,
                Phone = newCustomer.Phone,
                Longitude = newCustomer.CustLocation.Longitude,
                Lattitude = newCustomer.CustLocation.Lattitude     
              
            };
            try
            {
                AccessIdal.AddCustomer(newCus);
            }
            catch (IDAL.DO.IDExistsException)
            {
               throw new IDExistsException("customer");//#########################
            }
        }
        public void UpdateCustomer(int cusId, string cusName, string cusPhone)
        {
            try
            {
                if (!AccessIdal.CheckCustomer(cusId))
                    throw new IDAL.DO.BadIdException("doesnt exist");
                IDAL.DO.Customer cus = AccessIdal.GetCustomer(cusId);
                if (cusName != "")
                    cus.Name = cusName;
                if (cusPhone != "")
                    cus.Phone = cusPhone;
                AccessIdal.UpdateCustomer(cus);
            }
            catch (IDAL.DO.BadIdException)
            {

                throw new BadIdException("customer"); //##############
            }
        }
        public IEnumerable<Customer> GetAllCustomers()
        {
            return from item in AccessIdal.GetALLCustomer()
                   orderby item.Id
                   select GetCustomer(item.Id);                  
        }

        public IEnumerable<Customer> GetAllCustomersThatHasDeliveredParcels()
        {
            return from item in AccessIdal.GetALLCustomer()
                   orderby item.Id
                   select GetCustomer(item.Id);
        }

        public Customer GetCustomer(int id)
        {
            if (!AccessIdal.CheckCustomer(id))
                throw new BadIdException("customer"); //##################
            IDAL.DO.Customer c = AccessIdal.GetCustomer(id);
            
            Customer cb = new Customer()
            {
                Id = id,
                CustLocation = new Location() { Lattitude = c.Lattitude, Longitude = c.Longitude },
                Name = c.Name,
                Phone = c.Phone,     
            };
            cb.parcelToCustomer = new List<Parcel>();
            cb.parcelFromCustomer = new List<Parcel>();
            foreach (Parcel item in GetAllParcels())
            {
                if (item.Sender.Id == c.Id)
                    cb.parcelFromCustomer.Add(item);
            }
            foreach (Parcel item in GetAllParcels())
            {
                if (item.Target.Id == c.Id)
                    cb.parcelToCustomer.Add(item);
            }
            return cb;
        }

        public string ShowOneCustomer(int _id)
        {
            Customer s = GetCustomer(_id); //finding the station by its id
            return s.ToString();
        }

        public IEnumerable<Customer> GetAllCusromerRecived()
        {
            return from item in GetAllCustomers()
                   where item.parcelToCustomer.Count > 0
                   select GetCustomer(item.Id);
        }
    }
}
