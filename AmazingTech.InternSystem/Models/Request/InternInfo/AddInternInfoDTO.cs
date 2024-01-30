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
        public DateTime NgaySinh { get; set; }
        [Required]
        [RegularExpression(@"^\+(?:[0-9] ?){6,14}[0-9]$", ErrorMessage = "Vui lòng nhập số điện thoại hợp lệ!")] //Kiểm tra sđt chính xác 10 ký tự và phải là chữ số
        public string Sdt { get; set; }
        [Required]
        public string DiaChi { get; set; }
        [Required]
        public bool GioiTinh { get; set; }

        [Required]
        [EmailAddress]        
        
        public string EmailTruong { get; set; }
        [Required]
        public string LinkCV { get; set; }
        [Required]
        public string TrinhDoTiengAnh { get; set; }

        [JsonPropertyName("idViTri")]
        [Required]
        public string[] ViTrisId{ get; set; }
        [Required]
        public string[] IdDuAn { get; set; }
        [Required]
        public string[] IdNhomZalo { get; set; }

        //[Required]
        //public string Role { get; set; }
        [Required]
        public string IdTruong { get; set; }
    }
}
