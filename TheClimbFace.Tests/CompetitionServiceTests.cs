using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using TheClimbFace.Data.Models;
using TheClimbFace.Data.Models.Enums;
using TheClimbFace.Data.Repository.Interfaces;
using TheClimbFace.Services.Data;
using TheClimbFace.Web.ViewModels;
using TheClimbFace.Web.ViewModels.Competition;
namespace TheClimbFace.Tests
{
    public class CompetitionServiceTests
    {
        private CompetitionService competitionService;
        private Mock<IRepository<ClimbingCompetition>> mockCompetitionRepo;
        private Mock<IRepository<Boulder>> mockBoulderRepo;
        private Mock<IRepository<ClimberBoulderQualification>> mockClimberBoulderQualificationRepo;
        private Mock<IRepository<Climber>> mockClimberRepo;
        private Mock<IRepository<Club>> mockClubRepo;
        private Guid competitionId;
        private ClimbingCompetition competition;



        [SetUp]
        public void Setup()
        {
            mockCompetitionRepo = new Mock<IRepository<ClimbingCompetition>>();
            mockBoulderRepo = new Mock<IRepository<Boulder>>();
            mockClimberBoulderQualificationRepo = new Mock<IRepository<ClimberBoulderQualification>>();
            mockClimberRepo = new Mock<IRepository<Climber>>();
            mockClubRepo = new Mock<IRepository<Club>>();

            var competitions = GetClimbingCompetitionTestData();
            var mockCompetitionQueryable = competitions.AsQueryable().BuildMock();

            mockCompetitionRepo.Setup(r => r.GetAllAttached()).Returns(mockCompetitionQueryable);

            competitionService = new CompetitionService(mockCompetitionRepo.Object, mockBoulderRepo.Object, mockClimberBoulderQualificationRepo.Object, mockClimberRepo.Object, mockClubRepo.Object);

        }

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
                IsActive = false
            };

            competitions.Add(competition);

            return competitions;
        }

        [Test]
        public async Task EditCompetitionAsync_ShouldEditCompetition()
        {
            var editModel = new CreateCompetitionInputModel()
            {
                Name = "edited",
                Organizer = "edited",
                Information = "edited",
            };

            var StartDate = new DateTime(2025, 1, 1);
            var EndDate = new DateTime(2025, 1, 1);

            await competitionService.EditCompetitionAsync(editModel, StartDate, EndDate, competitionId);

            Assert.That(competition.Name, Is.EqualTo(editModel.Name));
            Assert.That(competition.Organizer, Is.EqualTo(editModel.Organizer));
            Assert.That(competition.Information, Is.EqualTo(editModel.Information));
        }

        [Test]
        public async Task CreateCompetitionAsync_ShouldCreateCompetition()
        {
            var createModel = new CreateCompetitionInputModel()
            {
                Name = "New Competition",
                Organizer = "Test Organizer",
                Information = "Test Information",
                Type = CompetitionType.Boulder,
                RouteCount = 5
            };

            var StartDate = new DateTime(2025, 1, 1);
            var EndDate = new DateTime(2025, 1, 2);
            var userId = Guid.NewGuid();

            ClimbingCompetition capturedCompetition = null;
            mockCompetitionRepo.Setup(r => r.AddAsync(It.IsAny<ClimbingCompetition>()))
                .Callback<ClimbingCompetition>(competition => capturedCompetition = competition);

            await competitionService.CreateCompetitionAsync(createModel, StartDate, EndDate, userId);

            Assert.That(capturedCompetition, Is.Not.Null);
            Assert.That(capturedCompetition.Name, Is.EqualTo(createModel.Name));
            Assert.That(capturedCompetition.Organizer, Is.EqualTo(createModel.Organizer));
            Assert.That(capturedCompetition.Information, Is.EqualTo(createModel.Information));
            Assert.That(capturedCompetition.Type, Is.EqualTo(createModel.Type));
            Assert.That(capturedCompetition.StartDate, Is.EqualTo(StartDate));
            Assert.That(capturedCompetition.EndDate, Is.EqualTo(EndDate));
            Assert.That(capturedCompetition.ApplicationUserId, Is.EqualTo(userId));
        }

        [Test]
        public async Task DeleteCompetitionAsync_ShouldDeleteCompetition()
        {
            mockCompetitionRepo.Setup(r => r.DeleteAsync(competitionId))
                .ReturnsAsync(true);

            await competitionService.DeleteCompetitionAsync(competitionId);

            mockCompetitionRepo.Verify(r => r.DeleteAsync(competitionId), Times.Once);
        }

        [Test]
        public async Task GetCompetitionAsync_ShouldReturnCompetition()
        {
            var expectedCompetition = new ClimbingCompetition
            {
                Id = competitionId,
                Name = "Test Competition",
                Organizer = "Test Organizer",
                Information = "Test Information",
                StartDate = new DateTime(2025, 1, 15),
                EndDate = new DateTime(2025, 2, 20),
                RouteCount = 5,
                ApplicationUserId = Guid.NewGuid()
            };

            mockCompetitionRepo.Setup(r => r.GetByIdAsync(competitionId))
                .ReturnsAsync(expectedCompetition);

            var result = await competitionService.GetCompetitionAsync(competitionId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(expectedCompetition.Name));
            Assert.That(result.Organizer, Is.EqualTo(expectedCompetition.Organizer));
            Assert.That(result.Information, Is.EqualTo(expectedCompetition.Information));
            Assert.That(result.RouteCount, Is.EqualTo(expectedCompetition.RouteCount));
            Assert.That(result.ApplicationUserId, Is.EqualTo(expectedCompetition.ApplicationUserId));

            Assert.That(result.StartDay, Is.EqualTo(expectedCompetition.StartDate.Day));
            Assert.That(result.StartMonth, Is.EqualTo(expectedCompetition.StartDate.ToString("MMMM")));
            Assert.That(result.StartYear, Is.EqualTo(expectedCompetition.StartDate.Year));
            Assert.That(result.EndDay, Is.EqualTo(expectedCompetition.EndDate.Day));
            Assert.That(result.EndMonth, Is.EqualTo(expectedCompetition.EndDate.ToString("MMMM")));
            Assert.That(result.EndYear, Is.EqualTo(expectedCompetition.EndDate.Year));
        }

        [Test]
        public async Task StartCompetitionAsync_IsActiveShouldBeTrue()
        {
            await competitionService.StartCompetitionAsync(competitionId);

            Assert.That(competition.IsActive, Is.True);
        }

        [Test]
        public async Task StopCompetitionAsync_IsActiveShouldBeFalse()
        {
            competition.IsActive = true;

            await competitionService.StopCompetitionAsync(competitionId);

            Assert.That(competition.IsActive, Is.False);
        }

    }
}