
using TheClimbFace.Data.Models;
using TheClimbFace.Web.ViewModels.Arbitrator;

namespace TheClimbFace.Services.Data.Interfaces;

public interface IBoulderScoringService
{
    Task<ScoreViewModel> GetScoreViewModelAsync(Guid competitionId, Guid userId, int BoulderNumber);
    Task<CurrentClimberViewModel> GetClimberForScoringAsync(Guid CompetitionId, int StartNumber, int BoulderNumber);
    Task<ScoreViewModel> GetScoreViewModelWithClimberAsync(Guid competitionId, Guid userId, int startNumber, int BoulderNumber);
    Task SetFailForClimberAsync(Guid CompetitionId, int StartNumber, int BoulderNumber);
    Task SetTopForClimberAsync(Guid CompetitionId, int StartNumber, int BoulderNumber);
    Task SetZoneForClimberAsync(Guid CompetitionId, int StartNumber, int BoulderNumber);
    Task<ClimberBoulderQualification> GetClimberBoulderAsync(Guid CompetitionId, int StartNumber, int BoulderNumber);
}
