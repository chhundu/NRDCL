using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NRDCL.Models;
using NRDCL.Models.Report;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NRDCL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService productService;

        public HomeController(ILogger<HomeController> logger, IProductService PService)
        {
            _logger = logger;
            productService = PService;
        }

        public IActionResult Index()
        {
            List<Report> productPieChart = productService.GetProductPieChartData();
            return View(productPieChart);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
