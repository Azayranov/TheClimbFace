using System;
using Microsoft.EntityFrameworkCore;
using TheClimbFace.Data.Models;
using TheClimbFace.Data.Repository.Interfaces;
using TheClimbFace.Services.Data.Interfaces;
using TheClimbFace.Web.ViewModels.Competition.Climber;

namespace TheClimbFace.Services.Data;

public class ClimberService(IRepository<ClimbingCompetition> competitionRepository, IRepository<Climber> climberRepository) : IClimberService
{
    public async Task AddClimberToCompetitionAsync(Guid competitionId, AddClimberInputModel model, DateTime birthDate)
    {
        ClimbingCompetition? competition = await competitionRepository
            .GetAllAttached()
            .Where(x => x.Id == competitionId)
            .Include(c => c.Climbers)
            .ThenInclude(c => c.Club)
            .FirstOrDefaultAsync();


        int climberAge = GetClimberAge(birthDate);


        int climberGroup = GetClimberGroup(climberAge);

        Climber climber;

        Club club;
        GetClub(model, birthDate, competition, climberAge, climberGroup, out climber, out club);

        competition.Clubs.Add(club);
        competition.Climbers.Add(climber);
        await competitionRepository.SaveChangesAsync();
    }

    private static void GetClub(AddClimberInputModel model, DateTime birthDate, ClimbingCompetition competition, int climberAge, int climberGroup, out Climber climber, out Club club)
    {
        club = new();
        if (competition!.Clubs.FirstOrDefault(x => x.ClubName == model.ClubName) != null)
            club = competition.Clubs.First(n => n.ClubName == model.ClubName);
        else
        {
            club = new Club
            {
                ClubName = model.ClubName
            };
        }

        climber = model.ToClimber(club, birthDate, climberAge, climberGroup);
    }

    private static int GetClimberGroup(int climberAge)
    {
        int climberGroup = 0;

        if (climberAge >= 7 && climberAge <= 20)
            climberGroup = (climberAge - 7) / 2 + 1;
        return climberGroup;
    }

    private static int GetClimberAge(DateTime birthDate)
    {
        return DateTime.Now.Year - birthDate.Year;
    }

    public async Task DeleteClimberAsync(Guid climberId)
    {
        await climberRepository.DeleteAsync(climberId);
    }

    public async Task<CompetitionClimbersViewModel> GetCompetitionClimbersAsync(Guid competitionId)
    {
        ClimbingCompetition? competition = await competitionRepository
            .GetAllAttached()
            .Where(x => x.Id == competitionId)
            .Include(c => c.Climbers)
            .Include(c => c.Clubs)
            .FirstOrDefaultAsync();

        if (competition == null)
            return null!;

        List<CompetitionClimberViewModel> climbersModel = new();

        if (competition.Climbers.Count != 0)
        {
            foreach (var c in competition.Climbers)
            {
                CompetitionClimberViewModel climber = new()
                {
                    Id = c.Id.ToString(),
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    ClubName = c.Club.ClubName,
                    GroupNumber = c.GroupNumber,
                    StartNumber = c.StartNumber,
                    Gender = c.Sex
                };
                climbersModel.Add(climber);
            }
        }

        CompetitionClimbersViewModel model = new()
        {
            CompetitionId = competition.Id.ToString(),
            CompetitionName = competition.Name,
            Climbers = climbersModel,
            IsActive = competition.IsActive,
            ApplicationUserId = competition.ApplicationUserId,
        };

        return model;
    }

    public async Task SetStartingNumbersAsync(Guid competitionId)
    {
        ClimbingCompetition? competition = await competitionRepository
            .GetAllAttached()
            .Where(x => x.Id == competitionId)
            .Include(c => c.Climbers)
            .FirstOrDefaultAsync();

        List<Climber> climbers = competition!.Climbers
            .OrderBy(x => x.GroupNumber)
            .ToList();

        for (int i = 0; i < climbers.Count; i++)
        {
            climbers[i].StartNumber = i + 1;
        }

        competition.Climbers = climbers;
        await competitionRepository.SaveChangesAsync();
    }
}
