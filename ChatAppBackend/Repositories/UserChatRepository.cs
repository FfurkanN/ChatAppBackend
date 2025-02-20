using ChatAppBackend.Data;
using ChatAppBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackend.Repositories
{
    public class UserChatRepository : IUserChatRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;

        public UserChatRepository(ApplicationDbContext context, IChatRepository chatRepository, IUserRepository userRepository)
        {
            _context = context;
            _chatRepository = chatRepository;
            _userRepository = userRepository;
        }
        public async Task<AppUserChat> AddUserChat(Guid userId, Guid chatId)
        {
            AppUserChat appUserChat = new AppUserChat
            {
                UserId = userId,
                ChatId = chatId
            };
            await _context.UserChat.AddAsync(appUserChat);
            await _context.SaveChangesAsync();

            return appUserChat;
        }

        public async Task<IEnumerable<AppChat>> GetUserChatsAsync(Guid userId)
        {
            List<AppUserChat> appUserChats = await _context.UserChat.Where(p => p.UserId == userId).ToListAsync();

            List<AppChat> userChats = new List<AppChat>();
            foreach (var userChat in appUserChats)
            {
                AppChat appChat = await _chatRepository.GetChatAsync(userChat.ChatId);
                userChats.Add(appChat);
            }
            return userChats;
        }

        public async Task<IEnumerable<AppUser>> GetUsersFromChatAsync(Guid chatId)
        {
            List<AppUserChat> appchatUsers = await _context.UserChat.Where(p => p.ChatId == chatId).ToListAsync();

            List<AppUser> chatUsers = new List<AppUser>();

            foreach (var chatUser in appchatUsers)
            {
                AppUser? user = await _userRepository.GetUserByIdAsync(chatUser.UserId);
                if(user != null)
                {
                    chatUsers.Add(user);
                }
            }
            return chatUsers;
        }

        public async Task<AppUserChat> RemoveUserChat(Guid userId, Guid chatId)
        {
            AppUserChat? appUserChat = await _context.UserChat.FirstOrDefaultAsync(p => p.UserId == userId && p.ChatId == chatId);

            if (appUserChat == null)
            {
                throw new KeyNotFoundException("UserChat not found!");
            }

            _context.UserChat.Remove(appUserChat);
            await _context.SaveChangesAsync();

            return appUserChat;
        }

        public async Task<AppUserChat> UpdateUnreadMessageCountAsync(Guid userId, Guid chatId, int count)
        {
            Console.WriteLine("USERID" + userId + " CHATID" + chatId+" COUNT"+count);
            AppUserChat? appUserChat = await _context.UserChat.FirstOrDefaultAsync(p=> p.UserId == userId && p.ChatId == chatId);
            if (appUserChat == null)
            {
                throw new KeyNotFoundException("UserChat was not found");
            }
            _context.UserChat.Update(appUserChat);
            appUserChat.UnreadMessageCount = count;
            await _context.SaveChangesAsync();

            return appUserChat;
        }

        public async Task<int> GetUnreadMessageCountAsync(Guid userId, Guid chatId)
        {
            AppUserChat? appUserChat = await _context.UserChat.FirstOrDefaultAsync(p => p.UserId == userId && p.ChatId == chatId);
            if (appUserChat == null)
            {
                throw new KeyNotFoundException("UserChat was not found");
            }
            return appUserChat.UnreadMessageCount;
        }
    }
}
