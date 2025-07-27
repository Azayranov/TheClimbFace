using System;
using TheClimbFace.Web.ViewModels.Climber;

namespace TheClimbFace.Services.Data.Interfaces;

public interface IClimberService
{
    Task<CompetitionClimbersViewModel> GetCompetitionClimbersAsync(Guid competitionId);
    Task AddClimberToCompetitionAsync(Guid competitionId, AddClimberInputModel model, DateTime birthDate);
    Task DeleteClimberAsync(Guid climberId);



}
