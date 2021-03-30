using NRDCL.Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using NRDCL.Models;
using SelectPdf;

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

        public IActionResult GeneratePdf(string html)
        {
            html = html.Replace("StrTag", "<").Replace("EndTag", ">");
            HtmlToPdf oHtmlTpPdf = new HtmlToPdf();
            PdfDocument oPdfDocument = oHtmlTpPdf.ConvertHtmlString(html);
            byte[] pdf = oPdfDocument.Save();
            oPdfDocument.Close();

            return File(pdf, "application/pdf", "Customer&ProductReports.pdf");
        }
    }
}