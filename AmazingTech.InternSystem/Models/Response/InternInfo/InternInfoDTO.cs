

using AmazingTech.InternSystem.Data.Entity;
using System.Globalization;

namespace AmazingTech.InternSystem.Models.Response.InternInfo
{
    public class InternInfoDTO
    {
        public string MSSV { get; set; }
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
        public string HoTen { get; set; }
        public string NgaySinh { get; set; }
        public string Sdt { get; set; }
        public bool GioiTinh { get; set; }
        public string[] ViTri { get; set; }
        public KiThucTap KiThucTap { get; set; }
        public string DiaChi { get; set; }
        public string EmailTruong { get; set; }
        public string LinkCV { get; set; }
        public string? EmailCaNhan { get; set; }

        public string CreatedTime { get; set; } 

        public string? DeletedBy { get; set; }
        public string DeletedTime { get; set; }

        public string LastUpdateBy { get;set; }
        public string LastUpdatedTime { get; set; }

        public string Status { get; set; }

        public ICollection<Comment> Comments { get; set; }
        // public string SdtNguoiThan { get; set; }     
        //public decimal GPA { get; set; }
        //public string TrinhDoTiengAnh { get; set; }
        //public string ChungChi { get; set; }
        //public string LinkFacebook { get; set; }
        //public string NganhHoc { get; set; }
        //public int Round { get; set; }
        //public string KiThucTapId { get; set; }


    }
}
