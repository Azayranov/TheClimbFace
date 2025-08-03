using System;

namespace TheClimbFace.Web.ViewModels.Arbitrator;

public class ScoreViewModel
{
    public string CompetitionId { get; set; } = null!;
    public int BoulderNumber { get; set; }
    public int StartNumber { get; set; }
    public CurrentClimberViewModel CurrentClimber { get; set; } = null!;
}

