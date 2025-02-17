using ChatAppBackend.Data;
using ChatAppBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public sealed class UserRolesController(ApplicationDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Create(Guid userId, Guid roleId, CancellationToken cancellationToken)
        {
            AppUserRole userRole = new()
            {
                RoleId = roleId,
                UserId = userId,
            };

            await context.UserRoles.AddAsync(userRole);
            await context.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}
