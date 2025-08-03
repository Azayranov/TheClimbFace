using System.ComponentModel.DataAnnotations;

namespace TheClimbFace.Web.ViewModels.Competition.Arbitrator;

public class AddArbitratorInputModel
{
    public string? Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public int AssignedBoulderNumber { get; set; }
    public string CompetitionId { get; set; } = null!;

    public ICollection<BoulderViewModel> AvailableBoulders = new List<BoulderViewModel>();

}
