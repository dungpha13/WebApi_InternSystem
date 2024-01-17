using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("InternInfo")]
    public class InternInfo : AbstractEntity
    {


        [ForeignKey("User")]
        public string? UserId { get; set; }
        public User? User { get; set; }
        public string HoTen { get; set; }
        public DateTime? NgaySinh { get; set; }
        public bool GioiTinh { get; set; }
        public string? MSSV { get; set; }
        public string? EmailTruong { get; set; } = null;
        public string? EmailCaNhan { get; set; }
        public string? Sdt { get; set; }
        public string? SdtNguoiThan { get; set; }
        public string? DiaChi { get; set; }
        public decimal? GPA { get; set; }
        public string? TrinhDoTiengAnh { get; set; }
        public string? ChungChi { get; set; }
        public string? LinkFacebook { get; set; }
        public string? LinkCV { get; set; }
        public string? NganhHoc { get; set; }
        public string? Status { get; set; }
        public int? Round { get; set; }
        public string? KiThucTapId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        [JsonIgnore]
        public KiThucTap? KiThucTap { get; set; }
    }
}