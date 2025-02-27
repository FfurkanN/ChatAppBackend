using ChatAppBackend.Dtos;
using ChatAppBackend.Models;
using ChatAppBackend.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;

namespace ChatAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]

    public class ChannelController(IChannelRepository channelRepository) : ControllerBase
    {

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateChannelAsync(ChannelCreateDto channelCreateDto, CancellationToken cancellationToken)
        {
            AppChannel appChannel = new AppChannel()
            {
                Name = channelCreateDto.ChannelName,
                ChannelImageUrl = channelCreateDto.ChannelImageUrl,
                Description = channelCreateDto?.Description,
                Creator_Id = new Guid(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value),
                Create_Date = DateTime.Now,
                isPublic = channelCreateDto.isPublic
            };
            var createdChannel = await channelRepository.CreateChannelAsync(appChannel);

            return Ok(createdChannel);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetChannelByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            AppChannel appChannel = await channelRepository.GetChannelAsync(id);

            if(appChannel == null)
            {
                return NoContent();
            }

            return Ok(appChannel);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddUserToChannelAsync(UserChannelDto userChannelDto, CancellationToken cancellationToken)
        {
            Console.WriteLine("USERID" + userChannelDto.UserId);
            var userChannel = await channelRepository.AddUserToChannelAsync(userChannelDto);
            if(userChannel == null)
            {
                return BadRequest();
            }
            return Ok(userChannel);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUsersByChannelIdAsync(Guid channelId, CancellationToken cancellationToken)
        {
            var users = await channelRepository.GetUsersByChannelIdAsync(channelId);
            if(users == null)
            {
                return BadRequest();
            }
            return Ok(users);
        }

    }
}
