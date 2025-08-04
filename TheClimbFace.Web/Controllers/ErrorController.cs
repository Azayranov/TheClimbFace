using Microsoft.AspNetCore.Mvc;

namespace TheClimbFace.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/404")]
        public IActionResult Error404()
        {
            return View("404");
        }

        [Route("Error/500")]
        public IActionResult Error500()
        {
            return View("500");
        }


        [Route("Error/{code}")]
        public IActionResult General(int code)
        {
            return View("General", code);
        }
    }
}
