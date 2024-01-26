using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.Request.Authenticate
{
    public class ResetPasswordDTO
    {
        [Required(ErrorMessage = "Token is required.")]
        public string ResetToken { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
