using System;
using System.Collections.Generic;

namespace FutsalSystem.Models.DTO.Match
{
    public class MatchDTO
    {
        public int Id { get; set; }
        public int HomeTeamId { get; set; }
        public string HomeTeam { get; set; }
        public int AwayTeamId { get; set; }
        public string AwayTeam { get; set; }
        public DateTime MatchDate { get; set; }
        //public int Round { get; set; } // Turas
        public int HomeTeamFirstHalfScore { get; set; }
        public int AwayTeamFirstHalfScore { get; set; }
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }
        public bool IsFinished { get; set; }
        public IEnumerable<Models.MatchEvent> MatchEvents { get; set; }
    }
}
