namespace swp391_be.API.Models.Request.User
{
    public class ChangePasswordRequestDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }
}
