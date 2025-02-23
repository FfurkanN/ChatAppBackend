using ChatAppBackend.Data;
using ChatAppBackend.Dtos;
using ChatAppBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackend.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context;

        public ChatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AppChat> CreateChatAsync(AppChat chat, Guid channelId)
        {
            await _context.Chats.AddAsync(chat);

            await _context.ChannelChat.AddAsync(new AppChannelChat { ChannelId = channelId, ChatId = chat.Id });
            
            await _context.SaveChangesAsync();
            return chat;
        }

        public async Task<AppMessage> CreateMessageAsync(AppMessage message)
        {
            await _context.Messages.AddAsync(message);

            await _context.ChatMessages.AddAsync(new AppChatMessage { ChatId = message.Chat_Id, MessageId = message.Id });

            await _context.SaveChangesAsync();
            
            return message;
        }

        public async Task<AppChat> DeleteChatAsync(Guid Id)
        {
            var chat = await _context.Chats.FindAsync(Id);
            if (chat == null)
                throw new KeyNotFoundException("Chat not found");

            _context.Chats.Remove(chat);
            _context.ChannelChat.Where(c => c.ChatId == Id).ExecuteDelete();
            _context.ChatMessages.Where(m => m.ChatId == Id).ExecuteDelete();
            _context.Messages.Where(m => m.Chat_Id == Id).ExecuteDelete();

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


            List<AppChatMessage> chatMessages = await _context.ChatMessages.Where(m=> m.ChatId == chatId).ToListAsync();

            List<AppMessage> messages = new List<AppMessage>();

            foreach (var chatMessage in chatMessages)
            {
                AppMessage? message = await _context.Messages.FindAsync(chatMessage.MessageId);
                if (message == null)
                    throw new KeyNotFoundException("Message not found");
                messages.Add(message);
            }

            return messages.OrderBy(m=> m.Send_Date);
        }

        public async Task<AppChat> UpdateChat(AppChat chat)
        {
            _context.Chats.Update(chat);
            await _context.SaveChangesAsync();

            return chat;
        }
    }
}
