namespace TheClimbFace.Web.ViewModels.Competition;

public class ArbitratorCompetition
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Organizer { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int AssignedBoulderNumber { get; set; }
}
