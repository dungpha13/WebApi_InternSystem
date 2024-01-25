using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("ThongBao")]
    public class ThongBao : AbstractEntity
    {
        [ForeignKey("NguoiNhan")]
        public string IdNguoiNhan { get; set; }
        public virtual User NguoiNhan { get; set; }

        [ForeignKey("NguoiGui")]
        public string IdNguoiGui { get; set; }
        public virtual User NguoiGui { get; set; }

        public string? TieuDe { get; set; }

        public string? NoiDung { get; set; }

        public bool TinhTrang { get; set; } // Da doc/chua doc

    }
}
