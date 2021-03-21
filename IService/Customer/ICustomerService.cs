using NRDCL.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NRDCL.Models
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetCustomerList();
        Task<Customer> GetCustomerDetails(string CitizenshipID);
        Task<bool> IsCustomerExist(string citizenshipID);
        Task<ResponseMessage> SaveCustomer(Customer customer);
        Task<ResponseMessage> UpdateCustomer(Customer customer);
        ResponseMessage DeleteCustomer(string citizenshipID);
        List<Report.Report> GetReportData(int customerReportNo);
    }
}
