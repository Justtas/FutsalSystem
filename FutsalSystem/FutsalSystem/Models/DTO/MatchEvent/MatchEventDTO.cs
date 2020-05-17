using FutsalSystem.Base.Interface;
using FutsalSystem.Enumerators;
using System.ComponentModel.DataAnnotations.Schema;

namespace FutsalSystem.Models.DTO.MatchEvent
{
    public class MatchEventDTO : IBaseEntity
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        [ForeignKey("MatchId")]
        public Models.Match Match { get; set; }
        public int Minute { get; set; }
        public int PlayerId { get; set; }

        [ForeignKey("PlayerId")]
        public Models.Player Player { get; set; }
        public string PlayerName { get; set; }
        public string TeamName { get; set; }
        public PlayerEvent EventType { get; set; }
    }
}
