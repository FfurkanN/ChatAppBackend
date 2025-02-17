using Microsoft.AspNetCore.Identity;

namespace ChatAppBackend.Models
{
    public sealed class AppUser : IdentityUser<Guid>
    {
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Fullname => string.Join(" ",Firstname,Lastname);

        public string ProfileImageUrl { get; set; } = string.Empty;

        public List<Guid> Chats { get; set; } = new List<Guid>();

        public string RefreshToken { get; set; } = string.Empty;
        public bool isOnline { get; set; } = false;
    }
}
