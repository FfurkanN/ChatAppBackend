
using ChatAppBackend.Models;
using Microsoft.AspNetCore.Identity;

namespace ChatAppBackend.Repositories
{
    public interface IUserRepository
    {
        public Task<AppUser> AddChatToUserAsync(Guid userId, Guid chatId);
        public Task<AppUser> RemoveChatFromUserAsync(Guid userId, Guid chatId);
        public Task<AppUser?> GetUserByIdAsync(Guid id);
        public Task<AppUser?> GetUserByUsernameAsync(string username);
        public Task<AppUser?> ChangeUserStatus(AppUser user, bool isOnline);
        //Task<AppUser> AddUserAsync(AppUser user);
        //Task<AppUser> UpdateUserAsync(AppUser user);
        //Task<IEnumerable<AppUser>> GetUsersAsync();
        //Task<AppUser> DeleteUserAsync(string id);


    }
}
