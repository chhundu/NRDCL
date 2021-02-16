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
                responseMessage.MessageKey = "CitizenshipID";
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
                    responseMessage.MessageKey = "CitizenshipID";
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

        public List<Report.Report> GetReportData(int reportNumber)
        {
            var reportDataList = new List<Report.Report>();

            //if (reportNumber == 1)//customer report
            //{
            //    reportDataList = (from o in dbContext.Orders
            //                      join c in dbContext.Customers on o.CustomerID equals c.CitizenshipID
            //                      join s in dbContext.Sites on o.SiteID equals s.SiteId
            //                      join d in dbContext.Deposits on o.CustomerID equals d.CustomerID
            //                      join p in dbContext.Products on o.ProductID equals p.ProductId
            //                      select new Report
            //                      {
            //                          CustomerID = o.CustomerID,
            //                          ProductID = p.ProductId,
            //                          ProductName = p.ProductName,
            //                          PriceAmount = p.PricePerUnit,
            //                          TransportAmount = p.TransportRate * s.DistanceFrom,
            //                          AdvanceBalance = d.Balance
            //                      }).ToList()
            //                  .GroupBy(reportGrouped => new { reportGrouped.CustomerID })
            //                .Select(customerReport => new Report()
            //                {
            //                    CustomerID = customerReport.Key.CustomerID,
            //                    ProductName = customerReport.FirstOrDefault().ProductName,
            //                    PriceAmount = customerReport.Sum(a => a.PriceAmount),
            //                    TransportAmount = customerReport.Sum(a => a.TransportAmount),
            //                    AdvanceBalance = customerReport.Sum(a => a.AdvanceBalance),
            //                    ProductID = customerReport.FirstOrDefault().ProductID,
            //                }).ToList();
            //}

            //else{ //product report
            //    /// summing amounts and grouping by product id.
            //    reportDataList = (from o in dbContext.Orders
            //                      join c in dbContext.Customers on o.CustomerID equals c.CitizenshipID
            //                      join s in dbContext.Sites on o.SiteID equals s.SiteId
            //                      join d in dbContext.Deposits on o.CustomerID equals d.CustomerID
            //                      join p in dbContext.Products on o.ProductID equals p.ProductId
            //                      select new Report
            //                      {
            //                          CustomerID = o.CustomerID,
            //                          ProductID = p.ProductId,
            //                          ProductName = p.ProductName,
            //                          PriceAmount = p.PricePerUnit,
            //                          TransportAmount = p.TransportRate * s.DistanceFrom,
            //                          AdvanceBalance = d.Balance
            //                      }).ToList()
            //                      .GroupBy(reportGrouped => new { reportGrouped.ProductID })
            //                .Select(productReport => new Report()
            //                {
            //                    ProductID = productReport.FirstOrDefault().ProductID,
            //                    ProductName = productReport.FirstOrDefault().ProductName,
            //                    PriceAmount = productReport.Sum(a => a.PriceAmount),
            //                    TransportAmount = productReport.Sum(a => a.TransportAmount)
            //                }).ToList();
            //}
            return reportDataList;
        }
    }
}