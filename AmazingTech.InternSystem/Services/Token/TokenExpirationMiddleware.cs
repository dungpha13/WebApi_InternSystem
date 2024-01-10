using Microsoft.AspNetCore.Authentication;

namespace swp391_be.API.Services.Token
{
    public class TokenExpirationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenExpirationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authResult = await context.AuthenticateAsync();

            if (authResult.Succeeded && authResult.Properties.ExpiresUtc.HasValue &&
                authResult.Properties.ExpiresUtc.Value < DateTime.UtcNow)
            {
                context.Response.StatusCode = 401; // Unauthorized
                return;
            }

            await _next(context);
        }
    }
}
