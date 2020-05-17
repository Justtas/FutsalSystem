using System.Collections.Generic;
using System.Threading.Tasks;
using FutsalSystem.Models.DTO.Match;

namespace FutsalSystem.Services.Interfaces
{
    public interface IMatchService
    {
        Task<IEnumerable<MatchDTO>> GetEntities();
        Task<MatchDTO> GetEntityById(int matchId);
        Task<MatchDTO> CreateEntity(MatchDTO matchDTO);
        Task<MatchDTO> UpdateEntity(MatchDTO matchDTO);
        Task<MatchDTO> UpdateEntityMatchEvents(MatchDTO matchDTO);
        Task DeleteEntity(int matchId);
    }
}
