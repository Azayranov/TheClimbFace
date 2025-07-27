using Microsoft.AspNetCore.Mvc;
using TheClimbFace.Services.Data.Interfaces;
using TheClimbFace.Web.ViewModels.Arbitrator;

namespace TheClimbFace.Web.Controllers
{
    public class CompetitionArbitratorController(IArbitratorService arbitratorService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(string idCompetition)
        {
            if (!Guid.TryParse(idCompetition, out Guid competitionId))
                return RedirectToAction(nameof(Index));


            var model = await arbitratorService.GetCompetitionArbitratorsAsync(competitionId);

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Add(ArbitratorViewModel model)
        {

            if (!Guid.TryParse(model.CompetitionId, out Guid competitionId))
                return RedirectToAction(nameof(Index));

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

            await arbitratorService.DeleteArbitratorFromCompetitionAsync(competitionId, userId);

            return RedirectToAction(nameof(Index), new { idCompetition = model.CompetitionId });
        }


    }
}
