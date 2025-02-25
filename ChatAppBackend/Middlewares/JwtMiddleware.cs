using ChatAppBackend.Services;
using System.Security.Claims;

namespace ChatAppBackend.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next) { _next = next; }

        public async Task InvokeAsync(HttpContext context, JwtService jwtService)
        {
            var token = context.Request.Cookies["access_token"];

            if(!string.IsNullOrEmpty(token))
            {
                context.Request.Headers.Append("Authorization", $"Bearer {token}");
                var claims = jwtService.ValidateJwtToken(token);
                if(claims != null)
                {
                    var userId = new Guid(claims.FindFirst(ClaimTypes.NameIdentifier).Value);
                    context.Items["user"] = userId;
                }
            }
            await _next(context);
        }
    }
}
