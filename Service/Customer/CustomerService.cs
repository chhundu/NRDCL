using NRDCL.Data;
using NRDCL.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NRDCL.Models
{
    public class CustomerService : ICustomerService
    {
        private readonly NRDCL_DB_Context dataBaseContext;

        public CustomerService(NRDCL_DB_Context context)
        {
            dataBaseContext = context;
        }

        /// <summary>
        /// This method is use to get customer detail based on CitizenshipID
        /// </summary>
        /// <param name="CitizenshipID"></param>
        /// <returns></returns>
        public Customer GetCustomerDetails(String CitizenshipID)
        {
            var customer = (from cus in dataBaseContext.Customer_Table where cus.CitizenshipID.Equals(CitizenshipID) select cus).SingleOrDefault();

            return customer;
        }

        public List<Customer> GetCustomerList()
        {
                List<Customer> customerList = dataBaseContext.Customer_Table.ToList();
            return customerList;
        }

        public ResponseMessage DeleteCustomer(string CitizenshipID)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            var customer=GetCustomerDetails(CitizenshipID);
            dataBaseContext.Customer_Table.Remove(customer);
            dataBaseContext.SaveChanges();
            responseMessage.Status = true;
            responseMessage.Text = "Customer deleted succesfully.";
            return responseMessage;
        }

        public bool IsCustomerExist(string citizenshipID)
        {
            return dataBaseContext.Customer_Table.Any(x => x.CitizenshipID.Equals(citizenshipID)); ;
        }

        public ResponseMessage SaveCustomer(Customer customer) {

            ResponseMessage responseMessage = new ResponseMessage();

            if (IsCustomerExist(customer.CitizenshipID))
            {
                responseMessage.Status=false;
                responseMessage.Text = "Customer with this citizenshipID already exists";
                return responseMessage;
               
            }

            dataBaseContext.Add(customer);
            dataBaseContext.SaveChanges();
            responseMessage.Status = true;
            responseMessage.Text = "Customer saved succesfully.";

            return responseMessage;
        }

        public ResponseMessage UpdateCustomer(Customer customer)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                dataBaseContext.Update(customer);
                dataBaseContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IsCustomerExist(customer.CitizenshipID))
                {
                    responseMessage.Status = false;
                    responseMessage.Text = "Customer doesn't exist.";
                    return responseMessage;
                }
                else
                {
                    throw;
                }
            }
            responseMessage.Status = true;
            responseMessage.Text = "Customer updated succesfully.";
            return responseMessage;
        }
    }
}