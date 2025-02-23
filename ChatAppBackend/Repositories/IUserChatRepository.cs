using ChatAppBackend.Dtos;
using ChatAppBackend.Models;

namespace ChatAppBackend.Repositories
{
    public interface IUserChatRepository
    {
        public Task<AppChannelUser> RemoveUserChat(Guid userId, Guid chatId);
        public Task<IEnumerable<AppChannelUser>> AddUserChat(Guid[] userId, Guid chatId);
        public Task<IEnumerable<ChatDto>> GetUserChatsAsync(Guid userId);
        public Task<IEnumerable<ChatDto>> GetOwnedChatsAsync(Guid userId);
        public Task<IEnumerable<UserDto>> GetUsersFromChatAsync(Guid chatId);
        public Task<int> GetUserCountAsync(Guid chatId);
        public Task<int> GetOnlineUserCountAsync(Guid chatId);
        public Task<AppChannelUser> UpdateUnreadMessageCountAsync(Guid userId, Guid chatId, int count);
        public Task<int> GetUnreadMessageCountAsync(Guid userId, Guid chatId);
    }
}
