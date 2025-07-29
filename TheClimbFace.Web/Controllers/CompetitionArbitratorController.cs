using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheClimbFace.Data.Models;
using TheClimbFace.Services.Data.Interfaces;
using TheClimbFace.Web.ViewModels.Arbitrator;

namespace TheClimbFace.Web.Controllers
{
    [Authorize]
    public class CompetitionArbitratorController(IArbitratorService arbitratorService, ICompetitionService competitionService, UserManager<ApplicationUser> userManager) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(string idCompetition)
        {
            if (!Guid.TryParse(idCompetition, out Guid competitionId))
                return RedirectToAction(nameof(Index));

            var user = await userManager.GetUserAsync(User);
            var compModel = await competitionService.GetCompetitionAsync(competitionId);

            if (!competitionService.IsUserCreator(user!.Id, compModel.ApplicationUserId))
                return RedirectToAction("Index", "Home");

            var model = await arbitratorService.GetCompetitionArbitratorsAsync(competitionId);

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Add(ArbitratorViewModel model)
        {

            if (!Guid.TryParse(model.CompetitionId, out Guid competitionId))
                return RedirectToAction(nameof(Index));

            var user = await userManager.GetUserAsync(User);
            var compModel = await competitionService.GetCompetitionAsync(competitionId);

            if (!competitionService.IsUserCreator(user!.Id, compModel.ApplicationUserId))
                return RedirectToAction("Index", "Home");

            await arbitratorService.AddArbitratorToCompetitionAsync(competitionId, model);

            return RedirectToAction(nameof(Index), new { idCompetition = model.CompetitionId });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(DeleteArbitratorViewModel model)
        {
            if (!Guid.TryParse(model.CompetitionId, out Guid competitionId))
                return RedirectToAction(nameof(Index));

            if (!Guid.TryParse(model.ArbitratorId, out Guid userId))
                return RedirectToAction(nameof(Index));

            var user = await userManager.GetUserAsync(User);
            var compModel = await competitionService.GetCompetitionAsync(competitionId);

            if (!competitionService.IsUserCreator(user!.Id, compModel.ApplicationUserId))
                return RedirectToAction("Index", "Home");

            await arbitratorService.DeleteArbitratorFromCompetitionAsync(competitionId, userId);

            return RedirectToAction(nameof(Index), new { idCompetition = model.CompetitionId });
        }

    }
}
