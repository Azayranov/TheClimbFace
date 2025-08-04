using MockQueryable.Moq;
using Moq;
using TheClimbFace.Data.Models;
using TheClimbFace.Data.Repository.Interfaces;
using TheClimbFace.Services.Data;

namespace TheClimbFace.Tests
{
    public class HomeServiceTests
    {
        private Guid competitionId;
        private Guid climberId;
        private Guid boulderId;
        private Mock<IRepository<ClimbingCompetition>> mockCompetitionRepo;
        private HomeService service;
        private ClimbingCompetition competition;
        private Climber climber;
        private Boulder boulder;
        private Club club;
        private ClimberBoulderQualification climberBoulderQualification;

        private List<ClimbingCompetition> GetClimbingCompetitionTestData()
        {
            var competitions = new List<ClimbingCompetition>();
            competitionId = Guid.NewGuid();
            climberId = Guid.NewGuid();
            boulderId = Guid.NewGuid();

            club = new Club { ClubName = "Test Club" };

            climber = new Climber
            {
                Id = climberId,
                FirstName = "Ivan",
                LastName = "Ivanov",
                StartNumber = 1,
                GroupNumber = 1,
                Sex = "Male",
                Club = club
            };

            boulder = new Boulder
            {
                Id = boulderId,
                BoulderNumber = 1
            };

            climberBoulderQualification = new ClimberBoulderQualification
            {
                ClimberId = climberId,
                BoulderId = boulderId,
                CompetitionId = competitionId,
                Climber = climber,
                Boulder = boulder,
                MaxTries = 5,
                CurrentTry = 2,
                TriesForTop = 2,
                TriesForZone = 1
            };

            competition = new ClimbingCompetition
            {
                Id = competitionId,
                Name = "Test Competition",
                Organizer = "Test Organizer",
                StartDate = new DateTime(2025, 1, 1),
                EndDate = new DateTime(2025, 1, 2),
                IsActive = true,
                Climbers = new List<Climber> { climber },
                Boulders = new List<Boulder> { boulder },
                Clubs = new List<Club> { club }
            };

            climber.ClimbersBouldersQualification = new List<ClimberBoulderQualification> { climberBoulderQualification };

            competitions.Add(competition);

            return competitions;
        }

        [SetUp]
        public void Setup()
        {
            mockCompetitionRepo = new Mock<IRepository<ClimbingCompetition>>();

            var competitions = GetClimbingCompetitionTestData();
            var mockCompetitionQueryable = competitions.AsQueryable().BuildMock();

            mockCompetitionRepo.Setup(x => x.GetAllAttached()).Returns(mockCompetitionQueryable);

            service = new HomeService(mockCompetitionRepo.Object);
        }

        [Test]
        public async Task GetActiveCompetitionsAsync_ShouldReturnActiveCompetitions()
        {
            var result = await service.GetActiveCompetitionsAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            
            var firstCompetition = result.First();
            Assert.That(firstCompetition.Name, Is.EqualTo(competition.Name));
            Assert.That(firstCompetition.Organizer, Is.EqualTo(competition.Organizer));
        }

        [Test]
        public async Task GetActiveCompetitionsAsync_ShouldNotReturnInactiveCompetitions()
        {
            competition.IsActive = false;

            var result = await service.GetActiveCompetitionsAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task GetCompetitionDetailsAsync_ShouldReturnCompetitionDetails()
        {
            var result = await service.GetCompetitionDetailsAsync(competitionId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(competitionId));
            Assert.That(result.Name, Is.EqualTo(competition.Name));
            Assert.That(result.Organizer, Is.EqualTo(competition.Organizer));
            Assert.That(result.StartDate, Is.EqualTo(competition.StartDate));
            Assert.That(result.EndDate, Is.EqualTo(competition.EndDate));
        }

        [Test]
        public async Task GetCompetitionDetailsAsync_ShouldReturnClimbersWithCorrectData()
        {
            var result = await service.GetCompetitionDetailsAsync(competitionId);

            Assert.That(result.Climbers, Is.Not.Null);
            Assert.That(result.Climbers.Count, Is.EqualTo(1));

            var climberResult = result.Climbers.First();
            Assert.That(climberResult.Id, Is.EqualTo(climberId.ToString()));
            Assert.That(climberResult.FirstName, Is.EqualTo(climber.FirstName));
            Assert.That(climberResult.LastName, Is.EqualTo(climber.LastName));
            Assert.That(climberResult.ClubName, Is.EqualTo(club.ClubName));
            Assert.That(climberResult.Group, Is.EqualTo(climber.GroupNumber));
            Assert.That(climberResult.Gender, Is.EqualTo(climber.Sex));
        }

        [Test]
        public async Task GetCompetitionDetailsAsync_ShouldCalculateClimberStatistics()
        {
            var result = await service.GetCompetitionDetailsAsync(competitionId);

            var climberResult = result.Climbers.First();
            
            
            Assert.That(climberResult.TopTries, Is.EqualTo(2)); 
            Assert.That(climberResult.ZoneTries, Is.EqualTo(1)); 
            Assert.That(climberResult.Tops, Is.EqualTo(1)); 
            Assert.That(climberResult.Zones, Is.EqualTo(1)); 
        }

        [Test]
        public async Task GetCompetitionDetailsAsync_ShouldReturnBouldersWithCorrectData()
        {
            var result = await service.GetCompetitionDetailsAsync(competitionId);

            var climberResult = result.Climbers.First();
            Assert.That(climberResult.Boulders, Is.Not.Null);
            Assert.That(climberResult.Boulders.Count, Is.EqualTo(1));

            var boulderResult = climberResult.Boulders.First();
            Assert.That(boulderResult.Id, Is.EqualTo(boulderId.ToString()));
            Assert.That(boulderResult.Number, Is.EqualTo(boulder.BoulderNumber));
            Assert.That(boulderResult.TriesForTop, Is.EqualTo(climberBoulderQualification.TriesForTop));
            Assert.That(boulderResult.TriesForZone, Is.EqualTo(climberBoulderQualification.TriesForZone));
        }

        [Test]
        public async Task GetCompetitionDetailsAsync_ShouldOrderClimbersByPerformance()
        {
            
            var secondClimber = new Climber
            {
                Id = Guid.NewGuid(),
                FirstName = "Petar",
                LastName = "Petrov",
                StartNumber = 2,
                GroupNumber = 1,
                Sex = "Male",
                Club = club
            };

            var secondBoulder = new Boulder
            {
                Id = Guid.NewGuid(),
                BoulderNumber = 2
            };

            var secondClimberBoulderQualification = new ClimberBoulderQualification
            {
                ClimberId = secondClimber.Id,
                BoulderId = secondBoulder.Id,
                CompetitionId = competitionId,
                Climber = secondClimber,
                Boulder = secondBoulder,
                TriesForTop = 1, 
                TriesForZone = 1
            };

            secondClimber.ClimbersBouldersQualification = new List<ClimberBoulderQualification> { secondClimberBoulderQualification };
            competition.Climbers.Add(secondClimber);

            var result = await service.GetCompetitionDetailsAsync(competitionId);

            Assert.That(result.Climbers.Count, Is.EqualTo(2));
            
            
            Assert.That(result.Climbers.First().FirstName, Is.EqualTo("Petar"));
            Assert.That(result.Climbers.Last().FirstName, Is.EqualTo("Ivan"));
        }

        [Test]
        public async Task GetCompetitionDetailsAsync_WhenClimberHasNoBoulders_ShouldHandleCorrectly()
        {
            
            climber.ClimbersBouldersQualification = new List<ClimberBoulderQualification>();

            var result = await service.GetCompetitionDetailsAsync(competitionId);

            var climberResult = result.Climbers.First();
            Assert.That(climberResult.Boulders.Count, Is.EqualTo(0));
            Assert.That(climberResult.TopTries, Is.EqualTo(0));
            Assert.That(climberResult.ZoneTries, Is.EqualTo(0));
            Assert.That(climberResult.Tops, Is.EqualTo(0));
            Assert.That(climberResult.Zones, Is.EqualTo(0));
        }
    }
}