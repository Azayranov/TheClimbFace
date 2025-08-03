using System;
using Microsoft.EntityFrameworkCore;
using TheClimbFace.Data.Models;
using TheClimbFace.Data.Repository.Interfaces;
using TheClimbFace.Services.Data.Interfaces;
using TheClimbFace.Web.ViewModels.Competition.Arbitrator;
using TheClimbFace.Web.ViewModels.Home;

namespace TheClimbFace.Services.Data;

public class ArbitratorService(IRepository<ClimbingCompetition> competitionRepository, IRepository<ApplicationUser> userRepository) : IArbitratorService
{
    public async Task DeleteArbitratorFromCompetitionAsync(Guid competitionId, Guid userId)
    {
        ClimbingCompetition? competition = await competitionRepository.GetAllAttached()
            .Where(x => x.Id == competitionId)
            .Include(x => x.Arbitrators)
            .FirstOrDefaultAsync();

        Arbitrator arbitrator = competition!.Arbitrators.Where(x => x.UserId == userId).FirstOrDefault()!;

        competition.Arbitrators.Remove(arbitrator);
        await competitionRepository.SaveChangesAsync();
    }

    public async Task<bool> AddArbitratorToCompetitionAsync(Guid competitionId, AddArbitratorInputModel model)
    {
        ClimbingCompetition? competition = await competitionRepository
            .GetAllAttached()
            .Where(x => x.Id == competitionId)
            .Include(x => x.Arbitrators)
            .FirstOrDefaultAsync();

        var user = await userRepository.GetAllAttached()
            .Where(x => x.Email.ToLower() == model.Email.ToLower())
            .FirstOrDefaultAsync();

        if (user == null)
            return false;

        Arbitrator arbitrator = new()
        {
            UserId = user.Id,
            Name = model.Name,
            AssignedBoulderNumber = model.AssignedBoulderNumber
        };


        competition!.Arbitrators.Add(arbitrator);

        await competitionRepository.SaveChangesAsync();
        return true;
    }


    public async Task<ArbitratorsViewModel> GetCompetitionArbitratorsAsync(Guid competitionId)
    {
        ClimbingCompetition? competition = await competitionRepository.GetAllAttached()
            .Where(x => x.Id == competitionId)
            .Include(x => x.Arbitrators)
            .ThenInclude(x => x.User)
            .Include(x => x.Boulders)
            .FirstOrDefaultAsync();



        List<ArbitratorViewModel> arbitrators = new();

        foreach (var a in competition!.Arbitrators)
        {
            ArbitratorViewModel arbitrator = new()
            {
                Id = a.UserId.ToString(),
                Name = a.Name,
                Email = a.User.Email!,
                AssignedBoulderNumber = a.AssignedBoulderNumber
            };

            arbitrators.Add(arbitrator);
        }


        ArbitratorsViewModel model = new()
        {
            CompetitionId = competitionId.ToString(),
            CompetitionName = competition.Name,
            Arbitrators = arbitrators,
        };

        return model;
    }

    public async Task<List<BoulderViewModel>> GetAvailableBouldersAsync(Guid competitionId)
    {
        ClimbingCompetition? competition = await competitionRepository
            .GetAllAttached()
            .Where(x => x.Id == competitionId)
            .Include(x => x.Arbitrators)
            .FirstOrDefaultAsync();

        List<BoulderViewModel> boulders = new();

        foreach (var b in competition.Boulders)
        {
            BoulderViewModel boulder = new()
            {
                Id = b.Id.ToString(),
                Number = b.BoulderNumber
            };

            boulders.Add(boulder);
        }

        return boulders;
    }

    public async Task<AddArbitratorInputModel> GetAddArbitratorModelAsync(Guid competitionId)
    {

        ClimbingCompetition? competition = await competitionRepository.GetAllAttached()
            .Where(x => x.Id == competitionId)
            .Include(x => x.Boulders)
            .FirstOrDefaultAsync();

        var model = new AddArbitratorInputModel();

        List<BoulderViewModel> boulders = await GetAvailableBouldersAsync(competitionId);

        model.AvailableBoulders = boulders;
        model.CompetitionId = competitionId.ToString();

        return model;
    }
}
