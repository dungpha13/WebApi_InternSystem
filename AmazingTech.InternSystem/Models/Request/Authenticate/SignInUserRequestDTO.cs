using System.ComponentModel.DataAnnotations;

namespace swp391_be.API.Models.Request.Authenticate
{
    public class SignInUserRequestDTO
    {
       
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
