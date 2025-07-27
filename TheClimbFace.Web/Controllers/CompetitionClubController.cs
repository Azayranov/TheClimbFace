using Microsoft.AspNetCore.Mvc;
using TheClimbFace.Services.Data.Interfaces;

namespace TheClimbFace.Web.Controllers
{
    public class CompetitionClubController(IClubService clubService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(string Id)
        {
            if (!Guid.TryParse(Id, out Guid competitionId))
                return RedirectToAction(nameof(Index));

            var model = await clubService.GetCompetitionClubsAsync(competitionId);

            return View(model);
        }

    }
}
