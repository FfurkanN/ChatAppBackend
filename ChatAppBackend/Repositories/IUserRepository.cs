
using ChatAppBackend.Models;
using Microsoft.AspNetCore.Identity;

namespace ChatAppBackend.Repositories
{
    public interface IUserRepository
    {  
        public Task<AppUser?> GetUserByIdAsync(Guid id);
        public Task<AppUser?> GetUserByUsernameAsync(string username);
        public Task<AppUser?> ChangeUserStatus(AppUser user, bool isOnline);
    }
}
