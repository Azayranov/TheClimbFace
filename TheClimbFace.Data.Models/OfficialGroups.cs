using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheClimbFace.Data.Models;

public class OfficialGroups
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public int Number { get; set; }

    [Required]
    public int MinAge { get; set; }

    [Required]
    public int MaxAge { get; set; }

    public int? BoulderMaxTries { get; set; }

    public int? LimitTime { get; set; }

    public int? FinalsQuota { get; set; }

    public Guid CompetitionId { get; set; }

    [ForeignKey(nameof(CompetitionId))]
    public ClimbingCompetition Competition { get; set; } = null!;
}