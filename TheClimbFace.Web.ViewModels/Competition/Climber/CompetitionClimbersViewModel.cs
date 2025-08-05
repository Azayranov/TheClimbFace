using System;

namespace TheClimbFace.Web.ViewModels.Competition.Climber;


public class CompetitionClimbersViewModel
{
    public string CompetitionId { get; set; } = null!;
    public string CompetitionName { get; set; } = null!;
    public bool IsActive { get; set; }
    public Guid ApplicationUserId { get; set; }
    public int? GroupFilter { get; set; }
    public string? GenderFilter { get; set; }
    public ICollection<CompetitionClimberViewModel> Climbers = new List<CompetitionClimberViewModel>();
}
