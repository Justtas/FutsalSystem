using FutsalSystem.Models.DTO.Announcement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FutsalSystem.Services.Interfaces
{
    public interface IAnnouncementService
    {
        Task<IEnumerable<AnnouncementDTO>> GetEntities();
        Task<AnnouncementDTO> GetEntityById(int announcementId);
        Task<AnnouncementDTO> CreateEntity(AnnouncementDTO announcementDTO);
        Task<AnnouncementDTO> UpdateEntity(AnnouncementDTO announcementDTO);
        Task DeleteEntity(int announcementId);
    }
}
