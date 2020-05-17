using System;
using FutsalSystem.Base.Interface;
using System.ComponentModel.DataAnnotations.Schema;

namespace FutsalSystem.Models
{
    public class Player : IBaseEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Column(TypeName="date")]
        public DateTime DateOfBirth { get; set; }
        public string ImagePath { get; set; }
        public int YellowCardsCount { get; set; }
        public int RedCardsCount { get; set; }
        public int MatchesPlayed { get; set; }
        public int Goals { get; set; }
        public int OwnGoals { get; set; }
        public int? Number { get; set; }
        public int? TeamId { get; set; }

        [ForeignKey("TeamId")]
        public virtual Team Team { get; set; }
    }
}
