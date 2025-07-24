using Microsoft.EntityFrameworkCore;
using TheClimbFace.Data.Models;
using TheClimbFace.Data.Repository.Interfaces;
using TheClimbFace.Services.Data.Interfaces;
using TheClimbFace.Web.ViewModels.Home;

namespace TheClimbFace.Services.Data;

public class HomeService(IRepository<ClimbingCompetition> competitionRepository) : IHomeService
{
    public async Task<IEnumerable<HomeViewModel>> GetActiveCompetitionsAsync()
    {
        var activeCompetitions = await competitionRepository.GetAllAttached()
            .Where(x => x.IsActive)
            .ToListAsync();

        List<HomeViewModel> models = new();

        foreach (var competition in activeCompetitions)
        {
            HomeViewModel model = new();
            models.Add(model.ToHomeViewModel(competition));
        }

        return models;
    }

    public Task<CompetitionDetailsViewModel> GetCompetitionDetailsAsync(Guid competitionId)
    {
        throw new NotImplementedException();
    }
}
