using ChatAppBackend.Dtos;
using ChatAppBackend.Models;
using ChatAppBackend.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserChatRepository _userChatRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IChannelRepository _channelRepository;

        public UserController(IUserRepository userRepository, IUserChatRepository userChatRepository, UserManager<AppUser> userManager, IChannelRepository channelRepository)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _userChatRepository = userChatRepository;
            _channelRepository = channelRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserByUsername(string username, CancellationToken cancellationToken)
        {
            AppUser? user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NoContent();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> GetUserByIdAsync(string[] usersId, CancellationToken cancellationToken)
        {
            List<AppUser> users = new List<AppUser>();
            foreach(var userId in usersId)
            {
                AppUser? user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NoContent();
                }
                users.Add(user);
            }

            return Ok(users);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetOwnedChannelsAsync(CancellationToken cancellationToken)
        {
            var userId = new Guid(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            List<ChannelDto> channels = await _userRepository.GetOwnedChannelsAsync(userId);

            return Ok(channels);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserChannelsAsync(CancellationToken cancellationToken)
        {
            var userId = new Guid(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            IEnumerable<ChannelDto> channels = await _userRepository.GetChannelsAsync(userId);

            return Ok(channels);
        }

        //[HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<IActionResult> GetUserCountByChatId(Guid chatId, CancellationToken cancellationToken)
        //{
        //    var userCount = await _userChatRepository.GetUserCountAsync(chatId);

        //    return Ok(userCount);
        //}

        //[HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<IActionResult> GetOnlineUserCountByChatId(Guid chatId, CancellationToken cancellationToken)
        //{
        //    var userCount = await _userChatRepository.GetOnlineUserCountAsync(chatId);

        //    return Ok(userCount);
        //}


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UploadProfileImage([FromForm] IFormFile file, CancellationToken cancellationToken)
        {
            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/profile_images");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest("File upload failed!");
            }
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var fileUrl = $"profile_images/{fileName}";

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            AppUser? user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return NoContent();
            }

            if (!string.IsNullOrEmpty(user.ProfileImageUrl))
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.ProfileImageUrl);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            user.ProfileImageUrl = fileUrl;
            await _userManager.UpdateAsync(user);

            return Ok(user);
        }
    }
}

