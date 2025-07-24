using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static TheClimbFace.Common.EntityValidations.Climber;

namespace TheClimbFace.Data.Models;

public class Climber
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(FirstNameMaxLength)]
    public string FirstName { get; set; } = null!;

    [MaxLength(MiddleNameMaxLength)]
    public string? MiddleName { get; set; }

    [Required]
    [MaxLength(LastNameMaxLength)]
    public string LastName { get; set; } = null!;

    [ForeignKey(nameof(Club))]
    public Guid ClubId { get; set; }
    public Club Club { get; set; } = null!;

    [Required]
    public string Sex { get; set; } = null!;

    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    public int Age { get; set; } 

    [Required]
    public int GroupNumber { get; set; }

    [Required]
    public int StartNumber { get; set; } = 0;

    public Guid CompetitionId { get; set; }

    [ForeignKey(nameof(CompetitionId))]
    public ClimbingCompetition ClimbingCompetition { get; set; } = null!;

    //

    public ICollection<ClimberBoulderQualification> ClimbersBouldersQualification { get; set; } = new List<ClimberBoulderQualification>();
}
