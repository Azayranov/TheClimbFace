using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheClimbFace.Models;
using TheClimbFace.Services.Data.Interfaces;
using TheClimbFace.Web.ViewModels.Home;

namespace TheClimbFace.Controllers;

public class HomeController(IHomeService homeService, ILogger<HomeController> logger) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        IEnumerable<HomeViewModel> activeCompetitions = await homeService.GetActiveCompetitionsAsync();

        return View(activeCompetitions);
    }

    [HttpGet]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public async Task<IActionResult> Details(string id)
    {
        if (!Guid.TryParse(id, out Guid competitionId))
            return RedirectToAction(nameof(Index));

        CompetitionDetailsViewModel model = await homeService.GetCompetitionDetailsAsync(competitionId);

        return View(model);
    }
}
