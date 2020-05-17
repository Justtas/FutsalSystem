using FutsalSystem.Base.Interface;

namespace FutsalSystem.Models.DTO.Announcement
{
    public class AnnouncementDTO : IBaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CreationDate { get; set; }
        public string Message { get; set; }
    }
}
