using System;

namespace TheClimbFace.Web.ViewModels.Club;

public class ClubsViewModel
{
    public string CompetitionId { get; set; } = null!;
    public string CompetitionName { get; set; } = null!;
    public Guid ApplicationUserId { get; set; }
    public ICollection<ClubViewModel> Clubs { get; set; } = new List<ClubViewModel>();

}

