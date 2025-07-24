using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheClimbFace.Data.Models;

namespace TheClimbFace.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<ClimbingCompetition> ClimbingCompetitions { get; set; }
    public DbSet<Climber> Climbers { get; set; }
    public DbSet<Boulder> Boulders { get; set; }
    public DbSet<Club> Clubs { get; set; }
    public DbSet<Arbitrator> Arbitrators { get; set; }
    public DbSet<OfficialGroups> OfficialGroups { get; set; }
    public DbSet<ClimberBoulderQualification> ClimbersBouldersQualifications { get; set; }
}
