using System;

namespace TheClimbFace.Web.ViewModels.Arbitrator;


public class ArbitratorsViewModel
{
    public string CompetitionId { get; set; } = null!;
    public string CompetitionName { get; set; } = null!;
    public Guid ApplicationUserId { get; set; }
    public ICollection<ArbitratorViewModel> Arbitrators = new List<ArbitratorViewModel>();
    public ICollection<BoulderViewModel> AvailableBoulders = new List<BoulderViewModel>();

}

public class ArbitratorViewModel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int AssignedBoulderNumber { get; set; }
    public string CompetitionId { get; set; } = null!;
}

public class BoulderViewModel
{
    public string Id { get; set; } = null!;
    public int Number { get; set; }
}

