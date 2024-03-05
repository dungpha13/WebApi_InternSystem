using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.Request.Authenticate
{
    public class RegisterRoleDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Bạn chưa điền password.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$",
          ErrorMessage = "- Độ dài: Mật khẩu nên có ít nhất 8 ký tự.\\n" +
          "- Sử dụng ký tự đặc biệt: Có ít nhất một ký tự đặc biệt.\\n" +
          "- Sử dụng chữ in hoa và chữ thường: Kết hợp giữa các chữ cái in hoa và thường.\\n" +
          "- Sử dụng số: Thêm ít nhất một hoặc nhiều chữ số.\\n" +
          "- Không khoảng trắng, có ít nhất 1 ký tự đặc biệt, 1 chữ, 1 số")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please retype the password.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string RetypePassword { get; set; }

    }
}
