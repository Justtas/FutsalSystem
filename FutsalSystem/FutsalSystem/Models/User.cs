using FutsalSystem.Base.Interface;

namespace FutsalSystem.Models
{
    public class User : IBaseEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
