using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.Request.Authenticate
{
    public class RegisterSchoolDTO
    {
        [Required(ErrorMessage = "Bạn chưa điền tên trường.")]
        public string SchoolName { get; set; }

        [Required(ErrorMessage = "Bạn chưa điền email.")]
        [EmailAddress(ErrorMessage = "Sai format email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bạn chưa điền username.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Bạn chưa điền password.")]
        [RegularExpression("^(?=.*[B!@#$%^&*()_+{}[\\]|:;\"'<>,.?/])(?=.*[a-zA-Z])(?=.*\\d)(?!.*\\s).{8,}$\r\n",
            ErrorMessage = "- Độ dài: Mật khẩu nên có ít nhất 8 ký tự.\r\n" +
            "- Sử dụng ký tự đặc biệt: Có ít nhất một ký tự đặc biệt.\r\n" +
            "- Sử dụng chữ in hoa và chữ thường: Kết hợp giữa các chữ cái in hoa và thường.\r\n" +
            "- Sử dụng số: Thêm ít nhất một hoặc nhiều chữ số.\r\n" +
            "- Không khoảng trắng, có ít nhất 1 ký tự đặc biệt, 1 chữ, 1 số")]
        public string Password { get; set; }
    }
}
