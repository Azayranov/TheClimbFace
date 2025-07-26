using System;
using Microsoft.EntityFrameworkCore;
using TheClimbFace.Data.Models;
using TheClimbFace.Data.Repository.Interfaces;
using TheClimbFace.Services.Data.Interfaces;
using TheClimbFace.Web.ViewModels.Climber;

namespace TheClimbFace.Services.Data;

public class ClimberService(IRepository<ClimbingCompetition> competitionRepository, IRepository<Climber> climbingRepository) : IClimberService
{
    public async Task AddClimberToCompetitionAsync(Guid competitionId, AddClimberInputModel model, DateTime birthDate)
    {
        ClimbingCompetition? competition = await competitionRepository
            .GetAllAttached()
            .Where(x => x.Id == competitionId)
            .Include(c => c.Climbers)
            .ThenInclude(c => c.Club)
            .FirstOrDefaultAsync();

        //climber age
        #warning make it a method
        int climberAge = DateTime.Now.Year - birthDate.Year;

        //climber group
        #warning make it a method

        int climberGroup = 0;

        if (climberAge >= 7 && climberAge <= 20)
            climberGroup = (climberAge - 7) / 2 + 1;

        //climber ClubId
        #warning make it a method

        Club club = new();
        Climber climber;

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

        competition.Clubs.Add(club);
        competition.Climbers.Add(climber);
        await competitionRepository.SaveChangesAsync();
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
                    GroupNumber = c.GroupNumber.ToString(),
                    StartNumber = c.StartNumber
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
            ApplicationUserId = competition.ApplicationUserId
        };

        return model;
    }
}
