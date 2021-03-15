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
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        public ProductController(IProductService service)
        {
            productService = service;
        }

        // GET: Products
        public IActionResult Index()
        {
            List<Product> productList = productService.GetProductList();
            ViewBag.Subtitle = "Product Information.";
            return View(productList);
        }

        // GET: Products/Details/5
        public  IActionResult Details(int productID)
        {
            if (productID == 0)
            {
                return NotFound();
            }
            var product = productService.GetProductDetails(productID);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ProductId,ProductName,PricePerUnit,TransportRate")] Product product)
        {
            if (ModelState.IsValid)
            {
                ResponseMessage responseMessage = productService.SaveProduct(product);
                if (responseMessage.Status == false)
                {
                    ModelState.AddModelError(responseMessage.MessageKey, responseMessage.Text);
                    return View(product);
                }

                ViewBag.Result = CommonProperties.saveSuccessMsg;
                ModelState.Clear();
                product = new Product();
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public IActionResult Edit(int productId)
        {
            if (productId == 0)
            {
               return new NotFoundResult();
            }

            var product = productService.GetProductDetails(productId);
            if (product == null)
            {
                return new NotFoundResult();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int productId, [Bind("ProductId,ProductName,PricePerUnit,TransportRate")] Product product)
        {
            if (productId != product.ProductId)
            {
                return new NotFoundResult();
            }

            if (ModelState.IsValid)
            {
                ResponseMessage responseMessage = productService.UpdateProduct(product);
                if (responseMessage.Status == false)
                {
                    ModelState.AddModelError(responseMessage.MessageKey, responseMessage.Text);
                    return View(product);
                }
                ViewBag.Result = CommonProperties.updateSuccessMsg;
                ModelState.Clear();
                product = new Product();
            }
            return View(product);
        }
    }
}
