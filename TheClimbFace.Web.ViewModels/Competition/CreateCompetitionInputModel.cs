
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheClimbFace.Data.Models;
using TheClimbFace.Data.Models.Enums;
using static TheClimbFace.Common.EntityValidations.ClimbingCompetition;

namespace TheClimbFace.Web.ViewModels.Competition;

public class CreateCompetitionInputModel
{
    public string? Id { get; set; }

    [Required]
    [MinLength(NameMinLength), MaxLength(NameMaxLength)]
    public string Name { get; set; } = null!;

    [Required]
    [MinLength(OrganiserMinLength), MaxLength(OrganiserMaxLength)]
    public string Organizer { get; set; } = null!;

    [Required]
    public CompetitionType Type { get; set; }

    [Required]
    [MinLength(InfoMinLength), MaxLength(InfoMaxLength)]
    public string Information { get; set; } = null!;

    [Required]
    public int StartDay { get; set; }

    [Required]
    public string StartMonth { get; set; } = null!;

    [Required]
    public int StartYear { get; set; }

    [Required]
    public int EndDay { get; set; }

    [Required]
    public string EndMonth { get; set; } = null!;

    [Required]
    public int EndYear { get; set; }

    [Required]
    public int RouteCount { get; set; } = 0;

    public ClimbingCompetition ToClimbingCompetition(DateTime startDate, DateTime endDate)
    {
        ClimbingCompetition climbingCompetition = new()
        {
            Name = this.Name,
            Organizer = this.Organizer,
            Information = this.Information,
            Type = this.Type,
            StartDate = startDate,
            EndDate = endDate,
        };

        return climbingCompetition;
    }
}
