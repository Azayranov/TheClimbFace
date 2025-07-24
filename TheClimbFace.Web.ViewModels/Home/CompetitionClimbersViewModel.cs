namespace TheClimbFace.Web.ViewModels.Home;

public class CompetitionClimbersViewModel
{
    public string Id { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string ClubName { get; set; } = null!;
    public int Group { get; set; }
    public string Gender { get; set; } = null!;
    public int ZoneTries { get; set; }
    public int TopTries { get; set; }
    public int Zones { get; set; }
    public int Tops { get; set; }
    public List<BouldersViewModel> Boulders { get; set; } = new();
}
