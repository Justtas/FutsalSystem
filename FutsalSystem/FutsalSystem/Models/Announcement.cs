using FutsalSystem.Base.Interface;

namespace FutsalSystem.Models
{
    public class Announcement : IBaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CreationDate { get; set; }
        public string Message { get; set; }
    }
}
