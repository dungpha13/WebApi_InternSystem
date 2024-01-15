using AmazingTech.InternSystem.Data.Entity;

namespace AmazingTech.InternSystem.Models.Request.InternInfo
{
    public class UpdateInternInfoDTO
    {
        
        
        public string? MSSV { get; set; }
        
        public string? HoTen { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? Sdt { get; set; }
        public bool? GioiTinh { get; set; }
        //public string[] IdViTris { get; set; }
        //public KiThucTap? KiThucTap { get; set; }
        public string? DiaChi { get; set; }
        public string? EmailTruong { get; set; }
        public string? LinkCV { get; set; }
        public string? EmailCaNhan { get; set; }

        
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? Status { get; set; }

       // public ICollection<Comment>? Comments { get; set; }


    }
}
