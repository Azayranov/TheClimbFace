using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using TheClimbFace.Data.Models;
using TheClimbFace.Data.Repository.Interfaces;
using TheClimbFace.Services.Data;
using TheClimbFace.Web.ViewModels.Competition.Arbitrator;

namespace TheClimbFace.Tests
{
    public class ArbitratorServiceTests
    {
        private Guid competitionId;
        private Guid userId;
        private Mock<IRepository<ClimbingCompetition>> mockCompetitionRepo;
        private Mock<IRepository<ApplicationUser>> mockUserRepo;
        private ArbitratorService service;
        private ClimbingCompetition competition;
        private ApplicationUser user;
        private List<ClimbingCompetition> GetClimbingCompetitionTestData()
        {
            var competitions = new List<ClimbingCompetition>();
            competitionId = Guid.NewGuid();
            var clubName = "Test Club";

            var club = new Club { ClubName = clubName };

            competition = new ClimbingCompetition
            {
                Id = competitionId,
                Name = "Test Competition",
                Clubs = new List<Club> { club },
                Climbers = new List<Climber>(),
                Arbitrators = new List<Arbitrator>()
            };

            competitions.Add(competition);

            return competitions;
        }

        private List<ApplicationUser> GetUserTestData()
        {
            var users = new List<ApplicationUser>();
            userId = Guid.NewGuid();

            user = new ApplicationUser
            {
                Id = userId,
                Email = "alex12@gmail.com",
                UserName = "alex12@gmail.com"
            };

            users.Add(user);

            return users;
        }

        [SetUp]
        public void Setup()
        {
            mockCompetitionRepo = new Mock<IRepository<ClimbingCompetition>>();
            mockUserRepo = new Mock<IRepository<ApplicationUser>>();

            var competitions = GetClimbingCompetitionTestData();
            var users = GetUserTestData();

            var mockCompetitionQueryable = competitions.AsQueryable().BuildMock();
            var mockUserQueryable = users.AsQueryable().BuildMock();

            mockCompetitionRepo.Setup(x => x.GetAllAttached()).Returns(mockCompetitionQueryable);
            mockUserRepo.Setup(x => x.GetAllAttached()).Returns(mockUserQueryable);

            service = new ArbitratorService(mockCompetitionRepo.Object, mockUserRepo.Object);
        }

        [Test]
        public async Task AddArbitratorToCompetitionAsync_ShouldAddArbitrator()
        {
            var initialArbitratorCount = competition.Arbitrators.Count;

            var addModel = new AddArbitratorInputModel()
            {
                Name = "Test",
                Email = "alex12@gmail.com",
                AssignedBoulderNumber = 1,
            };

            await service.AddArbitratorToCompetitionAsync(competitionId, addModel);

            Assert.That(initialArbitratorCount, Is.EqualTo(0));
            Assert.That(competition.Arbitrators.Count, Is.EqualTo(1));
            Assert.That(competition.Arbitrators.First().Name, Is.EqualTo(addModel.Name));
            Assert.That(competition.Arbitrators.First().UserId, Is.EqualTo(userId));
            Assert.That(competition.Arbitrators.First().AssignedBoulderNumber, Is.EqualTo(1));
        }

        [Test]
        public async Task DeleteArbitratorFromCompetitionAsync()
        {

            var addModel = new AddArbitratorInputModel()
            {
                Name = "Test",
                Email = "alex12@gmail.com",
                AssignedBoulderNumber = 1,
            };

            await service.AddArbitratorToCompetitionAsync(competitionId, addModel);

            var initialArbitratorCount = competition.Arbitrators.Count;
            await service.DeleteArbitratorFromCompetitionAsync(competitionId, userId);

            Assert.That(initialArbitratorCount, Is.EqualTo(1));
            Assert.That(competition.Arbitrators.Count, Is.Zero);
        }

        [Test]
        public async Task GetCompetitionArbitratorsAsync_ShouldReturnAllArbitrators()
        {

            var arbitrator1 = new Arbitrator
            {
                UserId = Guid.NewGuid(),
                Name = "Test1",
                AssignedBoulderNumber = 1,
                User = new ApplicationUser { Email = "alex1@gmail.com" }
            };

            var arbitrator2 = new Arbitrator
            {
                UserId = Guid.NewGuid(),
                Name = "Test2",
                AssignedBoulderNumber = 2,
                User = new ApplicationUser { Email = "alex1@gmail.com" }
            };

            competition.Arbitrators.Add(arbitrator1);
            competition.Arbitrators.Add(arbitrator2);

            var result = await service.GetCompetitionArbitratorsAsync(competitionId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Arbitrators.Count, Is.EqualTo(2));

            Assert.That(result.Arbitrators.First().Name, Is.EqualTo(arbitrator1.Name));
            Assert.That(result.Arbitrators.First().AssignedBoulderNumber, Is.EqualTo(arbitrator1.AssignedBoulderNumber));

            Assert.That(result.Arbitrators.Last().Name, Is.EqualTo(arbitrator2.Name));
            Assert.That(result.Arbitrators.Last().AssignedBoulderNumber, Is.EqualTo(arbitrator2.AssignedBoulderNumber));
        }

        [Test]
        public async Task GetAvailableBouldersAsync()
        {
            var boulder1 = new Boulder
            {
                Id = Guid.NewGuid(),
                BoulderNumber = 1,
                CompetitionId = competitionId
            };

            var boulder2 = new Boulder
            {
                Id = Guid.NewGuid(),
                BoulderNumber = 2,
                CompetitionId = competitionId
            };

            competition.Boulders.Add(boulder1);
            competition.Boulders.Add(boulder2);


            var result = await service.GetAvailableBouldersAsync(competitionId);

            Assert.That(result.Count, Is.EqualTo(2));

            Assert.That(result.First().Id, Is.EqualTo(boulder1.Id.ToString()));
            Assert.That(result.First().Number, Is.EqualTo(boulder1.BoulderNumber));

            Assert.That(result.Last().Id, Is.EqualTo(boulder2.Id.ToString()));
            Assert.That(result.Last().Number, Is.EqualTo(boulder2.BoulderNumber));
        }

    }
}