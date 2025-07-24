using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TheClimbFace.Data.Models;

[PrimaryKey(nameof(ClimberId), nameof(BoulderId))]
public class ClimberBoulderQualification
{
    [ForeignKey(nameof(Climber))]
    public Guid ClimberId { get; set; }
    public Climber Climber { get; set; } = null!;

    [ForeignKey(nameof(Boulder))]
    public Guid BoulderId { get; set; }
    public Boulder Boulder { get; set; } = null!;

    [Required]
    public int MaxTries { get; set; } = 5;

    [Required]
    public int CurrentTry { get; set; } = 0;

    [Required]
    public int TriesForTop { get; set; } = 0;

    [Required]
    public int TriesForZone { get; set; } = 0;

    public Guid CompetitionId { get; set; }

    [ForeignKey(nameof(CompetitionId))]
    public ClimbingCompetition ClimbingCompetition { get; set; } = null!;
}

