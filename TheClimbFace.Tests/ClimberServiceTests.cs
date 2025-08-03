using Moq;
using TheClimbFace.Data.Models;
using TheClimbFace.Data.Repository.Interfaces;
using TheClimbFace.Services.Data;
using TheClimbFace.Web.ViewModels.Competition.Climber;
using MockQueryable.Moq;
using NUnit.Framework.Internal;

namespace TheClimbFace.Tests;

public class ClimberServiceTests
{
    private Guid competitionId;
    private string clubName;
    private DateTime birthDate;
    private Mock<IRepository<ClimbingCompetition>> mockCompetitionRepo;
    private Mock<IRepository<Climber>> mockClimberRepo;
    private ClimbingCompetition competition;
    private ClimberService service;

    [SetUp]
    public void Setup()
    {
        var competitionId = Guid.NewGuid();
        clubName = "Test Club";
        birthDate = new DateTime(2007, 12, 12);

        mockCompetitionRepo = new Mock<IRepository<ClimbingCompetition>>();
        mockClimberRepo = new Mock<IRepository<Climber>>();

        var club = new Club { ClubName = clubName };
        competition = new ClimbingCompetition
        {
            Id = competitionId,
            Name = "Test Competition",
            Clubs = new List<Club> { club },
            Climbers = new List<Climber>()
        };

        var competitions = new List<ClimbingCompetition> { competition };
        var mockCompetitionQueryable = competitions.AsQueryable().BuildMock();

        mockCompetitionRepo.Setup(r => r.GetAllAttached()).Returns(mockCompetitionQueryable);
        mockCompetitionRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        service = new ClimberService(mockCompetitionRepo.Object, mockClimberRepo.Object);
    }
    private Climber GetRandomTestClimberData()
    {
        Random random = new Random();
        var climber = new Climber
        {
            Id = Guid.NewGuid(),
            FirstName = $"Ivan{random.Next(1, 999)}",
            LastName = $"Ivanov{random.Next(1, 999)}",
            Club = new Club { ClubName = clubName },
            Sex = "Male",
            BirthDate = birthDate
        };

        return climber;
    }

    [Test]
    public void Dummy()
    {
        Assert.Pass();
    }

    [Test]
    public async Task AddClimberToCompetitionAsync_ShouldAddClimber()
    {
        var climber = GetRandomTestClimberData();
        var model = new AddClimberInputModel()
        {
            FirstName = climber.FirstName,
            LastName = climber.LastName,
            Gender = climber.Sex,
            ClubName = clubName
        };

        var service = new ClimberService(mockCompetitionRepo.Object, mockClimberRepo.Object);
        await service.AddClimberToCompetitionAsync(competitionId, model, birthDate);

        Assert.That(competition.Climbers.Count, Is.EqualTo(1));
        Assert.That(competition.Climbers.First().FirstName, Is.EqualTo(model.FirstName));
        Assert.That(competition.Climbers.First().LastName, Is.EqualTo(model.LastName));
        Assert.That(competition.Climbers.First().Club.ClubName, Is.EqualTo(clubName));
        Assert.That(competition.Climbers.First().Sex, Is.EqualTo("Male"));
        Assert.That(competition.Climbers.First().BirthDate.Year, Is.EqualTo(2007));
        Assert.That(competition.Climbers.First().BirthDate.Day, Is.EqualTo(12));
        Assert.That(competition.Climbers.First().BirthDate.Month, Is.EqualTo(12));
        mockCompetitionRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task GetCompetitionClimbersAsync_ShouldGetAllClimbers()
    {
        var climber1 = GetRandomTestClimberData();
        var model1 = new AddClimberInputModel()
        {
            FirstName = climber1.FirstName,
            LastName = climber1.LastName,
            Gender = climber1.Sex,
            ClubName = clubName
        };

        var climber2 = GetRandomTestClimberData();
        var model2 = new AddClimberInputModel()
        {
            FirstName = climber2.FirstName,
            LastName = climber2.LastName,
            Gender = climber2.Sex,
            ClubName = clubName
        };

        var service = new ClimberService(mockCompetitionRepo.Object, mockClimberRepo.Object);
        await service.AddClimberToCompetitionAsync(competitionId, model1, birthDate);
        await service.AddClimberToCompetitionAsync(competitionId, model2, birthDate);

        Assert.That(competition.Climbers.Count, Is.EqualTo(2));
        Assert.That(competition.Climbers.First().FirstName, Is.EqualTo(model1.FirstName));
        Assert.That(competition.Climbers.First().LastName, Is.EqualTo(model1.LastName));
        Assert.That(competition.Climbers.First().Club.ClubName, Is.EqualTo(model1.ClubName));
        Assert.That(competition.Climbers.First().Sex, Is.EqualTo(model1.Gender));

        Assert.That(competition.Climbers.Last().FirstName, Is.EqualTo(model2.FirstName));
        Assert.That(competition.Climbers.Last().LastName, Is.EqualTo(model2.LastName));
        Assert.That(competition.Climbers.Last().Club.ClubName, Is.EqualTo(model2.ClubName));
        Assert.That(competition.Climbers.Last().Sex, Is.EqualTo(model2.Gender));

    }
}