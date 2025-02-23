using ChatAppBackend.Dtos;
using ChatAppBackend.Models;

namespace ChatAppBackend.Repositories
{
    public interface IChannelRepository
    {
        public Task<AppChannel> GetChannelAsync(Guid id);
        public Task<AppChannel> CreateChannelAsync(AppChannel channel);
        public Task<AppChannel> UpdateChannelAsync(AppChannel channel);
        public Task<AppChannel> DeleteChannelAsync(Guid id);
        public Task<UserChannelDto> AddUserToChannelAsync(UserChannelDto userChannelDto);
        public Task<int> GetUserCountAsync(Guid id);
        public Task<int> GetOnlineUserCountAsync(Guid chatId);
    }
}
