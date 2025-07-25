using System;
using TheClimbFace.Web.ViewModels.Competition;

namespace TheClimbFace.Services.Data.Interfaces;

public interface ICompetitionService
{
    Task CreateCompetitionAsync(CreateCompetitionInputModel model, DateTime StartDate, DateTime EndDate, Guid userId);
    Task<CompetitionListViewModel> GetUserCompetitionsAsync(Guid userId);
    Task<DetailsViewModel> GetCompetitionDetailsAsync(Guid competitionId);
    

}
