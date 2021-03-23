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
            return RedirectToAction("Create", new { CitizenshipID });
        }

        // GET: Customers/Create
        public IActionResult Create(string? CitizenshipID)
        {
            Customer customer = null;
            if (!string.IsNullOrEmpty(CitizenshipID)) {
                 customer = customerService.GetCustomerDetails(CitizenshipID).Result;
                customer.CMDstatus = "M";
            }
            var customerList = customerService.GetCustomerList();
            ViewBag.Customers = customerList.Result;
            return View(customer);
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<IActionResult> Create([Bind("CMDstatus,CitizenshipID,CustomerName,TelephoneNumber,EmailId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                ResponseMessage responseMessage = null; 
                if (!string.IsNullOrEmpty(customer.CMDstatus) && customer.CMDstatus.Equals("M"))
                {
                    responseMessage =  await customerService.UpdateCustomer(customer);
                }
                else {
                    responseMessage = await customerService.SaveCustomer(customer);
                }

                var customerList = customerService.GetCustomerList().Result;
                ViewBag.Customers = customerList;
                if (responseMessage.Status == false)
                {
                    ModelState.AddModelError(responseMessage.MessageKey, responseMessage.Text);
                    return View(customer);
                }
                ViewBag.Result = responseMessage.Text;
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
            return RedirectToAction("Create", new {CitizenshipID});
        }
    }
}
