using System;

namespace TheClimbFace.Web.ViewModels.Competition;

public class DetailsViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Organizer { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public int ClimbersCount { get; set; }
    public int ClubsCount { get; set; }
    public int ArbitratorsCount { get; set; }
    public int BoulderCount { get; set; }
}

