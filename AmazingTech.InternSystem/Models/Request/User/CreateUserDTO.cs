using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.Request.User
{
    public class CreateUserDTO
    {
        [Required(ErrorMessage = "Bạn chưa điền họ tên hoặc tên trường.")]
        public string FullNameOrSchoolName { get; set; }

        [Required(ErrorMessage = "Bạn chưa điền username.")]
        [RegularExpression("^[a-zA-Z0-9-._@+]{3,20}$", ErrorMessage = "Username cần có 3-20 kí tự.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Bạn chưa điền password.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$",
            ErrorMessage = "- Độ dài: Mật khẩu nên có ít nhất 8 ký tự.\\n" +
            "- Sử dụng ký tự đặc biệt: Có ít nhất một ký tự đặc biệt.\\n" +
            "- Sử dụng chữ in hoa và chữ thường: Kết hợp giữa các chữ cái in hoa và thường.\\n" +
            "- Sử dụng số: Thêm ít nhất một hoặc nhiều chữ số.\\n" +
            "- Không khoảng trắng, có ít nhất 1 ký tự đặc biệt, 1 chữ, 1 số")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Bạn chưa điền email.")]
        [EmailAddress(ErrorMessage = "Sai format email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bạn chưa điền số điện thoại.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Vui lòng nhập số điện thoại hợp lệ!")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Bạn chưa điền role.")]
        [RegularExpression(@"\b(Intern|HR|Admin|School|Mentor)\b",
            ErrorMessage = "Hãy điền đúng tên role.")]
        public string Role { get; set; }
    }
}
