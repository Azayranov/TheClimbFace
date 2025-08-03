using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheClimbFace.Data.Models;
using TheClimbFace.Services.Data.Interfaces;
using TheClimbFace.Web.ViewModels.Arbitrator;

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

        [HttpPost]
        public async Task<IActionResult> AddClimber(ScoreViewModel model)
        {
            if (!Guid.TryParse(model.CompetitionId, out Guid competitionId))
                return RedirectToAction(nameof(Index), "Home");

            Guid userId = Guid.Parse(userManager.GetUserId(User)!);

            var updatedModel = await boulderScoringService.GetScoreViewModelWithClimberAsync(competitionId, userId, model.StartNumber, model.BoulderNumber);

            return View(nameof(Score), updatedModel);
        }

        [HttpPost]
        public IActionResult RemoveClimber(ScoreViewModel model)
        {
            model.CurrentClimber = null!;
            return View(nameof(Score), model);
        }


    }
}
