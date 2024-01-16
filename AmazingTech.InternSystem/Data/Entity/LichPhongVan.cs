using AmazingTech.InternSystem.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("LichPhongVan")]
    public class LichPhongVan : Entity
    {
        [ForeignKey("NguoiPhongVan")]
        public string IdNguoiPhongVan { get; set; }
        public virtual User? NguoiPhongVan { get; set; }

        [ForeignKey("NguoiDuocPhongVan")]
        public string IdNguoiDuocPhongVan { get; set; }
        public virtual User? NguoiDuocPhongVan { get; set; }

        public DateTime? ThoiGianPhongVan { get; set; }

        public string? DiaDiemPhongVan { get; set; }

        public bool? DaXacNhanMail { get; set; }
        public InterviewForm InterviewForm { get; set; }

        public Status TrangThai { get; set; } // Chua PV/Da PV
        public TimeSpan TimeDuration { get; set; }

        public Result KetQua { get; set; } // Passed/Failed

    }
}
