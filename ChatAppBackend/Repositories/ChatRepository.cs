using ChatAppBackend.Data;
using ChatAppBackend.Dtos;
using ChatAppBackend.Models;

namespace ChatAppBackend.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context;

        public ChatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AppChat> CreateChatAsync(AppChat chat)
        {
            await _context.Chats.AddAsync(chat);
            await _context.SaveChangesAsync();
            return chat;
        }

        public async Task<AppMessage> CreateMessageAsync(AppMessage message)
        {
            await _context.Messages.AddAsync(message);
            AppChat? chat = await _context.Chats.FindAsync(message.Chat_Id);
            chat.Messages.Add(message.Id);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<AppChat> DeleteChatAsync(Guid Id)
        {
            var chat = await _context.Chats.FindAsync(Id);
            if (chat == null)
                throw new KeyNotFoundException("Chat not found");

            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();
            return chat;
        }

        public async Task<AppChat> GetChatAsync(Guid Id)
        {
            return await _context.Chats.FindAsync(Id);
        }

        public async Task<IEnumerable<AppMessage>> GetMessagesByChatIdAsync(Guid chatId)
        {
            AppChat? chat = await _context.Chats.FindAsync(chatId);

            if (chat == null)
                throw new KeyNotFoundException("Chat not found");

            List<AppMessage> messages = new List<AppMessage>();

            foreach (var messageId in chat.Messages)
            {
                AppMessage? message = await _context.Messages.FindAsync(messageId);
                if (message == null)
                    throw new KeyNotFoundException("Message not found");
                messages.Add(message);
            }

            return messages;
        }
    }
}
