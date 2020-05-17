using FutsalSystem.Base.Interface;
using System;
using System.Collections.Generic;

namespace FutsalSystem.Models
{
    public class Match : IBaseEntity
    {
        public int Id { get; set; }
        public int HomeTeamId { get; set; }
        public virtual Team HomeTeam { get; set; }
        public int AwayTeamId { get; set; }
        public virtual Team AwayTeam { get; set; }
        public DateTime MatchDate { get; set; }
        public int HomeTeamFirstHalfScore { get; set; }
        public int AwayTeamFirstHalfScore { get; set; }
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }
        public bool IsFinished { get; set; }
        public virtual IEnumerable<MatchEvent> MatchEvents { get; set; }
    }
}
