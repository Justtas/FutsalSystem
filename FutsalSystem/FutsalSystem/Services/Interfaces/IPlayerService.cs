using System.Collections.Generic;
using System.Threading.Tasks;
using FutsalSystem.Models.DTO.Player;

namespace FutsalSystem.Services.Interfaces
{
    public interface IPlayerService
    {
        Task<IEnumerable<PlayerDTO>> GetEntities();
        Task<PlayerDTO> GetEntityById(int playerId);
        Task<IEnumerable<PlayerDTO>> GetEntitiesByTeamId(int teamId);
        Task<PlayerDTO> CreateEntity(PlayerDTO playerDTO);
        Task<PlayerDTO> UpdateEntity(PlayerDTO playerDTO);
        Task DeleteEntity(int playerId);
        string SaveImageToSharedDirectory(string imageBase64);
    }
}
