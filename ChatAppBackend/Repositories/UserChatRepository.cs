using ChatAppBackend.Data;
using ChatAppBackend.Dtos;
using ChatAppBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackend.Repositories
{
    public class UserChatRepository : IUserChatRepository
    {
        private readonly ApplicationDbContext _context;

        public UserChatRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        //public async Task<IEnumerable<AppUserChannel>> AddUserChat(Guid[] userIds, Guid chatId)
        //{
        //   List<AppUserChannel> usersChats = new List<AppUserChannel>();
        //   foreach(var userId in userIds)
        //    {
        //        AppUserChannel appUserChat = new AppUserChannel
        //        {
        //            UserId = userId,
        //            ChatId = chatId
        //        };
        //        await _context.UserChat.AddAsync(appUserChat);
        //        usersChats.Add(appUserChat);
        //    }
        //    await _context.SaveChangesAsync();

        //    return usersChats;
        //}

        //public async Task<IEnumerable<ChatDto>> GetUserChatsAsync(Guid userId)
        //{

        //    var chats = await _context.UserChat.Where(uc => uc.UserId == userId)
        //        .Select(uc => new ChatDto
        //        (
        //            uc.Chat.Id,
        //            uc.Chat.Name,
        //            uc.Chat.Creator_Id,
        //            uc.Chat.isPublic,
        //            uc.Chat.Create_Date,
        //            _context.UserChat.Count(c=> c.ChatId == uc.Chat.Id),
        //            _context.UserChat.Count(c=> c.ChatId == uc.Chat.Id && c.User.isOnline)
        //        )).ToListAsync();

        //    return chats;
        //}

        //public async Task<IEnumerable<ChatDto>> GetOwnedChatsAsync(Guid userId)
        //{

        //    var chats = await _context.UserChat.Where(uc => uc.UserId == userId && uc.Chat.Creator_Id == userId)
        //        .Select(uc => new ChatDto
        //        (
        //            uc.Chat.Id,
        //            uc.Chat.Name,
        //            uc.Chat.Creator_Id,
        //            uc.Chat.isPublic,
        //            uc.Chat.Create_Date,
        //            _context.UserChat.Count(c => c.ChatId == uc.Chat.Id),
        //            _context.UserChat.Count(c => c.ChatId == uc.Chat.Id && c.User.isOnline)
        //        )).ToListAsync();

        //    return chats;
        //}

        //public async Task<IEnumerable<UserDto>> GetUsersFromChatAsync(Guid chatId)
        //{
        //    var users = await _context.UserChat.Where(uc => uc.ChatId == chatId)
        //        .Select(uc => new UserDto
        //        (
        //            uc.User.Id,
        //            uc.User.UserName,
        //            uc.User.isOnline,
        //            uc.User.ProfileImageUrl
        //        )).ToListAsync();

        //    return users;
        //}

        //public async Task<AppUserChannel> RemoveUserChat(Guid userId, Guid chatId)
        //{
        //    AppUserChannel? appUserChat = await _context.UserChat.FirstOrDefaultAsync(p => p.UserId == userId && p.ChatId == chatId);

        //    if (appUserChat == null)
        //    {
        //        throw new KeyNotFoundException("UserChat not found!");
        //    }

        //    _context.UserChat.Remove(appUserChat);
        //    await _context.SaveChangesAsync();

        //    return appUserChat;
        //}

        //public async Task<AppChannelUser> UpdateUnreadMessageCountAsync(Guid userId, Guid chatId, int count)
        //{
        //    AppChannelUser? appUserChat = await _context.UserChat.FirstOrDefaultAsync(p=> p.UserId == userId && p.ChatId == chatId);
        //    if (appUserChat == null)
        //    {
        //        throw new KeyNotFoundException("UserChat was not found");
        //    }
        //    appUserChat.UnreadMessageCount = count;
        //    _context.UserChat.Update(appUserChat);
        //    await _context.SaveChangesAsync();

        //    return appUserChat;
        //}

        //public async Task<int> GetUnreadMessageCountAsync(Guid userId, Guid chatId)
        //{
        //    AppUserChannel? appUserChat = await _context.UserChat.FirstOrDefaultAsync(p => p.UserId == userId && p.ChatId == chatId);
        //    if (appUserChat == null)
        //    {
        //        throw new KeyNotFoundException("UserChat was not found");
        //    }
        //    return appUserChat.UnreadMessageCount;
        //}

        public Task<AppChannelUser> RemoveUserChat(Guid userId, Guid chatId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AppChannelUser>> AddUserChat(Guid[] userId, Guid chatId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ChatDto>> GetUserChatsAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ChatDto>> GetOwnedChatsAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserDto>> GetUsersFromChatAsync(Guid chatId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetUserCountAsync(Guid chatId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetOnlineUserCountAsync(Guid chatId)
        {
            throw new NotImplementedException();
        }

        public Task<AppChannelUser> UpdateUnreadMessageCountAsync(Guid userId, Guid chatId, int count)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetUnreadMessageCountAsync(Guid userId, Guid chatId)
        {
            throw new NotImplementedException();
        }

        //public async Task<int> GetUserCountAsync(Guid chatId)
        //{
        //    return await _context.UserChat.Where(c => c.ChatId == chatId).CountAsync();
        //}

        //public async Task<int> GetOnlineUserCountAsync(Guid chatId)
        //{
        //    return await _context.UserChat.Where(uc => uc.ChatId == chatId && uc.User.isOnline).CountAsync();
        //}
    }
}
