using NRDCL.Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using NRDCL.Models;

namespace NRDCL.Controllers
{
    public class ReportController : Controller
    {
        private readonly ICustomerService customerService;
        public ReportController(ICustomerService service)
        {
            customerService = service;
        }
        public int customerReportNo = 1;
        public int productReportNo = 2;
        // GET: Report
        public IActionResult Index()
        {
            List<Report> customerReportData = customerService.GetReportData(customerReportNo);
            List<Report> productReportData = customerService.GetReportData(productReportNo);
            ViewBag.Subtitle = "Report";
            ViewBag.Customers = customerReportData;
            ViewBag.Products = productReportData;
            return View();
        }
    }
}