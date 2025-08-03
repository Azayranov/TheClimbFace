namespace TheClimbFace.Web.ViewModels.Competition.Arbitrator;


public class ArbitratorViewModel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int AssignedBoulderNumber { get; set; }
    public string CompetitionId { get; set; } = null!;
}

