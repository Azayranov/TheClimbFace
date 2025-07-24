using TheClimbFace.Web.ViewModels.Home;
namespace TheClimbFace.Services.Data.Interfaces;

public interface IHomeService
{
    Task<IEnumerable<HomeViewModel>> GetActiveCompetitionsAsync();

    Task<CompetitionDetailsViewModel> GetCompetitionDetailsAsync(Guid competitionId);
}
