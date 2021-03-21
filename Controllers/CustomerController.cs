using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NRDCL.Models;
using NRDCL.Models.Common;

namespace NRDCL.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerService customerService;
        public CustomerController(ICustomerService service)
        {
            customerService = service;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var customerList = await customerService.GetCustomerList();
            ViewBag.Subtitle = "Customer Information.";
            TempData["Status"] = false;
            return View(customerList);
        }


        // GET: Customers/Details/5
        public async Task<IActionResult> Details(string CitizenshipID)
        {
            if (CitizenshipID.Equals(null))
            {
                return new NotFoundResult();
            }
            var customer = await customerService.GetCustomerDetails(CitizenshipID);
            if (customer == null)
            {
                return new NotFoundResult();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<IActionResult> Create([Bind("Id,CitizenshipID,CustomerName,TelephoneNumber,EmailId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                Task<ResponseMessage> responseMessage = customerService.SaveCustomer(customer);
                if (responseMessage.Result.Status == false)
                {
                    ModelState.AddModelError(responseMessage.Result.MessageKey, responseMessage.Result.Text);
                    return View(customer);
                }
                ViewBag.Result = CommonProperties.saveSuccessMsg;
                ModelState.Clear();
                 customer = new Customer();
            }
            return View(await Task.FromResult(customer));
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(string CitizenshipID)
        {
            if (CitizenshipID.Equals(null))
            {
                return new NotFoundResult();
            }
            var customer = await customerService.GetCustomerDetails(CitizenshipID);
            if (customer == null)
            {
                return new NotFoundResult();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string CitizenshipID, [Bind("CitizenshipID,CustomerName,TelephoneNumber,EmailId")] Customer customer)
        {
            if (!CitizenshipID.Equals(customer.CitizenshipID))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                Task<ResponseMessage> responseMessage = customerService.UpdateCustomer(customer);
                if (responseMessage.Result.Status == false)
                {
                    ModelState.AddModelError(responseMessage.Result.MessageKey, responseMessage.Result.Text);
                    return View(customer);
                }

                ViewBag.Result = CommonProperties.updateSuccessMsg;
                ModelState.Clear();
                customer = new Customer();
            }
            return View(await Task.FromResult(customer));
        }
    }
}
