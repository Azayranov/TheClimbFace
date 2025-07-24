using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheClimbFace.Data.Models;

public class Arbitrator
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int AssignedBoulderNumber { get; set; } = 0;

    //

    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;

    public Guid CompetitionId { get; set; }

    [ForeignKey(nameof(CompetitionId))]
    public ClimbingCompetition ClimbingCompetition { get; set; } = null!;
}