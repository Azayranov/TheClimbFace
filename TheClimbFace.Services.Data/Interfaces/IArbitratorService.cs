
using TheClimbFace.Web.ViewModels.Arbitrator;

namespace TheClimbFace.Services.Data.Interfaces;

public interface IArbitratorService
{
    Task<ArbitratorsViewModel> GetCompetitionArbitratorsAsync(Guid competitionId);
    Task AddArbitratorToCompetitionAsync(Guid competitionId, ArbitratorViewModel arbitrators);
    Task DeleteArbitratorFromCompetitionAsync(Guid competitionId, Guid userId);



}
