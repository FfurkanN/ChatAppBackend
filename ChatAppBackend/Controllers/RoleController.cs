using ChatAppBackend.Dtos;
using ChatAppBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public sealed class RoleController(RoleManager<AppRole> roleManager) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(NameDto nameDto, CancellationToken cancellationToken)
        {
            AppRole appRole = new()
            {
                Name = nameDto.name,
            };

            IdentityResult result = await roleManager.CreateAsync(appRole);
            appRole = await roleManager.FindByNameAsync(appRole.Name);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return CreatedAtAction(nameof(Create),new {id = appRole.Id},appRole);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var roles = await roleManager.Roles.ToListAsync(cancellationToken);

            return Ok(roles);
        }
    }
}
