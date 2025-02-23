using ChatAppBackend.Data;
using ChatAppBackend.Dtos;
using ChatAppBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackend.Repositories
{
    public class ChannelRepository : IChannelRepository
    {
        private readonly ApplicationDbContext _context;

        public ChannelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserChannelDto> AddUserToChannelAsync(UserChannelDto userChannelDto)
        {
            AppChannel channel = await _context.Channels.FindAsync(userChannelDto.ChannelId);
            if(channel == null)
            {
                return null;
            }
            AppChannelUser isUserInChannel = await _context.ChannelUser.FirstOrDefaultAsync(c=>c.UserId == userChannelDto.UserId && c.ChannelId == userChannelDto.ChannelId);
            
            if (isUserInChannel != null)
            {
                return null;
            }

            var channeluser = new AppChannelUser { ChannelId = userChannelDto.ChannelId, UserId = userChannelDto.UserId };

            await _context.ChannelUser.AddAsync(channeluser);
            _context.SaveChanges();
            return userChannelDto;
        }

        public async Task<AppChannel> CreateChannelAsync(AppChannel channel)
        {
            await _context.Channels.AddAsync(channel);
            await _context.SaveChangesAsync();

            return channel;
        }

        public async Task<AppChannel> DeleteChannelAsync(Guid id)
        {
            var channel = await _context.Channels.FindAsync(id);
            if(channel == null)
            {
                return null;
            }
            _context.Channels.Remove(channel);
            await _context.SaveChangesAsync();

            return channel;
        }

        public async Task<AppChannel> GetChannelAsync(Guid id)
        {
            var channel = await _context.Channels.FindAsync(id);
            if (channel == null)
            {
                return null;
            }
            return channel;
        }

        public async Task<int> GetOnlineUserCountAsync(Guid id)
        {
            return await _context.ChannelUser.Where(cu => cu.ChannelId == id && cu.User.isOnline).CountAsync();
        }

        public async Task<int> GetUserCountAsync(Guid id)
        {
            return await _context.ChannelChat.Where(c => c.ChannelId == id).CountAsync();
        }

        public async Task<AppChannel> UpdateChannelAsync(AppChannel channel)
        {
           _context.Channels.Update(channel);
           _context.SaveChangesAsync();

            return channel;
        }
    }
}
