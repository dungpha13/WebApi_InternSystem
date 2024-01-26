using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.Request.Authenticate
{
    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "Old password cannot be empty")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "New password cannot be empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
