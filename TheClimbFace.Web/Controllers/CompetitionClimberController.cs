using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheClimbFace.Data.Models;
using TheClimbFace.Services.Data.Interfaces;
using TheClimbFace.Web.ViewModels.Climber;

namespace TheClimbFace.Web.Controllers
{
    [Authorize]
    public class CompetitionClimberController(IClimberService climberService, ICompetitionService competitionService,
                                  UserManager<ApplicationUser> userManager) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(string Id)
        {
            if (!Guid.TryParse(Id, out Guid competitionId))
                return RedirectToAction(nameof(Index));

            var user = await userManager.GetUserAsync(User);
            var model = await climberService.GetCompetitionClimbersAsync(competitionId);

            if (!competitionService.IsUserCreator(user!.Id, model.ApplicationUserId))
                return RedirectToAction("Index", "Home");

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add(string competitionId)
        {

            if (!Guid.TryParse(competitionId, out Guid id))
                return RedirectToAction(nameof(Index));

            var user = await userManager.GetUserAsync(User);
            var model = await competitionService.GetCompetitionAsync(id);

            if (!competitionService.IsUserCreator(user!.Id, model.ApplicationUserId))
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddClimberInputModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!Guid.TryParse(model.CompetitionId, out Guid id))
                return RedirectToAction(nameof(Index));

            if (!DateTime.TryParse($"{model.BirthDay}/{model.BirthMonth}/{model.BirthYear}", out DateTime birthDate))
            {
                ModelState.AddModelError(string.Empty, "Invalid date format.");
                return View(model);
            }

            await climberService.AddClimberToCompetitionAsync(id, model, birthDate);

            return RedirectToAction(nameof(Index), new { id = model.CompetitionId });
        }

    }
}
