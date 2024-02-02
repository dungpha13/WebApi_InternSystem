using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.Request.User
{
    public class UpdateUserDTO
    {
        [Required(ErrorMessage = "Bạn chưa điền họ tên hoặc tên trường.")]
        public string FullNameOrSchoolName { get; set; }

        [Required(ErrorMessage = "Bạn chưa điền username.")]
        [RegularExpression("^[a-zA-Z0-9-._@+]{3,20}$", ErrorMessage = "Username cần có 3-20 kí tự.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Bạn chưa điền email.")]
        [EmailAddress(ErrorMessage = "Sai format email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bạn chưa điền số điện thoại.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Vui lòng nhập số điện thoại hợp lệ!")]
        public string PhoneNumber { get; set; }
    }
}
