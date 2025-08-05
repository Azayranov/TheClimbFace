using System;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using TheClimbFace.Data.Models;
using TheClimbFace.Data.Repository.Interfaces;
using TheClimbFace.Services.Data.Interfaces;
using TheClimbFace.Web.ViewModels.Competition;

namespace TheClimbFace.Services.Data;

public class CompetitionService(IRepository<ClimbingCompetition> competitionRepository, IRepository<Boulder> boulderRepository,
                                IRepository<ClimberBoulderQualification> climbersBouldersRepository, IRepository<Climber> climberRepository,
                                IRepository<Club> clubRepository) : ICompetitionService
{
    public async Task AddCompetitionBouldersAsync(ClimbingCompetition competition)
    {
        await boulderRepository.DeleteRangeAsync(competition.Boulders.ToList());

        for (int i = 0; i < competition.RouteCount; i++)
        {
            Boulder boulder = new Boulder()
            {
                BoulderNumber = i + 1,
            };

            competition.Boulders.Add(boulder);
        }
    }

    public async Task CreateCompetitionAsync(CreateCompetitionInputModel model, DateTime StartDate, DateTime EndDate, Guid userId)
    {
        var competition = model.ToClimbingCompetition(StartDate, EndDate);
        competition.ApplicationUserId = userId;
        await competitionRepository.AddAsync(competition);
    }

    public async Task DeleteCompetitionAsync(Guid competitionId)
    {
        ClimbingCompetition? competition = await competitionRepository
            .GetAllAttached()
            .FirstOrDefaultAsync(x => x.Id == competitionId);

        if (competition != null)
        {
            competition.IsDeleted = true;
            competition.IsActive = false;
        }

        await competitionRepository.SaveChangesAsync();
    }

    public async Task EditCompetitionAsync(CreateCompetitionInputModel model, DateTime startDate, DateTime endDate, Guid competitionId)
    {
        ClimbingCompetition? competition = await competitionRepository
            .GetAllAttached()
            .Where(x => x.Id == competitionId)
            .Include(b => b.Boulders)
            .FirstOrDefaultAsync();

        bool isBoulderCountChanged = competition!.RouteCount != model.RouteCount;

        competition.Name = model.Name;
        competition.Organizer = model.Organizer;
        competition.Information = model.Information;
        competition.StartDate = startDate;
        competition.EndDate = endDate;
        competition.RouteCount = model.RouteCount;

        if (isBoulderCountChanged)
        {
            if (competition.RouteCount > 0)
                await AddCompetitionBouldersAsync(competition);
            else
                await boulderRepository.DeleteRangeAsync(competition.Boulders.ToList());
        }

        await competitionRepository.SaveChangesAsync();
    }


    public async Task<CreateCompetitionInputModel> GetCompetitionAsync(Guid competitionId)
    {
        ClimbingCompetition? competition = await competitionRepository.GetByIdAsync(competitionId);

        CreateCompetitionInputModel model = new()
        {
            Name = competition.Name,
            Organizer = competition.Organizer,
            Information = competition.Information,
            StartDay = competition.StartDate.Day,
            StartMonth = competition.StartDate.ToString("MMMM"),
            StartYear = competition.StartDate.Year,
            EndDay = competition.EndDate.Day,
            EndMonth = competition.EndDate.ToString("MMMM"),
            EndYear = competition.EndDate.Year,
            RouteCount = competition.RouteCount,
            ApplicationUserId = competition.ApplicationUserId
        };

        return model;

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
            IsActive = competition.IsActive,
            ApplicationUserId = competition.ApplicationUserId

        };

        return model;
    }

    public async Task<CompetitionListViewModel> GetUserCompetitionsAsync(Guid userId)
    {

        List<ClimbingCompetition> competitions = await competitionRepository.GetAllAttached()
            .Where(x => x.Arbitrators.Any(x => x.User.Id == userId) && x.IsDeleted == false)
            .Include(x => x.Arbitrators)
            .ThenInclude(x => x.User)
            .ToListAsync();

        IEnumerable<ClimbingCompetition> userCompetitions = competitionRepository.GetAllAttached()
            .Where(x => x.ApplicationUserId == userId && x.IsDeleted == false)
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

    public bool IsUserCreator(Guid applicationUserId, Guid userId)
    {
        return applicationUserId == userId;
    }

    public async Task StartCompetitionAsync(Guid competitionId)
    {
        ClimbingCompetition? competition = await competitionRepository
            .GetAllAttached()
            .Where(x => x.Id == competitionId)
            .Include(c => c.Climbers)
            .Include(b => b.Boulders)
            .Include(x => x.ClimbersBouldersQualifications)
            .Include(c => c.Clubs)
            .FirstOrDefaultAsync();

        var climbersBoulders = new List<ClimberBoulderQualification>();

        var climbers = competition!.Climbers.ToList();
        var boulders = competition.Boulders.ToList();

        foreach (var c in climbers)
        {
            var climberId = c.Id;

            foreach (var b in boulders)
            {
                var boulderId = b.Id;

                ClimberBoulderQualification cb = new()
                {
                    ClimberId = climberId,
                    BoulderId = boulderId
                };

                competition.ClimbersBouldersQualifications.Add(cb);
            }
        }
        competition!.IsActive = true;

        await competitionRepository.SaveChangesAsync();
    }

    public async Task StopCompetitionAsync(Guid competitionId)
    {
        ClimbingCompetition? competition = await competitionRepository
            .GetAllAttached()
            .Where(x => x.Id == competitionId)
            .Include(cb => cb.ClimbersBouldersQualifications)
            .FirstOrDefaultAsync();

        await climbersBouldersRepository.DeleteRangeAsync(competition!.ClimbersBouldersQualifications.ToList());
        competition!.IsActive = false;
        await competitionRepository.SaveChangesAsync();
    }
}
