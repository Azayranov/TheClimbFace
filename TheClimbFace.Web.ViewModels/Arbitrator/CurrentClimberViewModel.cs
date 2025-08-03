using System;

namespace TheClimbFace.Web.ViewModels.Arbitrator;

public class CurrentClimberViewModel
{
    public string Name { get; set; } = null!;
    public string Club { get; set; } = null!;
    public int Group { get; set; }
    public int StartNumber { get; set; }
    public int MaxAttempts { get; set; }
    public int CurrentAttempt { get; set; }
    public int TriesForZone { get; set; }
    public int TriesForTop { get; set; }
}
