using ChatAppBackend.Data;
using ChatAppBackend.Dtos;
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

        public async Task<List<ChannelDto>> GetChannelsAsync(Guid id)
        {
            var channels = await _context.ChannelUser.Where(c => c.UserId == id)
                .Select(c => new ChannelDto
                (
                c.Channel.Id,
                c.Channel.Name,
                c.Channel.ChannelImageUrl,
                c.Channel.Description,
                c.Channel.Creator_Id,
                c.Channel.Create_Date,
                c.Channel.isPublic
                )).ToListAsync();

            return channels;
        }

        public async Task<List<ChannelDto>> GetOwnedChannelsAsync(Guid id)
        {
            var channels = await _context.Channels.Where(c=>c.Creator_Id == id)
               .Select(c => new ChannelDto
               (
               c.Id,
               c.Name,
               c.ChannelImageUrl,
               c.Description,
               c.Creator_Id,
               c.Create_Date,
               c.isPublic
               )).ToListAsync();

            return channels;
        }

        public async Task<IEnumerable<AppChat>> GetOwnedChats(Guid Id)
        {
            return await _context.Chats.Where(c=> c.Creator_Id == Id).OrderBy(c=>c.Create_Date).ToListAsync();
        }

        public async Task<AppUser?> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser?> GetUserByUsernameAsync(string username)
        {
           return await _context.Users.FindAsync(username);
        }

        public async Task<AppUser?> GetUserByRefreshToken(string refreshToken)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
        }



    }

}
