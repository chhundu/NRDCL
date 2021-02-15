using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NRDCL.Data;
using NRDCL.Models;

namespace NRDCL.Controllers
{
    public class DepositController : Controller
    {
        private readonly NRDCL_DB_Context _context;

        public DepositController(NRDCL_DB_Context context)
        {
            _context = context;
        }

        // GET: Deposits
        public async Task<IActionResult> Index()
        {
            return View(await _context.Deposit_Table.ToListAsync());
        }

        // GET: Deposits/Details/5
        public async Task<IActionResult> Details(string customerID)
        {
            if (customerID.Equals(null))
            {
                return NotFound();
            }

            var deposit = await _context.Deposit_Table
                .FirstOrDefaultAsync(m => m.CustomerID.Equals(customerID));
            if (deposit == null)
            {
                return NotFound();
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
        public async Task<IActionResult> Create([Bind("Id,CustomerID,LastAmount,Balance")] Deposit deposit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deposit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deposit);
        }

        // GET: Deposits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deposit = await _context.Deposit_Table.FindAsync(id);
            if (deposit == null)
            {
                return NotFound();
            }
            return View(deposit);
        }

        // POST: Deposits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string customerID, [Bind("Id,CustomerID,LastAmount,Balance")] Deposit deposit)
        {
            if (customerID.Equals(deposit.CustomerID))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deposit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepositExists(deposit.CustomerID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(deposit);
        }

        // GET: Deposits/Delete/5
        public async Task<IActionResult> Delete(string customerID)
        {
            if (customerID.Equals(null))
            {
                return NotFound();
            }

            var deposit = await _context.Deposit_Table
                .FirstOrDefaultAsync(m => m.CustomerID.Equals(customerID));
            if (deposit == null)
            {
                return NotFound();
            }

            return View(deposit);
        }

        // POST: Deposits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deposit = await _context.Deposit_Table.FindAsync(id);
            _context.Deposit_Table.Remove(deposit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepositExists(string customerID)
        {
            return _context.Deposit_Table.Any(e => e.CustomerID.Equals(customerID));
        }
    }
}
