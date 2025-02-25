using ChatAppBackend.Dtos;
using ChatAppBackend.Models;

namespace ChatAppBackend.Repositories
{
    public interface IChatRepository
    {
        Task<AppChat> GetChatAsync(Guid Id);
        Task<List<ChatDto>> GetChatsAsync(Guid channelId);
        Task<AppChat> CreateChatAsync(AppChat chat, Guid channelId);
        Task<AppChat> DeleteChatAsync(Guid Id);
        Task<AppChat> UpdateChat(AppChat chat);
        Task<AppMessage> CreateMessageAsync(AppMessage message);
        Task<IEnumerable<AppMessage>> GetMessagesByChatIdAsync(Guid chatId);
    }
}
