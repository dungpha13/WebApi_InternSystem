using swp391_be.API.Models.DTO;

namespace swp391_be.API.Services.Token
{
    public class CheckToken : ICheckToken
    {
        public bool IsExpried(TokenDecode token)
        {
            return DateTime.Compare(token.Expried, DateTime.Now) < 0;
        }
    }
}
