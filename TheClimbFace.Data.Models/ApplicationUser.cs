namespace TheClimbFace.Data.Models;


using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public ApplicationUser()
    {
        this.Id = Guid.NewGuid();
    }

    //
    public ICollection<ClimbingCompetition> BoulderCompetitions { get; set; } = new List<ClimbingCompetition>();

}