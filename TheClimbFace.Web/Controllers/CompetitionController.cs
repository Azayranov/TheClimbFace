using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheClimbFace.Data.Models;
using TheClimbFace.Services.Data.Interfaces;
using TheClimbFace.Web.ViewModels.Competition;

namespace TheClimbFace.Web.Controllers
{
    [Authorize]
    public class CompetitionController(ICompetitionService competitionService, UserManager<ApplicationUser> userManager) : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Guid userId = Guid.Parse(userManager.GetUserId(User)!);
            var model = await competitionService.GetUserCompetitionsAsync(userId);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCompetitionInputModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!DateTime.TryParse($"{model.StartDay}/{model.StartMonth}/{model.StartYear}", out DateTime StartDate)
                || !DateTime.TryParse($"{model.EndDay}/{model.EndMonth}/{model.EndYear}", out DateTime EndDate))
            {
                ModelState.AddModelError(string.Empty, "Invalid date format.");
                return View(model);
            }

            Guid userId = Guid.Parse(userManager.GetUserId(User)!);
            await competitionService.CreateCompetitionAsync(model, StartDate, EndDate, userId);

            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Details(string Id)
        {

            if (!Guid.TryParse(Id, out Guid competitionId))
                return RedirectToAction(nameof(Index));

            var user = await userManager.GetUserAsync(User);
            var model = await competitionService.GetCompetitionDetailsAsync(competitionId);

            if (!competitionService.IsUserCreator(model.ApplicationUserId, user!.Id))
                return RedirectToAction(nameof(Index));

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {

            if (!Guid.TryParse(Id, out Guid competitionId))
                return RedirectToAction(nameof(Index));

            var user = await userManager.GetUserAsync(User);
            var model = await competitionService.GetCompetitionAsync(competitionId);

            if (!competitionService.IsUserCreator(model.ApplicationUserId, user!.Id))
                return RedirectToAction(nameof(Index));

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreateCompetitionInputModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!Guid.TryParse(model.Id, out Guid competitionId))
                return RedirectToAction(nameof(Index));

            if (!DateTime.TryParse($"{model.StartDay}/{model.StartMonth}/{model.StartYear}", out DateTime StartDate)
                || !DateTime.TryParse($"{model.EndDay}/{model.EndMonth}/{model.EndYear}", out DateTime EndDate))
            {
                ModelState.AddModelError(string.Empty, "Invalid date format.");
                return View(model);
            }

            await competitionService.EditCompetitionAsync(model, StartDate, EndDate, competitionId);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid competitionId))
                return RedirectToAction(nameof(Index));

            var user = await userManager.GetUserAsync(User);
            var competition = await competitionService.GetCompetitionAsync(competitionId);

            if (!competitionService.IsUserCreator(competition.ApplicationUserId, user!.Id))
                return RedirectToAction(nameof(Index));

            await competitionService.DeleteCompetitionAsync(competitionId);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Start(string idCompetition)
        {
            if (!Guid.TryParse(idCompetition, out Guid competitionId))
                return RedirectToAction(nameof(Index));

            var user = await userManager.GetUserAsync(User);
            var competition = await competitionService.GetCompetitionAsync(competitionId);

            if (!competitionService.IsUserCreator(competition.ApplicationUserId, user!.Id))
                return RedirectToAction(nameof(Index));

            await competitionService.StartCompetitionAsync(competitionId);


            return RedirectToAction(nameof(Details), new { Id =competitionId});
        }

        [HttpGet]
        public async Task<IActionResult> Stop(string idCompetition)
        {
            if (!Guid.TryParse(idCompetition, out Guid competitionId))
                return RedirectToAction(nameof(Index));

            var user = await userManager.GetUserAsync(User);
            var competition = await competitionService.GetCompetitionAsync(competitionId);

            if (!competitionService.IsUserCreator(competition.ApplicationUserId, user!.Id))
                return RedirectToAction(nameof(Index));

            await competitionService.StopCompetitionAsync(competitionId);

            return RedirectToAction(nameof(Details), new { Id = idCompetition });
        }

        
    }
}
