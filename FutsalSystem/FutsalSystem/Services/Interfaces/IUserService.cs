using FutsalSystem.Models.DTO.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FutsalSystem.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetEntities();
        Task<UserDTO> GetEntityById(int userId);
        Task<UserDTO> CreateEntity(UserDTO userDTO);
        Task<UserDTO> UpdateEntity(UserDTO userDTO);
        Task DeleteEntity(int userId);
    }
}
