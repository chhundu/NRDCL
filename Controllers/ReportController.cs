using NRDCL.Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace NRDCL.Controllers
{
    public class ReportController : Controller
    {
        public ReportBusinessLayer reportBL = new ReportBusinessLayer();
        public int customerReportNo = 1;
        public int productReportNo = 2;
        // GET: Report
        public IActionResult Index()
        {
            List<Report> customerReportData = reportBL.GetReportData(customerReportNo);
            List<Report> productReportData = reportBL.GetReportData(productReportNo);
            ViewBag.Subtitle = "Report";
            ViewBag.Customers = customerReportData;
            ViewBag.Products = productReportData;
            return View();
        }
    }
}