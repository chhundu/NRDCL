using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NRDCL.Data;
using NRDCL.Models;
using NRDCL.Models.Common;

namespace NRDCL.Controllers
{
    public class OrderController : Controller
    {
        private readonly NRDCL_DB_Context _context;
        private readonly ISiteService siteService;
        private readonly IOrderService orderService;
        private readonly IProductService productService;
        public OrderController(ISiteService service, IOrderService order, IProductService product)
        {
            siteService = service;
            orderService = order;
            productService = product;
        }

        // GET: Orders
        public IActionResult Index()
        {
            List<Order> orderList = orderService.GetOrderList();
            ViewBag.Subtitle = "Order Information.";
            return View(orderList);
        }

        // GET: Orders/Details/5
        public IActionResult Details(int orderId)
        {
            if (orderId == 0)
            {
                return new NotFoundResult();
            }
            var order = orderService.GetOrderDetails(orderId);
            if (order == null)
            {
                return new NotFoundResult();
            }
            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            List<Site> siteList = siteService.GetSiteList();
            ViewData["SiteList"] = new SelectList(siteList, "SiteId", "SiteName");

            List<Product> productList = productService.GetProductList();
            ViewData["ProductList"] = new SelectList(productList, "ProductId", "ProductName");

            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("OrderID,CustomerID,SiteID,ProductID,Quantity,OrderAmount")] Order order)
        {
            if (ModelState.IsValid)
            {
                ResponseMessage responseMessage = orderService.SaveOrder(order);
                if (responseMessage.Status == false)
                {
                    ModelState.AddModelError(responseMessage.MessageKey, responseMessage.Text);
                    List<Site> siteList = siteService.GetSiteList();
                    ViewData["SiteList"] = new SelectList(siteList, "SiteId", "SiteName");

                    List<Product> productList = productService.GetProductList();
                    ViewData["ProductList"] = new SelectList(productList, "ProductId", "ProductName");
                    return View(order);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public IActionResult Edit(int orderId)
        {
            if (orderId == 0)
            {
                return new NotFoundResult();
            }

            var order = orderService.GetOrderDetails(orderId);
            if (order == null)
            {
                return new NotFoundResult();
            }
            List<Site> siteList = siteService.GetSiteList();
            ViewData["SiteList"] = new SelectList(siteList, "SiteId", "SiteName");

            List<Product> productList = productService.GetProductList();
            ViewData["ProductList"] = new SelectList(productList, "ProductId", "ProductName");
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Edit(int orderId, [Bind("OrderID,CustomerID,SiteID,ProductID,Quantity,OrderAmount")] Order order)
        {
            if (orderId != order.OrderID)
            {
                return new NotFoundResult();
            }

            if (ModelState.IsValid)
            {

                ResponseMessage responseMessage = orderService.UpdateOrder(order);
                if (responseMessage.Status == false)
                {
                    ModelState.AddModelError(responseMessage.MessageKey, responseMessage.Text);
                    return View(order);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public IActionResult Delete(int OrderID)
        {
            if (OrderID == 0)
            {
                return new NotFoundResult();
            }
            var order = orderService.GetOrderDetails(OrderID);
            if (order == null)
            {
                return new NotFoundResult();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int OrderID)
        {
            var responseMessage = orderService.DeleteOrder(OrderID);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult GetSiteByCustomerId(string customerID)
        {
            var siteList = siteService.GetSiteList().Where(s => s.CitizenshipID.Equals(customerID)).Select(siteDropdown=> new Site() { 
            SiteId=siteDropdown.SiteId,
            SiteName=siteDropdown.SiteName
            }).ToList();
            var filteredSiteList = new SelectList(siteList, "SiteId", "SiteName");
            return Json(filteredSiteList);
        }

        [HttpGet]
        public ActionResult CalculateOrderAmount(Order order)
        {
            decimal orderAmount=orderService.CalculateOrderAmount(order);
            return Json(orderAmount);
        }
    }
}
