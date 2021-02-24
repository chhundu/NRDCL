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
    public class CustomerController : Controller
    {
        private readonly ICustomerService customerService;
        public CustomerController(ICustomerService service)
        {
            customerService = service;
        }

        // GET: Customers
        public IActionResult Index()
        {
            List<Customer> customerList = customerService.GetCustomerList();
            ViewBag.Subtitle = "Customer Information.";
            TempData["Status"] = false;
            return View(customerList);
        }

        // GET: Customers/Details/5
        public IActionResult Details(string CitizenshipID)
        {
            if (CitizenshipID.Equals(null))
            {
                return new NotFoundResult();
            }
            var customer = customerService.GetCustomerDetails(CitizenshipID);
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
        public  IActionResult Create([Bind("Id,CitizenshipID,CustomerName,TelephoneNumber,EmailId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                ResponseMessage responseMessage = customerService.SaveCustomer(customer);
                if (responseMessage.Status == false)
                {
                    ModelState.AddModelError(responseMessage.MessageKey, responseMessage.Text);
                    return View(customer);
                }
                TempData["Status"] = true;
                TempData["Text"] = responseMessage.Text;
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public IActionResult Edit(string CitizenshipID)
        {
            if (CitizenshipID.Equals(null))
            {
                return new NotFoundResult();
            }

            var customer = customerService.GetCustomerDetails(CitizenshipID);
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
        public IActionResult Edit(string CitizenshipID, [Bind("CitizenshipID,CustomerName,TelephoneNumber,EmailId")] Customer customer)
        {
            if (!CitizenshipID.Equals(customer.CitizenshipID))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                ResponseMessage responseMessage = customerService.UpdateCustomer(customer);
                if (responseMessage.Status == false)
                {
                    ModelState.AddModelError(responseMessage.MessageKey, responseMessage.Text);
                    return View(customer);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }
    }
}
