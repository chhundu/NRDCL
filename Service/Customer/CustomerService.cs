using NRDCL.Data;
using NRDCL.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace NRDCL.Models
{
    public class CustomerService : ICustomerService
    {
        private readonly NRDCL_DB_Context dataBaseContext;
        public CommonProperties CommonProperties { get; set; }

        public CustomerService(NRDCL_DB_Context context)
        {
            dataBaseContext = context;
        }

        /// <summary>
        /// This method is use to get customer detail based on CitizenshipID
        /// </summary>
        /// <param name="CitizenshipID"></param>
        /// <returns></returns>
        public async Task<Customer> GetCustomerDetails(String CitizenshipID)
        {
            var customer = (from cus in dataBaseContext.Customer_Table where cus.CitizenshipID.Equals(CitizenshipID) select cus).SingleOrDefault();
            return await Task.FromResult(customer);
        }

        public async Task<List<Customer>> GetCustomerList()
        {
            List<Customer> customerList = dataBaseContext.Customer_Table.ToList();
            return await Task.Run(() => customerList) ;
        }

        public ResponseMessage DeleteCustomer(string CitizenshipID)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            var customer=GetCustomerDetails(CitizenshipID);
            dataBaseContext.Customer_Table.Remove(customer.Result);
            dataBaseContext.SaveChanges();
            responseMessage.Status = true;
            responseMessage.Text = CommonProperties.deleteSuccessMsg;
            return responseMessage;
        }

        public async Task<bool> IsCustomerExist(string citizenshipID)
        {
            return await Task.FromResult(dataBaseContext.Customer_Table.Any(x => x.CitizenshipID.Equals(citizenshipID)));
        }

        public async Task<ResponseMessage> SaveCustomer(Customer customer) {

            ResponseMessage responseMessage = new ResponseMessage();

            if (IsCustomerExist(customer.CitizenshipID).Result)
            {
                responseMessage.Status=false;
                responseMessage.Text = CommonProperties.citizenshipIDExistMsg;
                responseMessage.MessageKey = "CitizenshipID";
                return responseMessage;
            }

            dataBaseContext.Add(customer);
            dataBaseContext.SaveChanges();
            responseMessage.Status = true;
            responseMessage.Text = CommonProperties.saveSuccessMsg;

            return await Task.FromResult(responseMessage);
        }

        public async Task<ResponseMessage> UpdateCustomer(Customer customer)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                dataBaseContext.Update(customer);
                await dataBaseContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IsCustomerExist(customer.CitizenshipID).Result)
                {
                    responseMessage.Status = false;
                    responseMessage.Text = CommonProperties.citizenshipIDNotRegisteredMsg;
                    responseMessage.MessageKey = "CitizenshipID";
                    return responseMessage;
                }
                else
                {
                    throw;
                }
            }
            responseMessage.Status = true;
            responseMessage.Text = CommonProperties.updateSuccessMsg;
            return await Task.FromResult(responseMessage);
        }

        public List<Report.Report> GetReportData(int reportNumber)
        {
            var reportDataList = new List<Report.Report>();

            if (reportNumber == 1)//customer report
            {
                reportDataList = (from o in dataBaseContext.Order_Table
                                  join c in dataBaseContext.Customer_Table on o.CustomerID equals c.CitizenshipID
                                  join s in dataBaseContext.Site_Table on o.SiteID equals s.SiteId
                                  join d in dataBaseContext.Deposit_Table on o.CustomerID equals d.CustomerID
                                  join p in dataBaseContext.Product_Table on o.ProductID equals p.ProductId
                                  select new Report.Report
                                  {
                                      CustomerID = o.CustomerID,
                                      ProductID = p.ProductId,
                                      ProductName = p.ProductName,
                                      PriceAmount = p.PricePerUnit,
                                      TransportAmount = p.TransportRate * (decimal)s.DistanceFrom,
                                      AdvanceBalance = d.Balance
                                  }).ToList()
                              .GroupBy(reportGrouped => new { reportGrouped.CustomerID })
                            .Select(customerReport => new Report.Report()
                            {
                                CustomerID = customerReport.Key.CustomerID,
                                ProductName = customerReport.FirstOrDefault().ProductName,
                                PriceAmount = customerReport.Sum(a => a.PriceAmount),
                                TransportAmount = customerReport.Sum(a => a.TransportAmount),
                                AdvanceBalance = customerReport.Sum(a => a.AdvanceBalance),
                                ProductID = customerReport.FirstOrDefault().ProductID,
                            }).ToList();
            }

            else
            { //product report
                /// summing amounts and grouping by product id.
                reportDataList = (from o in dataBaseContext.Order_Table
                                  join c in dataBaseContext.Customer_Table on o.CustomerID equals c.CitizenshipID
                                  join s in dataBaseContext.Site_Table on o.SiteID equals s.SiteId
                                  join d in dataBaseContext.Deposit_Table on o.CustomerID equals d.CustomerID
                                  join p in dataBaseContext.Product_Table on o.ProductID equals p.ProductId
                                  select new Report.Report
                                  {
                                      CustomerID = o.CustomerID,
                                      ProductID = p.ProductId,
                                      ProductName = p.ProductName,
                                      PriceAmount = p.PricePerUnit,
                                      TransportAmount = p.TransportRate * (decimal)s.DistanceFrom,
                                      AdvanceBalance = d.Balance
                                  }).ToList()
                                  .GroupBy(reportGrouped => new { reportGrouped.ProductID })
                            .Select(productReport => new Report.Report()
                            {
                                ProductID = productReport.FirstOrDefault().ProductID,
                                ProductName = productReport.FirstOrDefault().ProductName,
                                PriceAmount = productReport.Sum(a => a.PriceAmount),
                                TransportAmount = productReport.Sum(a => a.TransportAmount)
                            }).ToList();
            }
            return reportDataList;
        }
    }
}