
using TheClimbFace.Data.Models;
using TheClimbFace.Web.ViewModels.Arbitrator;

namespace TheClimbFace.Services.Data.Interfaces;

public interface IArbitratorService
{
    Task<ArbitratorsViewModel> GetCompetitionArbitratorsAsync(Guid competitionId);
    Task<bool> AddArbitratorToCompetitionAsync(Guid competitionId, AddArbitratorInputModel arbitrators);
    Task DeleteArbitratorFromCompetitionAsync(Guid competitionId, Guid userId);
    Task<AddArbitratorInputModel> GetAddArbitratorModelAsync(Guid competitionId);
    Task<List<BoulderViewModel>> GetAvailableBouldersAsync(Guid competitionId);




}
