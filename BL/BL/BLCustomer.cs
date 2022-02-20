﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using BO;
using BlApi;
using DalApi;


namespace BL
{
     partial class BL 
    {    
        /// <summary>
        /// the func gets a new customer and add it
        /// </summary>
        /// <param name="newCustomer">the new customer</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(Customer newCustomer)
        {

            DO.Customer newCus = new DO.Customer()
            {
                Id = newCustomer.Id,
                Name = newCustomer.Name,
                Phone = newCustomer.Phone,
                Longitude = newCustomer.CustLocation.Longitude,
                Lattitude = newCustomer.CustLocation.Lattitude,
                password = newCustomer.password
              
            };
            lock (AccessIdal)
            {
                try
                {
                    AccessIdal.AddCustomer(newCus);
                }
                catch (DO.IDExistsException)
                {
                    throw new IDExistsException(newCus.Id, "this customer already exists");
                }
            }
        }

        /// <summary>
        /// updating the bl customer: name and phone, by its id, and sending to change on the dal level 
        /// </summary>
        /// <param name="cusId"></param>
        /// <param name="cusName"></param>
        /// <param name="cusPhone"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(int cusId, string cusName, string cusPhone)
        {
            lock (AccessIdal)
            {
                try
                {
                    DO.Customer cus = AccessIdal.GetCustomer(cusId);
                    if (cusName != "")
                        cus.Name = cusName;
                    if (cusPhone != "")
                        cus.Phone = cusPhone;
                    AccessIdal.UpdateCustomer(cus);
                }
                catch (DO.BadIdException)
                {
                    throw new BadIdException(cusId, "this customer doesn't exist");
                }
            }
        }
        /// <summary>
        /// thre function returns all the customers
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetAllCustomers()
        {
            lock (AccessIdal)
            {
                return from item in AccessIdal.GetALLCustomer()
                       orderby item.Id
                       select GetCustomer(item.Id);
            }
        }
        /// <summary>
        /// the function returns all the customers that have deievered parcels
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetAllCustomersThatHasDeliveredParcels()
        {
            lock (AccessIdal)
            {
                return from item in GetAllCustomers()
                       where item.parcelFromCustomer.Count>0
                       orderby item.Id
                       select GetCustomer(item.Id);
            }
        }

        /// <summary>
        /// the functions gets an id and returs the customer the id belongs to
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomer(int id)
        {
            lock (AccessIdal)
            {
                if (!AccessIdal.CheckCustomer(id))
                    throw new BadIdException(id, "this customer doesn't exist!");
                DO.Customer c = AccessIdal.GetCustomer(id);
                Customer cb = new Customer()
                {
                    Id = id,
                    CustLocation = new Location() { Lattitude = c.Lattitude, Longitude = c.Longitude },
                    Name = c.Name,
                    Phone = c.Phone,
                    password = c.password
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
        }

        /// <summary>
        /// the funxtion returns all the customers that has received parcels
        /// </summary>
        /// <returns></returns> list of customer
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetAllCusromerRecived()
        {
            lock (AccessIdal)
            {
                return from item in GetAllCustomers()
                       where item.parcelToCustomer.Count > 0
                       select GetCustomer(item.Id);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetCustomerBy(Predicate<Customer> P)//lock??
        {
            return from d in GetAllCustomers()
                   where P(d)
                   select d;
        }
        /// <summary>
        /// the func returns all of the customer but as TOLIST customers
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public CustomerToList GetCustomerToList(int id)
        {
            lock (AccessIdal)
            {
                if (!AccessIdal.CheckCustomer(id))
                    throw new BadIdException(id, "this customer doesn't exist!");
            }

            Customer c = GetCustomer(id);
            CustomerToList ct = new CustomerToList()
            {
                Id = id,
                Name = c.Name,
                Phone = c.Phone,
                ParcelsArrived=0,
                ParcelsDelivered=0,
                ParcelsOnTheWay=0,
                PurcelsNotDelivered=0               
            };

            foreach (var item in c.parcelFromCustomer)
            {
                if (item.Delivered != null)
                    ct.ParcelsArrived++;
                else
                    ct.ParcelsOnTheWay++;
            }

            foreach (var item in c.parcelToCustomer)
            {
                if (item.Delivered != null)
                    ct.ParcelsDelivered++;
                else
                    ct.PurcelsNotDelivered++;
            }
            return ct;
        }
    }
}
