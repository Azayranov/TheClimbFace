using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheClimbFace.Data.Models;
using TheClimbFace.Services.Data.Interfaces;

namespace TheClimbFace.Web.Controllers
{
    [Authorize]
    public class ArbitratorController(IBoulderScoringService boulderScoringService, UserManager<ApplicationUser> userManager) : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Competition");
        }

        [HttpGet]
        public async Task<IActionResult> Score(string id, int boulderNumber)
        {
            if (!Guid.TryParse(id, out Guid CompetitionId))
                return RedirectToAction(nameof(Index), "Home");

            Guid userId = Guid.Parse(userManager.GetUserId(User)!);

            var model = await boulderScoringService.GetScoreViewModelAsync(CompetitionId, userId, boulderNumber);

            return View(model);
        }
        

    }
}
