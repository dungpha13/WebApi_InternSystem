using System.ComponentModel.DataAnnotations;

namespace swp391_be.API.Models.Request.User
{
    public class CreateUserRequestDTO
    {
        [Required]
        public string HoVaTen { get; set; }
        
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string Roles { get; set; }
    }
}
