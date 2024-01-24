using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("InternTruongKyThucTap")]
    public class InternTruongKyThucTap : AbstractEntity
    {
        [ForeignKey("InternInfo")]
        public string IdIntern { get; set; }
        public InternInfo Intern { get; set; }

        [ForeignKey("TruongHoc")]
        public string IdTruongHoc { get; set; }
        public TruongHoc TruongHoc { get; set; }

        [ForeignKey("KiThucTap")]
        public string? IdKiThucTap { get; set; }
        public KiThucTap KiThucTap { get; set; }

    }
}