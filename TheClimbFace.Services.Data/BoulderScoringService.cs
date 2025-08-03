using System;
using Microsoft.EntityFrameworkCore;
using TheClimbFace.Data.Models;
using TheClimbFace.Data.Repository.Interfaces;
using TheClimbFace.Services.Data.Interfaces;
using TheClimbFace.Web.ViewModels.Arbitrator;

namespace TheClimbFace.Services.Data;


public class BoulderScoringService(IRepository<ClimbingCompetition> competitionRepository, IRepository<ClimberBoulderQualification> climbersBouldersRepository) : IBoulderScoringService
{
    public async Task<ClimberBoulderQualification> GetClimberBoulderAsync(Guid CompetitionId, int StartNumber, int BoulderNumber)
    {
        var competition = await competitionRepository.GetAllAttached()
            .Where(x => x.Id == CompetitionId)
            .Include(x => x.ClimbersBouldersQualifications)
            .ThenInclude(x => x.Climber)
            .ThenInclude(x => x.Club)
            .Include(x => x.ClimbersBouldersQualifications)
            .ThenInclude(x => x.Boulder)
            .FirstOrDefaultAsync();

        var climberBoulder = competition!.ClimbersBouldersQualifications
            .Where(x => x.Boulder.BoulderNumber == BoulderNumber && x.Climber.StartNumber == StartNumber)
            .FirstOrDefault();

        return climberBoulder!;
    }

    public async Task<CurrentClimberViewModel> GetClimberForScoringAsync(Guid CompetitionId, int StartNumber, int BoulderNumber)
    {
        ClimberBoulderQualification climberBoulder = await GetClimberBoulderAsync(CompetitionId, StartNumber, BoulderNumber);

        if (climberBoulder == null)
            return null;

        var climberName = $"{climberBoulder.Climber.FirstName} {climberBoulder.Climber.LastName}";

        CurrentClimberViewModel model = new()
        {
            Name = climberName,
            Club = climberBoulder.Climber.Club.ClubName,
            Group = climberBoulder.Climber.GroupNumber,
            StartNumber = StartNumber,
            MaxAttempts = climberBoulder.MaxTries,
            CurrentAttempt = climberBoulder.CurrentTry,
            TriesForTop = climberBoulder.TriesForTop,
            TriesForZone = climberBoulder.TriesForZone
        };

        return model;
    }

    public async Task<ScoreViewModel> GetScoreViewModelAsync(Guid competitionId, Guid userId, int BoulderNumber)
    {
        var competition = await competitionRepository.GetAllAttached()
            .Where(x => x.Id == competitionId)
            .Include(x => x.Arbitrators)
            .FirstOrDefaultAsync();

        var arbitrator = competition!.Arbitrators.FirstOrDefault(x => x.UserId == userId && x.AssignedBoulderNumber == BoulderNumber);

        ScoreViewModel model = new()
        {
            CompetitionId = competitionId.ToString(),
            BoulderNumber = arbitrator.AssignedBoulderNumber,
            StartNumber = 0,
            CurrentClimber = null!
        };

        if (model.StartNumber > 0)
        {
            model.CurrentClimber = await GetClimberForScoringAsync(competitionId, model.StartNumber, model.BoulderNumber);
        }

        return model;
    }

    public async Task<ScoreViewModel> GetScoreViewModelWithClimberAsync(Guid competitionId, Guid userId, int startNumber, int boulderNumber)
    {
        var model = await GetScoreViewModelAsync(competitionId, userId, boulderNumber);
        model.StartNumber = startNumber;
        model.CurrentClimber = await GetClimberForScoringAsync(competitionId, startNumber, model.BoulderNumber);

        return model;
    }

    public async Task SetFailForClimberAsync(Guid CompetitionId, int StartNumber, int BoulderNumber)
    {
        ClimberBoulderQualification climberBoulder = await GetClimberBoulderAsync(CompetitionId, StartNumber, BoulderNumber);

        climberBoulder!.CurrentTry++;
        await climbersBouldersRepository.SaveChangesAsync();
    }

    public Task SetTopForClimberAsync(Guid CompetitionId, int StartNumber, int BoulderNumber)
    {
        throw new NotImplementedException();
    }

    public Task SetZoneForClimberAsync(Guid CompetitionId, int StartNumber, int BoulderNumber)
    {
        throw new NotImplementedException();
    }

}
