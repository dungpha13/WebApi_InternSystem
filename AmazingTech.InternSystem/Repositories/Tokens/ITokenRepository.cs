using AmazingTech.InternSystem.Data.Entity;
using Microsoft.AspNetCore.Identity;

namespace swp391_be.API.Repositories.Tokens
{
    public interface ITokenRepository
    {
        string CreateJwtToken(User user, List<string> roles);
    }
}
