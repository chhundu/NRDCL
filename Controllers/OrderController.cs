using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NRDCL.Models;
using NRDCL.Models.Common;

namespace NRDCL.Controllers
{
    [Authorize(Roles ="Admin")]
    public class OrderController : Controller
    {
        #region private variables
        private readonly ISiteService siteService;
        private readonly IOrderService orderService;
        private readonly IProductService productService;
        #endregion

        #region Constructor Injection
        public OrderController(ISiteService service, IOrderService order, IProductService product)
        {
            siteService = service;
            orderService = order;
            productService = product;
        }
        #endregion
        #region public methods
        /// <summary>
        /// Get order detail
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int orderId)
        {
            if (orderId == 0)
            {
                return new NotFoundResult();
            }
            var order = await orderService.GetOrderDetails(orderId);
            if (order == null)
            {
                return new NotFoundResult();
            }
            return View(order);
        }

        /// <summary>
        /// Create order
        /// </summary>
        /// <returns></returns>
        // GET: Orders/Create
        public IActionResult Create()
        {
            Task<List<Order>> orderList = orderService.GetOrderList();
            ViewBag.Orders = orderList.Result;
            var order = new Order();
            List<SelectListItem> siteList = new List<SelectListItem>()
                {
                new SelectListItem
                {
                    Value = null,
                    Text = " "
                }
            };
            order.SiteList = siteList;

            var productList = productService.GetProductList();
            ViewData["ProductList"] = new SelectList(productList.Result, "ProductId", "ProductName");
            return View(order);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("OrderID,CustomerID,SiteID,ProductID,Quantity,OrderAmount")] Order order)
        {
            Task<List<Order>> orderList = orderService.GetOrderList();
            ViewBag.Orders = orderList.Result;
            IEnumerable<SelectListItem> siteList = siteService.GetSiteList().Result.Where(s => s.CitizenshipID.Equals(order.CustomerID)).Select(s => new SelectListItem()
            {
                Value = s.SiteId.ToString(),
                Text = s.SiteName
            }).ToList();
            order.SiteList = siteList;

            var productList = productService.GetProductList();
            ViewData["ProductList"] = new SelectList(productList.Result, "ProductId", "ProductName");
            if (ModelState.IsValid)
            {
                ResponseMessage responseMessage = orderService.SaveOrder(order);
                if (responseMessage.Status == false)
                {
                    ModelState.AddModelError(responseMessage.MessageKey, responseMessage.Text);
                    return View(order);
                }
                ViewBag.Result = responseMessage.Text;
                ModelState.Clear();
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int orderId)
        {
            if (orderId == 0)
            {
                return new NotFoundResult();
            }
            var order = await orderService.GetOrderDetails(orderId);
            if (order == null)
            {
                return new NotFoundResult();
            }
            IEnumerable<SelectListItem> siteList = siteService.GetSiteList().Result.Where(s => s.CitizenshipID.Equals(order.CustomerID)).Select(s => new SelectListItem()
            {
                Value = s.SiteId.ToString(),
                Text = s.SiteName
            }).ToList();
            order.SiteList = siteList;

            var productList = productService.GetProductList();
            ViewData["ProductList"] = new SelectList(productList.Result, "ProductId", "ProductName");
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Edit(int orderId, [Bind("OrderID,CustomerID,SiteID,ProductID,Quantity,OrderAmount")] Order order)
        {
            IEnumerable<SelectListItem> siteList = siteService.GetSiteList().Result.Where(s => s.CitizenshipID.Equals(order.CustomerID)).Select(s => new SelectListItem()
            {
                Value = s.SiteId.ToString(),
                Text = s.SiteName
            }).ToList();
            order.SiteList = siteList;

            var productList = productService.GetProductList();
            ViewData["ProductList"] = new SelectList(productList.Result, "ProductId", "ProductName");
            if (orderId != order.OrderID)
            {
                return new NotFoundResult();
            }
            if (ModelState.IsValid)
            {
                ResponseMessage responseMessage = orderService.SaveOrder(order);
                if (responseMessage.Status == false)
                {
                    ModelState.AddModelError(responseMessage.MessageKey, responseMessage.Text);
                    return View(order);
                }
                ViewBag.Result = responseMessage.Text;
                ModelState.Clear();
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int OrderID)
        {
            if (OrderID == 0)
            {
                return new NotFoundResult();
            }
            var order = await orderService.GetOrderDetails(OrderID);
            if (order == null)
            {
                return new NotFoundResult();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int OrderID)
        {
            var responseMessage = await orderService.DeleteOrder(OrderID);
            return RedirectToAction(nameof(Create));
        }

        /// <summary>
        /// Get site dropdown based on customerID
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSiteByCustomerId(string customerID)
        {
            IEnumerable<SelectListItem> siteList= siteService.GetSiteList().Result.Where(s => s.CitizenshipID.Equals(customerID)).Select(s => new SelectListItem()
            {
                Value = s.SiteId.ToString(),
                Text = s.SiteName
            }).ToList();
            //var siteList = 
            var filteredSiteList = new SelectList(siteList, "Value", "Text");
            return Json(await Task.Run(()=> filteredSiteList));
            //return Json(filteredSiteList);
        }

        /// <summary>
        /// Calculates order amount
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CalculateOrderAmount(Order order)
        {
            Task<decimal> orderAmount= orderService.CalculateOrderAmount(order);
            return Json(await orderAmount);
        }
        #endregion
    }
}
