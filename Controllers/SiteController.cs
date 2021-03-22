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

        // GET: Sites
        public async Task<IActionResult> Index()
        {
            var siteList = await siteService.GetSiteList();
            ViewBag.Subtitle = "Site Information.";
            return View(siteList);
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SiteId,CitizenshipID,SiteName,DistanceFrom")] Site site)
        {
            if (ModelState.IsValid)
            {
                Task<ResponseMessage> responseMessage = siteService.SaveSite(site);
                if (responseMessage.Result.Status == false)
                {
                    ModelState.AddModelError(responseMessage.Result.MessageKey, responseMessage.Result.Text);
                    return View(site);
                }
                ViewBag.Result = CommonProperties.saveSuccessMsg;
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
            return View(site);
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
