using MockQueryable.Moq;
using Moq;
using TheClimbFace.Data.Models;
using TheClimbFace.Data.Repository.Interfaces;
using TheClimbFace.Services.Data;

namespace TheClimbFace.Tests
{
    public class BoulderScoringServiceTests
    {
        private Guid competitionId;
        private Guid climberId;
        private Guid boulderId;
        private Guid userId;
        private Mock<IRepository<ClimbingCompetition>> mockCompetitionRepo;
        private Mock<IRepository<ClimberBoulderQualification>> mockClimberBoulderQualificationRepo;
        private BoulderScoringService service;
        private ClimbingCompetition competition;
        private Climber climber;
        private Boulder boulder;
        private ApplicationUser user;
        private ClimberBoulderQualification climberBoulderQualification;

        private List<ClimbingCompetition> GetClimbingCompetitionTestData()
        {
            var competitions = new List<ClimbingCompetition>();
            competitionId = Guid.NewGuid();
            climberId = Guid.NewGuid();
            boulderId = Guid.NewGuid();
            userId = Guid.NewGuid();

            var club = new Club { ClubName = "Test Club" };

            climber = new Climber
            {
                Id = climberId,
                FirstName = "Ivan",
                LastName = "Ivanov",
                StartNumber = 1,
                GroupNumber = 1,
                Club = club
            };

            boulder = new Boulder
            {
                Id = boulderId,
                BoulderNumber = 1
            };

            user = new ApplicationUser
            {
                Id = userId,
                Email = "arbitrator@test.com"
            };

            climberBoulderQualification = new ClimberBoulderQualification
            {
                ClimberId = climberId,
                BoulderId = boulderId,
                CompetitionId = competitionId,
                Climber = climber,
                Boulder = boulder,
                MaxTries = 5,
                CurrentTry = 0,
                TriesForTop = 0,
                TriesForZone = 0
            };

            competition = new ClimbingCompetition
            {
                Id = competitionId,
                Name = "Test Competition",
                ClimbersBouldersQualifications = new List<ClimberBoulderQualification> { climberBoulderQualification },
                Arbitrators = new List<Arbitrator>
                {
                    new Arbitrator
                    {
                        UserId = userId,
                        Name = "Test Arbitrator",
                        AssignedBoulderNumber = 1,
                        User = user
                    }
                }
            };

            competitions.Add(competition);

            return competitions;
        }

        [SetUp]
        public void Setup()
        {
            mockCompetitionRepo = new Mock<IRepository<ClimbingCompetition>>();
            mockClimberBoulderQualificationRepo = new Mock<IRepository<ClimberBoulderQualification>>();

            var competitions = GetClimbingCompetitionTestData();
            var mockCompetitionQueryable = competitions.AsQueryable().BuildMock();

            mockCompetitionRepo.Setup(x => x.GetAllAttached()).Returns(mockCompetitionQueryable);
            mockClimberBoulderQualificationRepo.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            service = new BoulderScoringService(mockCompetitionRepo.Object, mockClimberBoulderQualificationRepo.Object);
        }

        [Test]
        public async Task GetClimberBoulderAsync_ShouldReturnClimberBoulder()
        {

            var result = await service.GetClimberBoulderAsync(competitionId, 1, 1);


            Assert.That(result, Is.Not.Null);
            Assert.That(result.ClimberId, Is.EqualTo(climberId));
            Assert.That(result.BoulderId, Is.EqualTo(boulderId));
            Assert.That(result.Climber.StartNumber, Is.EqualTo(1));
            Assert.That(result.Boulder.BoulderNumber, Is.EqualTo(1));
        }

        [Test]
        public async Task GetClimberForScoringAsync_ShouldReturnClimberViewModel()
        {

            var result = await service.GetClimberForScoringAsync(competitionId, 1, 1);


            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Ivan Ivanov"));
            Assert.That(result.Club, Is.EqualTo("Test Club"));
            Assert.That(result.Group, Is.EqualTo(1));
            Assert.That(result.StartNumber, Is.EqualTo(1));
            Assert.That(result.MaxAttempts, Is.EqualTo(5));
            Assert.That(result.CurrentAttempt, Is.EqualTo(0));
            Assert.That(result.TriesForTop, Is.EqualTo(0));
            Assert.That(result.TriesForZone, Is.EqualTo(0));
        }

        [Test]
        public async Task GetScoreViewModelAsync_ShouldReturnScoreViewModel()
        {

            var result = await service.GetScoreViewModelAsync(competitionId, userId, 1);


            Assert.That(result, Is.Not.Null);
            Assert.That(result.CompetitionId, Is.EqualTo(competitionId.ToString()));
            Assert.That(result.BoulderNumber, Is.EqualTo(1));
            Assert.That(result.StartNumber, Is.EqualTo(0));
            Assert.That(result.CurrentClimber, Is.Null);
        }

        [Test]
        public async Task GetScoreViewModelWithClimberAsync_ShouldReturnScoreViewModelWithClimber()
        {

            var result = await service.GetScoreViewModelWithClimberAsync(competitionId, userId, 1, 1);


            Assert.That(result, Is.Not.Null);
            Assert.That(result.CompetitionId, Is.EqualTo(competitionId.ToString()));
            Assert.That(result.BoulderNumber, Is.EqualTo(1));
            Assert.That(result.StartNumber, Is.EqualTo(1));
            Assert.That(result.CurrentClimber, Is.Not.Null);
            Assert.That(result.CurrentClimber.Name, Is.EqualTo("Ivan Ivanov"));
        }

        [Test]
        public async Task SetFailForClimberAsync_ShouldIncrementCurrentTry()
        {

            var initialCurrentTry = climberBoulderQualification.CurrentTry;


            await service.SetFailForClimberAsync(competitionId, 1, 1);


            Assert.That(climberBoulderQualification.CurrentTry, Is.EqualTo(initialCurrentTry + 1));
            mockClimberBoulderQualificationRepo.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task SetTopForClimberAsync_ShouldSetTopAndZone()
        {

            var initialCurrentTry = climberBoulderQualification.CurrentTry;


            await service.SetTopForClimberAsync(competitionId, 1, 1);


            Assert.That(climberBoulderQualification.CurrentTry, Is.EqualTo(initialCurrentTry + 1));
            Assert.That(climberBoulderQualification.TriesForTop, Is.EqualTo(initialCurrentTry + 1));
            Assert.That(climberBoulderQualification.TriesForZone, Is.EqualTo(initialCurrentTry + 1));
            mockClimberBoulderQualificationRepo.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task SetZoneForClimberAsync_ShouldSetZoneOnly()
        {

            var initialCurrentTry = climberBoulderQualification.CurrentTry;


            await service.SetZoneForClimberAsync(competitionId, 1, 1);


            Assert.That(climberBoulderQualification.CurrentTry, Is.EqualTo(initialCurrentTry + 1));
            Assert.That(climberBoulderQualification.TriesForZone, Is.EqualTo(initialCurrentTry + 1));
            Assert.That(climberBoulderQualification.TriesForTop, Is.EqualTo(0));
            mockClimberBoulderQualificationRepo.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task SetTopForClimberAsync_WhenZoneAlreadySet_ShouldNotChangeZone()
        {

            climberBoulderQualification.TriesForZone = 2;
            var initialZoneTries = climberBoulderQualification.TriesForZone;
            var initialCurrentTry = climberBoulderQualification.CurrentTry;


            await service.SetTopForClimberAsync(competitionId, 1, 1);


            Assert.That(climberBoulderQualification.CurrentTry, Is.EqualTo(initialCurrentTry + 1));
            Assert.That(climberBoulderQualification.TriesForTop, Is.EqualTo(initialCurrentTry + 1));
            Assert.That(climberBoulderQualification.TriesForZone, Is.EqualTo(initialZoneTries));
            mockClimberBoulderQualificationRepo.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}