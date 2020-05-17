using FutsalSystem.Base.Interface;

namespace FutsalSystem.Models.DTO.Team
{
    public class TeamDTO : IBaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Points { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int MatchesPlayed { get; set; }
        public int MatchesWon { get; set; }
        public int MatchesDrawn { get; set; }
        public int MatchesLost { get; set; }
        public string ImagePath { get; set; }
        public int GoalsDifference => GoalsFor - GoalsAgainst;

    }
}
