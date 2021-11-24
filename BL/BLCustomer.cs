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
               throw new IDExistsException("customer");
            }
        }
        public void UpdateCustomer(int cusId, string cusName, string cusPhone)
        {
            try
            {

            }
            catch (IDAL.DO.BadIdException)
            {

                throw;
            }
        }
        public IEnumerable<Customer> GetAllCustomers()
        {
            return 
        }


    }
}
