using System;
using Microsoft.EntityFrameworkCore;
using TheClimbFace.Data.Models;
using TheClimbFace.Data.Repository.Interfaces;
using TheClimbFace.Services.Data.Interfaces;
using TheClimbFace.Web.ViewModels.Competition;

namespace TheClimbFace.Services.Data;

public class CompetitionService(IRepository<ClimbingCompetition> competitionRepository) : ICompetitionService
{
    public async Task CreateCompetitionAsync(CreateCompetitionInputModel model, DateTime StartDate, DateTime EndDate, Guid userId)
    {
        var competition = model.ToClimbingCompetition(StartDate, EndDate);
        competition.ApplicationUserId = userId;
        await competitionRepository.AddAsync(competition);
    }

public async Task<DetailsViewModel> GetCompetitionDetailsAsync(Guid competitionId)
    {
        ClimbingCompetition? competition = await competitionRepository
            .GetAllAttached()
            .Where(x => x.Id == competitionId)
            .Include(x => x.Arbitrators)
            .Include(x => x.Boulders)
            .Include(c => c.Climbers)
            .ThenInclude(c => c.Club)
            .FirstOrDefaultAsync();

        if (competition == null)
            return null!;

        DetailsViewModel model = new()
        {
            Id = competition!.Id,
            Name = competition.Name,
            Organizer = competition.Organizer,
            StartDate = competition.StartDate,
            EndDate = competition.EndDate,
            ClimbersCount = competition.Climbers.Count,
            ClubsCount = competition.Clubs.Count,
            BoulderCount = competition.Boulders.Count,
            ArbitratorsCount = competition.Arbitrators.Count,
            IsActive = competition.IsActive
        };

        return model;
    }

    public async Task<CompetitionListViewModel> GetUserCompetitionsAsync(Guid userId)
    {

        List<ClimbingCompetition> competitions = await competitionRepository.GetAllAttached()
            .Where(x => x.Arbitrators.Any(x => x.User.Id == userId))
            .Include(x => x.Arbitrators)
            .ThenInclude(x => x.User)
            .ToListAsync();

        IEnumerable<ClimbingCompetition> userCompetitions = competitionRepository.GetAllAttached()
            .Where(x => x.ApplicationUserId == userId)
            .ToList();

        var activeCompetitions = new List<CompetitionViewModel>();
        var inactiveCompetitions = new List<CompetitionViewModel>();
        var arbitratorCompetitions = new List<ArbitratorCompetition>();

        foreach (var c in competitions)
        {

            foreach (var a in c.Arbitrators)
            {

                if (a.User.Id == userId)
                {
                    ArbitratorCompetition competition = new()
                    {
                        Id = c.Id.ToString(),
                        Name = c.Name,
                        Organizer = c.Organizer,
                        IsActive = c.IsActive,
                        StartDate = c.StartDate,
                        EndDate = c.EndDate,
                        AssignedBoulderNumber = a.AssignedBoulderNumber
                    };

                    arbitratorCompetitions.Add(competition);
                }
            }
        }

        foreach (var c in userCompetitions)
        {
            CompetitionViewModel model = new()
            {
                Id = c.Id,
                Name = c.Name,
                Organizer = c.Organizer,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                IsActive = c.IsActive
            };

            if (c.IsActive)
            {
                activeCompetitions.Add(model);
            }
            else
            {
                inactiveCompetitions.Add(model);
            }
        }

        return new CompetitionListViewModel
        {
            ActiveCompetitions = activeCompetitions,
            InactiveCompetitions = inactiveCompetitions,
            ArbitratorCompetitions = arbitratorCompetitions.OrderBy(x => x.AssignedBoulderNumber).ToList()
        };
    }

}
