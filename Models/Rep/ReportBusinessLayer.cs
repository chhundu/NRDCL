using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NRDCL.Models.Report
{
    public class ReportBusinessLayer
    {
        //public NRDCL_DB_Entities dbContext = new NRDCL_DB_Entities();

        public List<Report> GetReportData(int reportNumber)
        {
            var reportDataList = new List<Report>();

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