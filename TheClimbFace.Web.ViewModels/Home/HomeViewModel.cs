using System;
using TheClimbFace.Data.Models;

namespace TheClimbFace.Web.ViewModels.Home;

public class HomeViewModel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Organizer { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }


    public HomeViewModel ToHomeViewModel(ClimbingCompetition competition)
    {

        HomeViewModel model = new()
        {
            Id = competition.Id.ToString(),
            Name = competition.Name,
            Organizer = competition.Organizer,
            StartDate = competition.StartDate,
            EndDate = competition.EndDate
        };

        return model;
    }
}
