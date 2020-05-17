using System.ComponentModel.DataAnnotations.Schema;
using FutsalSystem.Base.Interface;
using FutsalSystem.Enumerators;

namespace FutsalSystem.Models
{
    public class MatchEvent : IBaseEntity
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        [ForeignKey("MatchId")]
        public virtual Match Match { get; set; }
        public int Minute { get; set; }
        public int PlayerId { get; set; }

        [ForeignKey("PlayerId")]
        public virtual Player Player { get; set; }
        public PlayerEvent EventType { get; set; }
    }
}
