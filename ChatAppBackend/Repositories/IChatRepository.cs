using ChatAppBackend.Models;

namespace ChatAppBackend.Repositories
{
    public interface IChatRepository
    {
        Task<AppChat> GetChatAsync(Guid Id);
        Task<AppChat> CreateChatAsync(AppChat chat);
        Task<AppChat> DeleteChatAsync(Guid Id);
        Task<AppMessage> CreateMessageAsync(AppMessage message);
        Task<IEnumerable<AppMessage>> GetMessagesByChatIdAsync(Guid chatId);
    }
}
