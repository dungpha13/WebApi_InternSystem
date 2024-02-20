

using AmazingTech.InternSystem.Data.Entity;
using System.Globalization;

namespace AmazingTech.InternSystem.Models.Response.InternInfo
{
    public class InternInfoDTO
    {
        public string Id { get; set; }
        public string MSSV { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string HoTen { get; set; }
        public string NgaySinh { get; set; }
        public string Sdt { get; set; }
        public string[] ViTri { get; set; }
        public string DiaChi { get; set; }
        public string? EmailCaNhan { get; set; }
        public string EmailTruong { get; set; }
        public string LinkCV { get; set; }
        public string GioiTinh { get; set; }
        public string TrinhDoTiengAnh { get; set; }
        public string[] DuAn { get; set; }
        public string[] NhomZalo { get; set; }
        public string TruongHoc { get; set; }
        public string KiThucTap { get; set; }

        public string Round { get; set; }
        public string Status { get; set; }
        public string ViTriMongMuon { get; set; }

        //public ICollection<Comment> Comments { get; set; }

        public string CreatedTime { get; set; }
        public string CreatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public string DeletedTime { get; set; }
        public string LastUpdatedBy { get; set; }
        public string LastUpdatedTime { get; set; }

        //public string SdtNguoiThan { get; set; }     
        //public decimal GPA { get; set; }
        //public string TrinhDoTiengAnh { get; set; }
        //public string ChungChi { get; set; }
        //public string LinkFacebook { get; set; }
        //public string NganhHoc { get; set; }
        //public int Round { get; set; }
        //public string KiThucTapId { get; set; }

    }
}
