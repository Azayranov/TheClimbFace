using System;

namespace TheClimbFace.Web.ViewModels.Competition;

public class CompetitionListViewModel
{
    public IEnumerable<CompetitionViewModel> ActiveCompetitions { get; set; } = new List<CompetitionViewModel>();
    public IEnumerable<CompetitionViewModel> InactiveCompetitions { get; set; } = new List<CompetitionViewModel>();
    public IEnumerable<ArbitratorCompetition> ArbitratorCompetitions { get; set; } = new List<ArbitratorCompetition>();
}
