namespace TheClimbFace.Web.ViewModels.Competition.Climber;


public class CompetitionClimberViewModel
{
    public string Id { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string ClubName { get; set; } = null!;
    public int GroupNumber { get; set; }
    public int StartNumber { get; set; }
    public string Gender { get; set; } = null!;
}