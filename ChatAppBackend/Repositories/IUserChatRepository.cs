using ChatAppBackend.Models;

namespace ChatAppBackend.Repositories
{
    public interface IUserChatRepository
    {
        public Task<AppUserChat> RemoveUserChat(Guid userId, Guid chatId);
        public Task<AppUserChat> AddUserChat(Guid userId, Guid chatId);

        public Task<IEnumerable<AppChat>> GetUserChatsAsync(Guid userId);

        public Task<IEnumerable<AppUser>> GetUsersFromChatAsync(Guid chatId);
    }
}
