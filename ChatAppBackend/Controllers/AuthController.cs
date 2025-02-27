using ChatAppBackend.Services;
using ChatAppBackend.Dtos;
using ChatAppBackend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace ChatAppBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public sealed class AuthController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, IConfiguration configuration) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto request, CancellationToken cancellationToken)
        {
            AppUser appUser = new()
            {
                Email = request.Email,
                UserName = request.Username,
                Firstname = request.FirstName,
                Lastname = request.LastName,
            };

            IdentityResult result = await userManager.CreateAsync(appUser, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var jwtService = new JwtService(configuration);
            string accessToken = jwtService.GenerateAccessToken(appUser);
            string refreshToken = jwtService.GenerateRefreshToken();

            appUser.RefreshToken = refreshToken;
            await userManager.UpdateAsync(appUser);

            //Response.Cookies.Append("access_token", accessToken, new CookieOptions
            //{
            //    HttpOnly = true,
            //    Secure = true,
            //    SameSite = SameSiteMode.None,
            //    Expires = DateTime.Now.AddHours(1),
            //    Domain = "localhost",
            //    Path = "/"
            //});
            //Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
            //{
            //    HttpOnly = true,
            //    Secure = true,
            //    SameSite = SameSiteMode.None,
            //    Expires = DateTime.Now.AddDays(1),
            //    Domain = "localhost",
            //    Path = "/"
            //});

            return Ok(new
            {
                TokenType = "Bearer",
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            });

        }
        

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto request, CancellationToken cancellationToken)
        {
            AppUser? appUser = await userManager.Users.FirstOrDefaultAsync(p => p.Email == request.UserNameOrEmail || p.UserName == request.UserNameOrEmail, cancellationToken);

            if (appUser is null)
            {
                return BadRequest();
            }

            SignInResult result = await signInManager.CheckPasswordSignInAsync(appUser, request.Password, false);

            if (result.IsLockedOut)
            {
                TimeSpan? timeSpan = appUser.LockoutEnd - DateTime.Now;
                return StatusCode(500, $"Your user has been locked for {(timeSpan?.TotalSeconds ?? 30)} seconds because you entered your password incorrectly 3 times.");
            }
            bool isPasswordCorrect = await userManager.CheckPasswordAsync(appUser, request.Password);

            //if (!result.Succeeded)
            //{
            //    return StatusCode(500, result);
            //}

            if (!isPasswordCorrect)
            {
                return BadRequest();
            }

            //if (result.IsNotAllowed)
            //{
            //    return StatusCode(500, "Your mail is not confirmed!");
            //}

            var jwtService = new JwtService(configuration);
            string accessToken = jwtService.GenerateAccessToken(appUser);
            string refreshToken = jwtService.GenerateRefreshToken();

            appUser.RefreshToken = refreshToken;
            await userManager.UpdateAsync(appUser);


            //Response.Cookies.Append("access_token", accessToken, new CookieOptions
            //{
            //    HttpOnly = true,
            //    Secure = true,
            //    SameSite = SameSiteMode.None,
            //    Expires = DateTime.Now.AddHours(1),
            //    Domain = "localhost",
            //    Path = "/"
            //});
            //Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
            //{
            //    HttpOnly=true,
            //    Secure = true,
            //    SameSite=SameSiteMode.None,
            //    Expires=DateTime.Now.AddDays(1),
            //    Domain="localhost",
            //    Path="/"
            //});

            return Ok(
            new
            {
                TokenType = "Bearer",
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }


        [HttpGet]
        public async Task<IActionResult> Admin()
        {
            var user = await userManager.FindByNameAsync("furkan");

            var adminRole = new AppRole
            {
                Name = "Admin"
            };
            await roleManager.CreateAsync(adminRole);
            await roleManager.AddClaimAsync(adminRole, new Claim("Permission", "chats.view"));
            await roleManager.AddClaimAsync(adminRole, new Claim("Permission", "chats.create"));
            await roleManager.AddClaimAsync(adminRole, new Claim("Permission", "chats.update"));
            await roleManager.AddClaimAsync(adminRole, new Claim("Permission", "chats.delete"));

            await userManager.AddToRoleAsync(user, adminRole.Name);

            return Ok();

        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto request, CancellationToken cancellationToken)
        {
            AppUser? appUser = await userManager.FindByIdAsync(request.id.ToString());

            if(appUser is null)
            {
                return BadRequest(new { Message = "User not found!" });
            }

            IdentityResult result = await userManager.ChangePasswordAsync(appUser, request.CurrentPassword, request.NewPassword);
            if(!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(s => s.Description));
            }

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> ForgotPassword(string email, CancellationToken cancellationToken)
        {
            AppUser? appUser = await userManager.FindByEmailAsync(email);

            if (appUser is null)
            {
                return BadRequest(new { Message = "User not found!" });
            }

            string token = await userManager.GeneratePasswordResetTokenAsync(appUser);

            return Ok(new {Token = token});
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordUsingToken(ChangePasswordUsingTokenDto request, CancellationToken cancellationToken)
        {
            AppUser? appUser = await userManager.FindByEmailAsync(request.Email);

            if (appUser is null)
            {
                return BadRequest(new { Message = "User not found!" });
            }

            IdentityResult result = await userManager.ResetPasswordAsync(appUser, request.Token, request.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(s => s.Description));
            }

            return NoContent();
        }

 
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserByTokenAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var user = await userManager.FindByIdAsync(userId);
            return Ok(user);
        }
        [HttpPost]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto refreshTokenDto, CancellationToken cancellationToken)
        {
            //string refreshToken = data.refreshToken;
            AppUser ? user = await userManager.Users.FirstOrDefaultAsync(p => p.RefreshToken == refreshTokenDto.refreshToken, cancellationToken);
            if (user is null)
            {
                return BadRequest(new { Message = "User not found!" });
            }

            var jwtService = new JwtService(configuration);
            string accessToken = jwtService.GenerateAccessToken(user);
            string newRefreshToken = jwtService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await userManager.UpdateAsync(user);

            return Ok(new
            {
                TokenType = "Bearer",
                AccessToken = accessToken,
                RefreshToken = newRefreshToken,
                expiresIn = 60*24
            });
        }
            
    }
}
