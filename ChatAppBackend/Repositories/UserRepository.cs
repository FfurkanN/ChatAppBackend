using ChatAppBackend.Data;
using ChatAppBackend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AppUser> AddChatToUserAsync(Guid userId, Guid chatId)
        {
            AppUser user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if(user == null)
            {
                throw new KeyNotFoundException("User not found!");
            }
            user.Chats.Add(chatId);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<AppUser?> ChangeUserStatus(AppUser user, bool isOnline)
        {
            AppUser? userByDb = await _context.Users.FindAsync(user.Id);
            if (userByDb == null)
            {
                throw new KeyNotFoundException("User not found!");
            }
            userByDb.isOnline = isOnline;
            _context.Users.Update(userByDb);
            await _context.SaveChangesAsync();

            return userByDb;
        }

        public async Task<AppUser?> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser?> GetUserByUsernameAsync(string username)
        {
           return await _context.Users.FindAsync(username);
        }

        public async Task<AppUser> RemoveChatFromUserAsync(Guid userId, Guid chatId)
        {
            AppUser user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found!");
            }
            user.Chats.Remove(chatId);
            await _context.SaveChangesAsync();
            return user;
        }

        //public async Task<AppUser> AddUserAsync(AppUser user)
        //{
        //    await _context.Users.AddAsync(user);
        //    await _context.SaveChangesAsync();
        //    return user;
        //}

        //public async Task<AppUser> DeleteUserAsync(string id)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //        throw new KeyNotFoundException("User not found");

        //    _context.Users.Remove(user);
        //    await _context.SaveChangesAsync();
        //    return user;
        //}

        //public async Task<AppUser?> GetUserByIdAsync(string id)
        //{
        //    return await _context.Users.FindAsync(id);
        //}

        //public async Task<IEnumerable<AppUser>> GetUsersAsync()
        //{
        //    return await _context.Users.ToListAsync();
        //}

        //public async Task<AppUser> UpdateUserAsync(AppUser user)
        //{
        //    var existingUser = await _context.FindAsync<AppUser>(user.Id);
        //    if(existingUser == null)
        //        throw new KeyNotFoundException("User not found!");

        //    existingUser.UserName = user.UserName;
        //    existingUser.Email = user.Email;

        //    _context.Users.Update(user);
        //    await _context.SaveChangesAsync();
        //    return existingUser;
        //}
    }

}
