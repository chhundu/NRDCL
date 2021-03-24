using Microsoft.AspNetCore.Mvc;

namespace NRDCL.Controllers
{
    public class AccessDeniedController : Controller
    {
        public IActionResult AccessDenied403()
        {
            return View();
        }
    }
}
