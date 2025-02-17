using ChatAppBackend.Dtos;
using ChatAppBackend.Models;
using ChatAppBackend.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;

        public UserController(IUserRepository userRepository, UserManager<AppUser> userManager)
        //public UserController(IUserRepository userRepository, UserManager<AppUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
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
        [HttpPost]
        public async Task<IActionResult> UploadProfileImage([FromForm]IFormFile file,Guid userId, CancellationToken cancellationToken)
        {
            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/profile_images");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            if(file == null || file.Length == 0)
            {
                return BadRequest("File upload failed!");
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadPath, fileName);

            using(var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var fileUrl = $"profile_images/{fileName}";

            AppUser? user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return NoContent();
            }

            user.ProfileImageUrl = fileUrl;
            await _userManager.UpdateAsync(user);

            return Ok(user);

        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<IdentityUser>>> GetUsersAsync()
        //{
        //    var users = await _userRepository.GetUsersAsync();
        //    return Ok(users);
        //}

        //[HttpGet]
        //public async Task<AppUser> GetUserByIdAsync(Guid id)
        //{
        //    var user = await _userManager.FindByIdAsync(id.ToString());
        //    if (user == null)
        //    {
        //        return null;
        //    }
        //    return user;
        //}

        //[HttpPost]
        //public async Task<ActionResult<IdentityUser>> AddUserAsync(AppUser user)
        //{
        //    if (ModelState.IsValid == false)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    await _userRepository.AddUserAsync(user);
        //    return CreatedAtAction(nameof(GetUserByIdAsync), new { id = user.Id }, user);
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult<IdentityUser>> DeleteUserAsync(string id)
        //{
        //    try
        //    {
        //        var user = await _userRepository.DeleteUserAsync(id);
        //        return Ok(user);
        //    }
        //    catch (KeyNotFoundException e)
        //    {
        //        return NotFound(e.Message);
        //    }
        //}
        //[HttpPut("{id}")]
        //public async Task<ActionResult<AppUser>> UpdateUserAsync(string id, AppUser user)
        //{
        //    //if (id != user.Id)
        //    //{
        //    //    return BadRequest("Id in the URL does not match the Id in the body");
        //    //}
        //    if (ModelState.IsValid == false)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    try
        //    {
        //        var updatedUser = await _userRepository.UpdateUserAsync(user);
        //        return CreatedAtAction(nameof(GetUserByIdAsync), new { id = user.Id }, user);
        //    }
        //    catch (KeyNotFoundException e)
        //    {
        //        return NotFound(e.Message);
        //    }

    }
}

