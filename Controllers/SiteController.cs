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
    public class SiteController : Controller
    {
        private readonly ISiteService siteService;

        public SiteController(ISiteService service)
        {
            siteService = service;
        }

        // GET: Sites
        public IActionResult Index()
        {
            List<Site> siteList = siteService.GetSiteList();
            ViewBag.Subtitle = "Site Information.";
            return View(siteList);
        }

        // GET: Sites/Details/5
        public IActionResult Details(int siteId)
        {
            if (siteId == 0)
            {
                return NotFound();
            }

            var site = siteService.GetSiteDetails(siteId);
            if (site == null)
            {
                return NotFound();
            }

            return View(site);
        }

        // GET: Sites/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("SiteId,CitizenshipID,SiteName,DistanceFrom")] Site site)
        {
            if (ModelState.IsValid)
            {
                ResponseMessage responseMessage = siteService.SaveSite(site);
                if (responseMessage.Status == false)
                {
                    ModelState.AddModelError(responseMessage.MessageKey, responseMessage.Text);
                    return View(site);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(site);
        }

        // GET: Sites/Edit/5
        public IActionResult Edit(int siteId)
        {
            if (siteId == 0)
            {
                return NotFound();
            }
            var site = siteService.GetSiteDetails(siteId);
            if (site == null)
            {
                return NotFound();
            }
            return View(site);
        }

        // POST: Sites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int siteId, [Bind("SiteId,CitizenshipID,SiteName,DistanceFrom")] Site site)
        {

            if (siteId!=site.SiteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                ResponseMessage responseMessage = siteService.UpdateSite(site);
                if (responseMessage.Status == false)
                {
                    ModelState.AddModelError(responseMessage.MessageKey, responseMessage.Text);
                    return View(site);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(site);
        }
    }
}
