using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        // GET: Sites/Details/5
        public async Task<IActionResult> Details(int siteId)
        {
            if (siteId == 0)
            {
                return NotFound();
            }
            var site = await siteService.GetSiteDetails(siteId);
            if (site == null)
            {
                return NotFound();
            }

            return View(site);
        }

        // GET: Sites/Create
        public IActionResult Create(int? siteId)
        {
            Site site = null;
            if (siteId!=null && siteId != 0) {
                site = siteService.GetSiteDetails((int)siteId).Result;
                site.CMDstatus = "M";
            }
            var siteList = siteService.GetSiteList();
            ViewBag.Sites = siteList.Result;
            return View(site);
        }

        // POST: Sites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CMDstatus,SiteId,CitizenshipID,SiteName,DistanceFrom")] Site site)
        {
            if (ModelState.IsValid)
            {
                var siteList = siteService.GetSiteList();
                ViewBag.Sites = siteList.Result;
                Task<ResponseMessage> responseMessage = null;
                if (!string.IsNullOrEmpty(site.CMDstatus) && site.CMDstatus.Equals("M"))
                {
                    responseMessage = siteService.UpdateSite(site);
                }
                else
                {
                    responseMessage = siteService.SaveSite(site);
                }
                if (responseMessage.Result.Status == false)
                {
                    ModelState.AddModelError(responseMessage.Result.MessageKey, responseMessage.Result.Text);
                    return View(site);
                }
                ViewBag.Result = responseMessage.Result.Text;
                ModelState.Clear();
                site = new Site();
            }
            
            return View(await Task.FromResult(site));
        }

        // GET: Sites/Edit/5
        public async Task<IActionResult> Edit(int siteId)
        {
            if (siteId == 0)
            {
                return NotFound();
            }
            var site = await siteService.GetSiteDetails(siteId);
            if (site == null)
            {
                return NotFound();
            }
            return RedirectToAction("Create", new { siteId });
        }

        // POST: Sites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int siteId, [Bind("SiteId,CitizenshipID,SiteName,DistanceFrom")] Site site)
        {

            if (siteId!=site.SiteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                Task<ResponseMessage> responseMessage = siteService.UpdateSite(site);
                if (responseMessage.Result.Status == false)
                {
                    ModelState.AddModelError(responseMessage.Result.MessageKey, responseMessage.Result.Text);
                    return View(site);
                }

                ViewBag.Result = CommonProperties.updateSuccessMsg;
                ModelState.Clear();
                site = new Site();
            }
            return View(await Task.FromResult(site));
        }
    }
}
