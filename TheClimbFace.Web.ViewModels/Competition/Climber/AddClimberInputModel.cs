using System.ComponentModel.DataAnnotations;

namespace TheClimbFace.Web.ViewModels.Competition.Climber;

public class AddClimberInputModel
{
    public string CompetitionId { get; set; } = null!;

    [Required]
    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    public string ClubName { get; set; } = null!;

    [Required]
    public string Gender { get; set; } = null!;

    [Required]
    public int BirthDay { get; set; }
    [Required]
    public string BirthMonth { get; set; } = null!;
    [Required]
    public int BirthYear { get; set; }


    public Data.Models.Climber ToClimber(Data.Models.Club club, DateTime birthDate, int age, int group)
    {
        Data.Models.Climber climber = new()
        {
            FirstName = this.FirstName,
            MiddleName = this.MiddleName,
            LastName = this.LastName,
            Club = club,
            Sex = this.Gender,
            BirthDate = birthDate,
            Age = age,
            GroupNumber = group,
        };
        return climber;
    }
}
