using System;

namespace TheClimbFace.Web.ViewModels.Arbitrator;


public class ArbitratorsViewModel
{
    public string CompetitionId { get; set; } = null!;
    public string CompetitionName { get; set; } = null!;
    public Guid ApplicationUserId { get; set; }
    public ICollection<ArbitratorViewModel> Arbitrators = new List<ArbitratorViewModel>();
}

