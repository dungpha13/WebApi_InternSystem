using AmazingTech.InternSystem.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AmazingTech.InternSystem.Models.Request.InternInfo
{
    public class UpdateInternInfoDTO
    {
        public string MSSV { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Sdt { get; set; }
        public string DiaChi { get; set; }
        public bool GioiTinh { get; set; }
        [Required]
        [EmailAddress]
        public string? EmailCaNhan { get; set; }
        [Required]
        [EmailAddress]
        public string EmailTruong { get; set; }
        public string LinkCV { get; set; }
        public string TrinhDoTiengAnh { get; set; }

        [JsonPropertyName("idViTri")]
        [Required]
        public string[] ViTrisId { get; set; }

        [Required]
        public string[] IdDuAn { get; set; }

        [Required]
        public string[] IdNhomZalo { get; set; }

        public string? Status { get; set; }
       

    }
}
