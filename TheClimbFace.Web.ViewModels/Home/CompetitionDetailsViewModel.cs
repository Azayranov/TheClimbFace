namespace TheClimbFace.Web.ViewModels.Home;

public class CompetitionDetailsViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Organizer { get; set; } = null!;
    public string Information { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<CompetitionClimbersViewModel> Climbers { get; set; } = new();
}
