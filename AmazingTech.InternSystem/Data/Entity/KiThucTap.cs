using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("KiThucTap")]
    public class KiThucTap : Entity
    {
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }

        [ForeignKey("TruongHoc")]
        public string IdTruong { get; set; }
        public TruongHoc TruongHoc { get; set; }

        public ICollection<InternInfo> InternInfos { get; set; }
    }
}