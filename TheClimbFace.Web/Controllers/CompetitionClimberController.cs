using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheClimbFace.Data.Models;
using TheClimbFace.Services.Data.Interfaces;

namespace TheClimbFace.Web.Controllers
{
    [Authorize]
    public class CompetitionClimbersController(IClimberService climberService, ICompetitionService competitionService,
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

        

    }
}
