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


    public HomeViewModel ToHomeViewModel(ClimbingCompetition competitions)
    {

        HomeViewModel model = new()
        {
            Id = Id.ToString(),
            Name = Name,
            Organizer = Organizer,
            StartDate = StartDate,
            EndDate = EndDate
        };

        return model;
    }
}
