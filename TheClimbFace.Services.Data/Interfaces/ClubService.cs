using Microsoft.EntityFrameworkCore;
using TheClimbFace.Data.Models;
using TheClimbFace.Data.Repository.Interfaces;
using TheClimbFace.Web.ViewModels.Club;

namespace TheClimbFace.Services.Data.Interfaces;

public class ClubService(IRepository<ClimbingCompetition> competitionRepository) : IClubService
{
    public async Task<ClubsViewModel> GetCompetitionClubsAsync(Guid competitionId)
    {
        ClimbingCompetition? competition = await competitionRepository
           .GetAllAttached()
           .Where(x => x.Id == competitionId)
           .Include(c => c.Climbers)
           .ThenInclude(c => c.Club)
           .FirstOrDefaultAsync();

        if (competition == null)
            return null!;


        List<ClubViewModel> clubsModel = new();


        foreach (var c in competition.Clubs)
        {
            ClubViewModel club = new()
            {
                Id = c.Id.ToString(),
                ClubName = c.ClubName,
                ClibersCount = c.Climbers.Count
            };

            clubsModel.Add(club);
        }

        ClubsViewModel model = new()
        {
            CompetitionId = competition.Id.ToString(),
            CompetitionName = competition.Name,
            ApplicationUserId = competition.ApplicationUserId,
            Clubs = clubsModel
            
        };

        return model;
    }

}
