namespace TheClimbFace.Web.ViewModels.Competition;

public class CompetitionViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Organizer { get; set; } = null!;
    public string Information { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public Guid ApplicationUserId { get; set; }

}
