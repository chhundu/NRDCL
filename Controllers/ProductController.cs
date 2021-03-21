using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Index()
        {
            var productList = await productService.GetProductList();
            ViewBag.Subtitle = "Product Information.";
            return View(productList);
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,PricePerUnit,TransportRate")] Product product)
        {
            if (ModelState.IsValid)
            {
                Task<ResponseMessage> responseMessage = productService.SaveProduct(product);
                if (responseMessage.Result.Status == false)
                {
                    ModelState.AddModelError(responseMessage.Result.MessageKey, responseMessage.Result.Text);
                    return View(product);
                }

                ViewBag.Result = CommonProperties.saveSuccessMsg;
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
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int productId, [Bind("ProductId,ProductName,PricePerUnit,TransportRate")] Product product)
        {
            if (productId != product.ProductId)
            {
                return new NotFoundResult();
            }

            if (ModelState.IsValid)
            {
                Task<ResponseMessage> responseMessage = productService.UpdateProduct(product);
                if (responseMessage.Result.Status == false)
                {
                    ModelState.AddModelError(responseMessage.Result.MessageKey, responseMessage.Result.Text);
                    return View(product);
                }
                ViewBag.Result = CommonProperties.updateSuccessMsg;
                ModelState.Clear();
                product = new Product();
            }
            return View(await Task.FromResult(product));
        }
    }
}
