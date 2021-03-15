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
    public class DepositController : Controller
    {
        private readonly NRDCL_DB_Context _context;
        private readonly IDepositService  depositService;
        

        /*public DepositController(NRDCL_DB_Context context)
        {
            _context = context;
        }*/
        public DepositController(IDepositService service)
        {
            depositService = service;
        }

        // GET: Deposits
        public IActionResult Index()
        {
            List<Deposit> depositList = depositService.GetDepositList();
            ViewBag.Subtitle = "Deposit Information.";
            return View(depositList);
        }

        // GET: Deposits/Details/5
        public IActionResult Details(string customerID)
        {
            if (customerID.Equals(null))
            {
                return new NotFoundResult();
            }
            var deposit = depositService.GetDepositDetails(customerID);
            if (deposit == null)
            {
                return new NotFoundResult();
            }
            return View(deposit);
        }

        // GET: Deposits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Deposits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CustomerID,LastAmount")] Deposit deposit)
        {
            if (ModelState.IsValid)
            {
                ResponseMessage responseMessage = depositService.SaveDeposit(deposit);
                if (responseMessage.Status == false)
                {
                    ModelState.AddModelError(responseMessage.MessageKey, responseMessage.Text);
                    return View(deposit);
                }
                ViewBag.Result = CommonProperties.saveSuccessMsg;
                ModelState.Clear();
                deposit = new Deposit();

                //return RedirectToAction(nameof(Index));
            }
            return View(deposit);
        }

    }
}
