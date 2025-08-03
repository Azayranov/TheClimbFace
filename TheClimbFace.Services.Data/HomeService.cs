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

    public async Task<CompetitionDetailsViewModel> GetCompetitionDetailsAsync(Guid competitionId)
    {
        ClimbingCompetition? competition = await competitionRepository
            .GetAllAttached()
            .Where(x => x.Id == competitionId)
            .Include(c => c.Climbers)
            .Include(b => b.Boulders)
            .Include(x => x.ClimbersBouldersQualifications)
            .Include(c => c.Clubs)
            .FirstOrDefaultAsync();

        List<CompetitionClimbersViewModel> climbers = new();

        foreach (var c in competition!.Climbers)
        {
            List<BouldersViewModel> climberBoulders = new();

            CompetitionClimbersViewModel climber = new()
            {
                Id = c.Id.ToString(),
                FirstName = c.FirstName,
                LastName = c.LastName,
                ClubName = c.Club.ClubName,
                Group = c.GroupNumber,
                Gender = c.Sex
            };

            foreach (var b in c.ClimbersBouldersQualification)
            {
                BouldersViewModel boulder = new()
                {
                    Id = b.BoulderId.ToString(),
                    Number = b.Boulder.BoulderNumber,
                    TriesForZone = b.TriesForZone,
                    TriesForTop = b.TriesForTop,
                };
                climberBoulders.Add(boulder);
            }

            climber.Boulders = climberBoulders;
            climber.TopTries = climberBoulders.Sum(x => x.TriesForTop);
            climber.ZoneTries = climberBoulders.Sum(x => x.TriesForZone);
            climber.Tops = climberBoulders.Count(x => x.TriesForTop != 0);
            climber.Zones = climberBoulders.Count(x => x.TriesForZone != 0);

            climbers.Add(climber);
        }

        CompetitionDetailsViewModel model = new()
        {
            Id = competition!.Id,
            Name = competition.Name,
            Organizer = competition.Organizer,
            StartDate = competition.StartDate,
            EndDate = competition.EndDate,
            Climbers = climbers
        };

        model.Climbers = model.Climbers
            .OrderByDescending(x => x.Tops)
            .ThenByDescending(x => x.Zones)
            .ThenBy(x => x.TopTries)
            .ThenBy(x => x.ZoneTries)
            .ToList();

        return model;
    }
}
