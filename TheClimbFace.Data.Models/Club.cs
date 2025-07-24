using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static TheClimbFace.Common.EntityValidations.Club;
namespace TheClimbFace.Data.Models;

public class Club
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(ClubNameMaxLength)]
    public string ClubName { get; set; } = null!;

    public Guid CompetitionId { get; set; }
    
    [ForeignKey(nameof(CompetitionId))]
    public ClimbingCompetition ClimbingCompetition { get; set; } = null!;

    //

    public ICollection<Climber> Climbers { get; set; } = new List<Climber>();
}