using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("TruongHoc")]
    public class TruongHoc : Entity
    {
        public string Ten { get; set; }
        public int SoTuanThucTap { get; set; }
        public ICollection<KiThucTap> KiThucTaps { get; set; }
    }
}