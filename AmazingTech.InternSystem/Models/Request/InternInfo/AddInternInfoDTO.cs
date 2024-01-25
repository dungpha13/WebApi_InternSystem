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
        public string Sdt { get; set; }
        public string DiaChi { get; set; }
        public bool GioiTinh { get; set; }

        [Required]
        [EmailAddress]        
        
        public string EmailTruong { get; set; }

        public string LinkCV { get; set; }
        public string TrinhDoTiengAnh { get; set; }

        [JsonPropertyName("idViTri")]
        public string[] ViTrisId{ get; set; }
        public string[] IdDuAn { get; set; }
        public string[] IdNhomZalo { get; set; } 
    }
}
