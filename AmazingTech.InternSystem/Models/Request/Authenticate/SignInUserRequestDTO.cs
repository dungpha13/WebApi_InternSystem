using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.Request.Authenticate
{
    public class SignInUserRequestDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
