using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheClimbFace.Models;
using TheClimbFace.Services.Data.Interfaces;
using TheClimbFace.Web.ViewModels.Home;

namespace TheClimbFace.Controllers;

public class HomeController(IHomeService homeService, ILogger<HomeController> logger) : Controller
{
    public async Task<IActionResult> Index()
    {
        IEnumerable<HomeViewModel> activeCompetitions = await homeService.GetActiveCompetitionsAsync();

        return View(activeCompetitions);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
