using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("CongNghe")]
    public class CongNghe : AbstractEntity
    {
        public string? Ten { get; set; }

        [ForeignKey("ViTri")]
        public string IdViTri { get; set; }

        public string? ImgUrl { get; set; }
        public ViTri ViTri { get; set; }
        public ICollection<CongNgheDuAn> CongNgheDuAns { get; set; }
    }
}