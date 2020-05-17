using FutsalSystem.Models.DTO.MatchEvent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FutsalSystem.Services.Interfaces
{
    public interface IMatchEventService
    {
        Task<IEnumerable<MatchEventDTO>> GetEntities(int matchId);
        Task<MatchEventDTO> GetEntityById(int matchId, int matchEventId);
        Task<MatchEventDTO> CreateEntity(MatchEventDTO matchEventDTO);
        Task<MatchEventDTO> UpdateEntity(MatchEventDTO matchEventDTO, int matchId, int matchEventId);
        Task DeleteEntity(int matchEventId);
    }
}
