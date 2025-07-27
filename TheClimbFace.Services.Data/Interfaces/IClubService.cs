using System;
using TheClimbFace.Web.ViewModels.Club;

namespace TheClimbFace.Services.Data.Interfaces;

public interface IClubService
{
    Task<ClubsViewModel> GetCompetitionClubsAsync(Guid competitionId);

}
