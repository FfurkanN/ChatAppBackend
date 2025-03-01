using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace ChatAppBackend.Models
{
    public  class AppUser : IdentityUser<Guid>
    {
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Fullname => string.Join(" ",Firstname,Lastname);
        public string ProfileImageUrl { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public bool isOnline { get; set; } = false;
        public string About { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<AppChannelUser> ChannelUser { get; set; }
    }
}
