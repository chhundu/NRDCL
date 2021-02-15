using NRDCL.Models;
using NRDCL.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NRDCL.Models
{
    public interface ICustomerService
    {
        List<Customer> GetCustomerList();
        Customer GetCustomerDetails(string CitizenshipID);
        bool IsCustomerExist(string citizenshipID);
        ResponseMessage SaveCustomer(Customer customer);
        ResponseMessage UpdateCustomer(Customer customer);
        ResponseMessage DeleteCustomer(string citizenshipID);
    }
}
