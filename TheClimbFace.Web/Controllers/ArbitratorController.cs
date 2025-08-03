using Microsoft.AspNetCore.Mvc;

namespace TheClimbFace.Web.Controllers
{
    public class ArbitratorController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Competition");
        }

    }
}
