﻿using ChatAppBackend.Data;
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

        public Task<List<ChatDto>> GetChatsAsync(Guid channelId)
        {
            var chats = _context.ChannelChat.Where(cc => cc.ChannelId == channelId)
                .Select(cc => new ChatDto(
                    cc.Chat.Id,
                    cc.Chat.Name,
                    cc.Chat.Creator_Id,
                    cc.Chat.Create_Date)).ToListAsync();
            return chats;
        }

        public async Task<IEnumerable<AppMessage>> GetMessagesByChatIdAsync(Guid chatId, DateTime? lastMessageDate)
        {
            AppChat? chat = await _context.Chats.FindAsync(chatId);

            if (chat == null)
                throw new KeyNotFoundException("Chat not found");

            List<AppMessage> messages = new List<AppMessage>();
            if (lastMessageDate.HasValue)
            {
               messages = await _context.ChatMessages.Where(m => m.ChatId == chatId && m.Message.Send_Date < lastMessageDate.Value)
                    .OrderByDescending(m => m.Message.Send_Date)
                    .Take(50)
                    .Select(m => m.Message)
                    .ToListAsync();
            }
            else
            {
               messages = await _context.ChatMessages.Where(m => m.ChatId == chatId)
                   .OrderByDescending(m => m.Message.Send_Date)
                   .Take(50)
                   .Select(m => m.Message)
                   .ToListAsync();
            }

            return messages;
        }

        public async Task<AppChat> UpdateChat(AppChat chat)
        {
            _context.Chats.Update(chat);
            await _context.SaveChangesAsync();

            return chat;
        }
    }
}
