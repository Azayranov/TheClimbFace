using System;
using TheClimbFace.Data.Models;
using TheClimbFace.Web.ViewModels.Competition;

namespace TheClimbFace.Services.Data.Interfaces;

public interface ICompetitionService
{
    Task CreateCompetitionAsync(CreateCompetitionInputModel model, DateTime StartDate, DateTime EndDate, Guid userId);
    Task<CompetitionListViewModel> GetUserCompetitionsAsync(Guid userId);
    Task<DetailsViewModel> GetCompetitionDetailsAsync(Guid competitionId);
    Task<CreateCompetitionInputModel> GetCompetitionAsync(Guid competitionId);
    Task EditCompetitionAsync(CreateCompetitionInputModel model, DateTime startDate, DateTime endDate, Guid competitionId);
    Task AddCompetitionBouldersAsync(ClimbingCompetition competition);
    Task DeleteCompetitionAsync(Guid competitionId);


}
