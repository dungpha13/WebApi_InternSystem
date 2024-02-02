using AmazingTech.InternSystem.Data.Entity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AmazingTech.InternSystem.Services
{
    public static class JwtGenerator
    {
        public static string GenerateToken(User user, List<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("de455d3d7f83bf393eea5aef43f474f4aac57e3e8d75f9118e60d526453002dc");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("username", user.UserName),
                    new Claim(ClaimTypes.Role, roles[0]),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)

                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static List<InvalidToken> InvalidTokens = new List<InvalidToken>();

        public static void InvalidateToken(string token)
        {
            InvalidTokens.Add(new InvalidToken { Token = token, InvalidatedAt = DateTime.UtcNow });
        }
        public static bool IsTokenValid(string token)
        {
            // Clear expired tokens before checking validity
            ClearExpiredTokens();

            return !InvalidTokens.Any(t => t.Token == token);
        }

        private static void ClearExpiredTokens()
        {
            var expirationThreshold = DateTime.UtcNow.AddHours(-1); // Set your expiration duration

            InvalidTokens.RemoveAll(t => t.InvalidatedAt < expirationThreshold);
        }

        private class InvalidToken
        {
            public string Token { get; set; }
            public DateTime InvalidatedAt { get; set; }
        }

        public static string ExtractTokenFromHeader(string authHeader)
        {
            return authHeader.Substring("Bearer ".Length).Trim();
        }

        public static string ExtractUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            // Extract the user ID claim
            var uid = jsonToken?.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            if (uid == null)
            {
                uid = jsonToken?.Claims.FirstOrDefault(c => c.Type.Equals("nameid"))?.Value;
            }
            return uid;
        }

        public static string ExtractUserRoleFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            // Extract the user ID claim
            var role = jsonToken?.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Role))?.Value;
            if (role == null)
            {
                role = jsonToken?.Claims.FirstOrDefault(c => c.Type.Equals("role"))?.Value;
            }

            return role;
        }
    }
}
