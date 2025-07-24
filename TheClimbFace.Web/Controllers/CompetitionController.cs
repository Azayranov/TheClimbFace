using Microsoft.AspNetCore.Mvc;

namespace TheClimbFace.Web.Controllers
{
    public class CompetitionController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public  IActionResult Create()
        {
            return View();
        }

    }
}
