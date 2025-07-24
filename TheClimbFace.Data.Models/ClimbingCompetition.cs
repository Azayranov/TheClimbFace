using System.ComponentModel.DataAnnotations;
using TheClimbFace.Data.Models.Enums;
using static TheClimbFace.Common.EntityValidations.ClimbingCompetition;

namespace TheClimbFace.Data.Models;

public class ClimbingCompetition
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(OrganiserMaxLength)]
    public string Organizer { get; set; } = null!;

    [Required]
    [MaxLength(InfoMaxLength)]
    public string Information { get; set; } = null!;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public int RouteCount { get; set; } = 0;

    [Required]
    public bool IsActive { get; set; } = false;

    [Required]
    public bool UseOficialGroups { get; set; } = true;

    [Required]
    public CompetitionType Type { get; set; } = CompetitionType.Boulder;

    public bool IsDeleted { get; set; } = false;
    public bool HasPassed { get; set; } = false;

    //

    public ICollection<OfficialGroups> OfficialGroups { get; set; } = new List<OfficialGroups>();
    public ICollection<Climber> Climbers { get; set; } = new List<Climber>();
    public ICollection<Club> Clubs { get; set; } = new List<Club>();
    public ICollection<Boulder> Boulders { get; set; } = new List<Boulder>();
    public ICollection<Arbitrator> Arbitrators { get; set; } = new List<Arbitrator>();
    public ICollection<ClimberBoulderQualification> ClimbersBouldersQualifications { get; set; } = new List<ClimberBoulderQualification>();

}
