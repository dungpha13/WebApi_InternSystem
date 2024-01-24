using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("KiThucTap")]
    public class KiThucTap : AbstractEntity
    {
        public string? Ten { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public ICollection<InternInfo> Interns { get; set; }

        [ForeignKey("Truong")]
        public string? IdTruong { get; set; }
        public TruongHoc? Truong { get; set; }
    }
}