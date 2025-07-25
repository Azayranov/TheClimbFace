using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheClimbFace.Data.Models;
using TheClimbFace.Services.Data.Interfaces;
using TheClimbFace.Web.ViewModels.Competition;

namespace TheClimbFace.Web.Controllers
{
    public class CompetitionController(ICompetitionService competitionService, UserManager<ApplicationUser> userManager) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Guid userId = Guid.Parse(userManager.GetUserId(User)!);
            var model = await competitionService.GetUserCompetitionsAsync(userId);
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
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

            var model = await competitionService.GetCompetitionDetailsAsync(competitionId);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            if (!Guid.TryParse(Id, out Guid competitionId))
                return RedirectToAction(nameof(Index));

            var model = await competitionService.GetCompetitionAsync(competitionId);

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
    }
}
