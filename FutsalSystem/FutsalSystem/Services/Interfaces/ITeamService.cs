using FutsalSystem.Models.DTO.Team;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FutsalSystem.Services.Interfaces
{
    public interface ITeamService
    {
        string SaveImageToSharedDirectory(string imageBase64);
        Task<IEnumerable<TeamDTO>> GetEntities();
        Task<IEnumerable<TeamDTO>> GetSortedEntities();
        Task<TeamDTO> GetEntityById(int teamId);
        Task<TeamDTO> CreateEntity(TeamDTO teamDTO);
        Task<TeamDTO> UpdateEntity(TeamDTO teamDTO);
        Task DeleteEntity(int teamId);
    }
}
