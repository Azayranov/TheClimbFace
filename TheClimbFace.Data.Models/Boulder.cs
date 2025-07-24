using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheClimbFace.Data.Models;

public class Boulder
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public int BoulderNumber { get; set; }

    public Guid CompetitionId { get; set; }
    
    [ForeignKey(nameof(CompetitionId))]
    public ClimbingCompetition ClimbingCompetition { get; set; } = null!;

    //

    public ICollection<ClimberBoulderQualification> ClimbersBoulders { get; set; } = new List<ClimberBoulderQualification>();

}