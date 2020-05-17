using System;
using FutsalSystem.Base.Interface;

namespace FutsalSystem.Models.DTO.Player
{
    public class PlayerDTO : IBaseEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int YellowCardsCount { get; set; }
        public int RedCardsCount { get; set; }
        public int MatchesPlayed { get; set; }
        public string ImagePath { get; set; }
        public int Goals { get; set; }
        public int OwnGoals { get; set; }
        public int? Number { get; set; }
        public int? TeamId { get; set; } // komandos id, kurioje zaidzia
        public string TeamName { get; set; }
    }
}
