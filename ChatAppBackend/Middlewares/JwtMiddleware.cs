using Azure;
using Azure.Core;
using ChatAppBackend.Models;
using ChatAppBackend.Repositories;
using ChatAppBackend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace ChatAppBackend.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next) { _next = next; }

        public async Task InvokeAsync(HttpContext context, JwtService jwtService, UserManager<AppUser> userManager, IUserRepository userRepository)
        {
            var token = context.Request.Headers["Authorization"];

            if(!string.IsNullOrEmpty(token))
            {
                //context.Request.Headers.Append("Authorization", $"Bearer {token}");
                //var claims = jwtService.ValidateJwtToken(token);
                //if(claims != null)
                //{
                //    var userId = new Guid(claims.FindFirst(ClaimTypes.NameIdentifier).Value);
                //    context.Items["user"] = userId;
                //}
            }
            else
            {
                var refreshToken = context.Request.Cookies["refresh_token"];
                if(!string.IsNullOrEmpty(refreshToken))
                {
                    var userByRefreshToken = await userRepository.GetUserByRefreshToken(refreshToken);
                    if(userByRefreshToken == null)
                    {
                        return;
                    }
                    string newAccessToken = jwtService.GenerateAccessToken(userByRefreshToken);
                    string newRefreshToken = jwtService.GenerateRefreshToken();

                    userByRefreshToken.RefreshToken = newRefreshToken;
                    await userManager.UpdateAsync(userByRefreshToken);

                    //context.Request.Headers.Append("Authorization", $"Bearer {newAccessToken}");
                }
            }
            await _next(context);
        }
    }
}
