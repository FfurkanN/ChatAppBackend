
using ChatAppBackend.Dtos;
using ChatAppBackend.Models;
using Microsoft.AspNetCore.Identity;

namespace ChatAppBackend.Repositories
{
    public interface IUserRepository
    {  
        public Task<AppUser?> GetUserByIdAsync(Guid id);
        public Task<AppUser?> GetUserByUsernameAsync(string username);
        public Task<AppUser?> ChangeUserStatus(AppUser user, bool isOnline);
        public Task<List<ChannelDto>> GetChannelsAsync(Guid id);
        public Task<List<ChannelDto>> GetOwnedChannelsAsync(Guid id);
    }
}
