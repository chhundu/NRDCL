using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NRDCL.Models;
using NRDCL.Models.Common;

namespace NRDCL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        public ProductController(IProductService service)
        {
            productService = service;
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int productID)
        {
            if (productID == 0)
            {
                return NotFound();
            }
            var product = await productService.GetProductDetails(productID);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create(int? productId)
        {
            Product product = null;
            if (productId != null && productId!=0)
            {
                product = productService.GetProductDetails((int)productId).Result;
                product.CMDstatus = "M";
            }
            var productList =  productService.GetProductList();
            ViewBag.Products = productList.Result;
            return View(product);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CMDstatus,ProductId,ProductName,PricePerUnit,TransportRate")] Product product)
        {
            if (ModelState.IsValid)
            {

                ResponseMessage responseMessage = null;
                if (!string.IsNullOrEmpty(product.CMDstatus) && product.CMDstatus.Equals("M"))
                {
                    responseMessage = await productService.UpdateProduct(product);
                }
                else
                {
                    responseMessage = await productService.SaveProduct(product);
                }
                var productList = productService.GetProductList();
                ViewBag.Products = productList.Result;
                if (responseMessage.Status == false)
                {
                    ModelState.AddModelError(responseMessage.MessageKey, responseMessage.Text);
                    return View(product);
                }
                ViewBag.Result = responseMessage.Text;
                ModelState.Clear();
                product = new Product();
            }
            return View(await Task.FromResult(product));
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int productId)
        {
            if (productId == 0)
            {
               return new NotFoundResult();
            }
            var product = await productService.GetProductDetails(productId);
            if (product == null)
            {
                return new NotFoundResult();
            }
            return RedirectToAction("Create", new { productId });
        }
    }
}
