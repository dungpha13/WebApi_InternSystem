using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("TruongHoc")]
    public class TruongHoc : AbstractEntity
    {
        public string? Ten { get; set; }
        public int? SoTuanThucTap { get; set; }
        public ICollection<InternTruongKyThucTap> InternTruongKyThucTaps { get; set; }
    }
}