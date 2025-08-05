using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheClimbFace.Data.Models;

namespace TheClimbFace.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        
        await context.Database.EnsureCreatedAsync();

        
        if (!context.Users.Any())
        {
            await SeedUsersAsync(userManager);
        }

        
        if (!context.ClimbingCompetitions.Any())
        {
            await SeedCompetitionAsync(context);
        }

        
        if (!context.Clubs.Any())
        {
            await SeedClubsAsync(context);
        }

        
        if (!context.Climbers.Any())
        {
            await SeedClimbersAsync(context);
        }

        
        if (!context.Boulders.Any())
        {
            await SeedBouldersAsync(context);
        }
    }

    private static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
    {
        var users = new[]
        {
            new ApplicationUser
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                UserName = "alex123@gmail.com",
                Email = "alex123@gmail.com",
                EmailConfirmed = true,
                NormalizedEmail = "ALEX123@GMAIL.COM",
                NormalizedUserName = "ALEX123@GMAIL.COM"
            },
            new ApplicationUser
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                UserName = "pesho123@gmail.com",
                Email = "pesho123@gmail.com",
                EmailConfirmed = true,
                NormalizedEmail = "PESHO123@GMAIL.COM",
                NormalizedUserName = "PESHO123@GMAIL.COM"
            }
        };

        foreach (var user in users)
        {
            if (await userManager.FindByEmailAsync(user.Email) == null)
            {
                var result = await userManager.CreateAsync(user, user.Email == "alex123@gmail.com" ? "alex123" : "pesho123");
                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to create user {user.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }

    private static async Task SeedCompetitionAsync(ApplicationDbContext context)
    {
        var alexUserId = new Guid("11111111-1111-1111-1111-111111111111");
        
        var competition = new ClimbingCompetition
        {
            Id = Guid.NewGuid(),
            Name = "Тестово Състезание 2025",
            Organizer = "Скален Клуб София",
            Information = "Тестово състезание за демонстрация на системата. Включва всички възрастови групи от 7 до 22+ години.",
            StartDate = new DateTime(2025, 12, 12),
            EndDate = new DateTime(2025, 12, 13),
            RouteCount = 4,
            IsActive = false, 
            UseOficialGroups = true,
            Type = TheClimbFace.Data.Models.Enums.CompetitionType.Boulder,
            ApplicationUserId = alexUserId
        };

        await context.ClimbingCompetitions.AddAsync(competition);
        await context.SaveChangesAsync();
    }

    private static async Task SeedClubsAsync(ApplicationDbContext context)
    {
        var competition = await context.ClimbingCompetitions.FirstAsync();
        
        var clubs = new[]
        {
            new Club { Id = Guid.NewGuid(), ClubName = "Скален Клуб София", CompetitionId = competition.Id },
            new Club { Id = Guid.NewGuid(), ClubName = "Вертикал Варна", CompetitionId = competition.Id },
            new Club { Id = Guid.NewGuid(), ClubName = "Катерач Пловдив", CompetitionId = competition.Id },
            new Club { Id = Guid.NewGuid(), ClubName = "Скала Бургас", CompetitionId = competition.Id },
            new Club { Id = Guid.NewGuid(), ClubName = "Алпинист Стара Загора", CompetitionId = competition.Id },
            new Club { Id = Guid.NewGuid(), ClubName = "Катерач Русе", CompetitionId = competition.Id }
        };

        await context.Clubs.AddRangeAsync(clubs);
        await context.SaveChangesAsync();
    }

    private static async Task SeedClimbersAsync(ApplicationDbContext context)
    {
        var clubs = await context.Clubs.ToListAsync();
        var competition = await context.ClimbingCompetitions.FirstAsync();
        
        var climbers = new List<Climber>();

        
        for (int i = 0; i < 3; i++)
        {
            var birthDate = new DateTime(2018 - i, 6, 15); 
            climbers.Add(new Climber
            {
                Id = Guid.NewGuid(),
                FirstName = $"Boy{i + 1}",
                LastName = "Group1",
                BirthDate = birthDate,
                Age = 2025 - birthDate.Year,
                Sex = "male",
                GroupNumber = 1,
                StartNumber = 0,
                ClubId = clubs[i % clubs.Count].Id,
                CompetitionId = competition.Id
            });
        }

        for (int i = 0; i < 3; i++)
        {
            var birthDate = new DateTime(2018 - i, 8, 20);
            climbers.Add(new Climber
            {
                Id = Guid.NewGuid(),
                FirstName = $"Gril{i + 1}",
                LastName = "Group1",
                BirthDate = birthDate,
                Age = 2025 - birthDate.Year,
                Sex = "female",
                GroupNumber = 1,
                StartNumber = 0,
                ClubId = clubs[(i + 3) % clubs.Count].Id,
                CompetitionId = competition.Id
            });
        }

        
        for (int i = 0; i < 3; i++)
        {
            var birthDate = new DateTime(2015 - i, 3, 10); 
            climbers.Add(new Climber
            {
                Id = Guid.NewGuid(),
                FirstName = $"Boy{i + 1}",
                LastName = "Group2",
                BirthDate = birthDate,
                Age = 2025 - birthDate.Year,
                Sex = "male",
                GroupNumber = 2,
                StartNumber = 0,
                ClubId = clubs[i % clubs.Count].Id,
                CompetitionId = competition.Id
            });
        }

        for (int i = 0; i < 3; i++)
        {
            var birthDate = new DateTime(2015 - i, 5, 25);
            climbers.Add(new Climber
            {
                Id = Guid.NewGuid(),
                FirstName = $"Gril{i + 1}",
                LastName = "Group2",
                BirthDate = birthDate,
                Age = 2025 - birthDate.Year,
                Sex = "female",
                GroupNumber = 2,
                StartNumber = 0,
                ClubId = clubs[(i + 3) % clubs.Count].Id,
                CompetitionId = competition.Id
            });
        }

        
        for (int i = 0; i < 3; i++)
        {
            var birthDate = new DateTime(2012 - i, 7, 8); 
            climbers.Add(new Climber
            {
                Id = Guid.NewGuid(),
                FirstName = $"Boy{i + 1}",
                LastName = "Group3",
                BirthDate = birthDate,
                Age = 2025 - birthDate.Year,
                Sex = "male",
                GroupNumber = 3,
                StartNumber = 0,
                ClubId = clubs[i % clubs.Count].Id,
                CompetitionId = competition.Id
            });
        }

        for (int i = 0; i < 3; i++)
        {
            var birthDate = new DateTime(2012 - i, 9, 12);
            climbers.Add(new Climber
            {
                Id = Guid.NewGuid(),
                FirstName = $"Gril{i + 1}",
                LastName = "Group3",
                BirthDate = birthDate,
                Age = 2025 - birthDate.Year,
                Sex = "female",
                GroupNumber = 3,
                StartNumber = 0,
                ClubId = clubs[(i + 3) % clubs.Count].Id,
                CompetitionId = competition.Id
            });
        }

        
        for (int i = 0; i < 3; i++)
        {
            var birthDate = new DateTime(2009 - i, 4, 15); 
            climbers.Add(new Climber
            {
                Id = Guid.NewGuid(),
                FirstName = $"Boy{i + 1}",
                LastName = "Group4",
                BirthDate = birthDate,
                Age = 2025 - birthDate.Year,
                Sex = "male",
                GroupNumber = 4,
                StartNumber = 0,
                ClubId = clubs[i % clubs.Count].Id,
                CompetitionId = competition.Id
            });
        }

        for (int i = 0; i < 3; i++)
        {
            var birthDate = new DateTime(2009 - i, 6, 30);
            climbers.Add(new Climber
            {
                Id = Guid.NewGuid(),
                FirstName = $"Gril{i + 1}",
                LastName = "Group4",
                BirthDate = birthDate,
                Age = 2025 - birthDate.Year,
                Sex = "female",
                GroupNumber = 4,
                StartNumber = 0,
                ClubId = clubs[(i + 3) % clubs.Count].Id,
                CompetitionId = competition.Id
            });
        }

        
        for (int i = 0; i < 3; i++)
        {
            var birthDate = new DateTime(2006 - i, 2, 20); 
            climbers.Add(new Climber
            {
                Id = Guid.NewGuid(),
                FirstName = $"Boy{i + 1}",
                LastName = "Group5",
                BirthDate = birthDate,
                Age = 2025 - birthDate.Year,
                Sex = "male",
                GroupNumber = 5,
                StartNumber = 0,
                ClubId = clubs[i % clubs.Count].Id,
                CompetitionId = competition.Id
            });
        }

        for (int i = 0; i < 3; i++)
        {
            var birthDate = new DateTime(2006 - i, 11, 5);
            climbers.Add(new Climber
            {
                Id = Guid.NewGuid(),
                FirstName = $"Gril{i + 1}",
                LastName = "Group5",
                BirthDate = birthDate,
                Age = 2025 - birthDate.Year,
                Sex = "female",
                GroupNumber = 5,
                StartNumber = 0,
                ClubId = clubs[(i + 3) % clubs.Count].Id,
                CompetitionId = competition.Id
            });
        }

        
        for (int i = 0; i < 3; i++)
        {
            var birthDate = new DateTime(2003 - i, 1, 10); 
            climbers.Add(new Climber
            {
                Id = Guid.NewGuid(),
                FirstName = $"Boy{i + 1}",
                LastName = "Group6",
                BirthDate = birthDate,
                Age = 2025 - birthDate.Year,
                Sex = "male",
                GroupNumber = 6,
                StartNumber = 0,
                ClubId = clubs[i % clubs.Count].Id,
                CompetitionId = competition.Id
            });
        }

        for (int i = 0; i < 3; i++)
        {
            var birthDate = new DateTime(2003 - i, 10, 15);
            climbers.Add(new Climber
            {
                Id = Guid.NewGuid(),
                FirstName = $"Gril{i + 1}",
                LastName = "Group6",
                BirthDate = birthDate,
                Age = 2025 - birthDate.Year,
                Sex = "female",
                GroupNumber = 6,
                StartNumber = 0,
                ClubId = clubs[(i + 3) % clubs.Count].Id,
                CompetitionId = competition.Id
            });
        }

        await context.Climbers.AddRangeAsync(climbers);
        await context.SaveChangesAsync();
    }

    private static async Task SeedBouldersAsync(ApplicationDbContext context)
    {
        var competition = await context.ClimbingCompetitions.FirstAsync();
        
        var boulders = new[]
        {
            new Boulder
            {
                Id = Guid.NewGuid(),
                BoulderNumber = 1,
                CompetitionId = competition.Id
            },
            new Boulder
            {
                Id = Guid.NewGuid(),
                BoulderNumber = 2,
                CompetitionId = competition.Id
            },
            new Boulder
            {
                Id = Guid.NewGuid(),
                BoulderNumber = 3,
                CompetitionId = competition.Id
            },
            new Boulder
            {
                Id = Guid.NewGuid(),
                BoulderNumber = 4,
                CompetitionId = competition.Id
            }
        };

        await context.Boulders.AddRangeAsync(boulders);
        await context.SaveChangesAsync();
    }
} 