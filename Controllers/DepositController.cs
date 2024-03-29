﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NRDCL.Models;
using NRDCL.Models.Common;

namespace NRDCL.Controllers
{
    public class DepositController : Controller
    {
        private readonly IDepositService  depositService;
        public DepositController(IDepositService service)
        {
            depositService = service;
        }

        // GET: Deposits/Details/5
        public async Task<IActionResult> Details(string customerID)
        {
            if (customerID.Equals(null))
            {
                return new NotFoundResult();
            }
            var deposit = await depositService.GetDepositDetails(customerID);
            if (deposit == null)
            {
                return new NotFoundResult();
            }
            return View(deposit);
        }

        // GET: Deposits/Create
        public IActionResult Create()
        {
            var depositList =  depositService.GetDepositList();
            ViewBag.Deposits = depositList.Result;
            return View();
        }

        // POST: Deposits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerID,LastAmount")] Deposit deposit)
        {
            if (ModelState.IsValid)
            {
                Task<ResponseMessage> responseMessage = depositService.SaveDeposit(deposit);
                if (responseMessage.Result.Status == false)
                {
                    ModelState.AddModelError(responseMessage.Result.MessageKey, responseMessage.Result.Text);
                    return View(deposit);
                }
                ViewBag.Result = CommonProperties.saveSuccessMsg;
                ModelState.Clear();
                deposit = new Deposit();
            }
            var depositList = depositService.GetDepositList();
            ViewBag.Deposits = depositList.Result;
            return View(await Task.FromResult(deposit));
        }
    }
}
