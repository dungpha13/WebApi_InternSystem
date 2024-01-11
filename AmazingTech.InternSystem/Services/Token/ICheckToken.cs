using swp391_be.API.Models.DTO;

namespace swp391_be.API.Services.Token
{
    public interface ICheckToken
    {
        public bool IsExpried(TokenDecode token);
    }
}
