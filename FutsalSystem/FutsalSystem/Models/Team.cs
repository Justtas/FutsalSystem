using FutsalSystem.Base.Interface;
using System.Collections.Generic;

namespace FutsalSystem.Models
{
    public class Team : IBaseEntity
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
        public virtual IEnumerable<Match> HomeMatches { get; set; }
        public virtual IEnumerable<Match> AwayMatches { get; set; }

        public virtual IEnumerable<Player> Players { get; set; }
        public int GoalsDifference => GoalsFor - GoalsAgainst;
    }
}
