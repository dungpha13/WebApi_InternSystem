using AmazingTech.InternSystem.Models.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AmazingTech.InternSystem.Models.Request.InternInfo
{
    public class AddInternInfoDTO
    {
        [Required]
        public string MSSV { get; set; }
        [Required]
        public string HoTen { get; set; }
        [Required]
        [ValidateDateTime(ErrorMessage = "Vui lòng nhập vào một ngày hợp lệ và đúng định dạng yyyy-MM-dd")]
        public string NgaySinh { get; set; }
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Vui lòng nhập số điện thoại hợp lệ!")] //Kiểm tra sđt chính xác 10 ký tự và phải là chữ số
        public string Sdt { get; set; }
        [Required]
        public string DiaChi { get; set; }
        [Required]
        public bool GioiTinh { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email không đúng định dạng!")]
        public string EmailCaNhan { get; set; }
        [Required]
        [EmailAddress]
        public string EmailTruong { get; set; }
        [Required]
        public string LinkCV { get; set; }
        [Required]
        public string TrinhDoTiengAnh { get; set; }
        [Required]
        public string? ViTriMongMuon { get; set; }

        //[Required]
        //public string Role { get; set; }
        [Required]
        public string IdTruong { get; set; }
        [Required]
        [JsonPropertyName("idKiThucTap")]
        public string? KiThucTapId { get; set; }
    }
}
