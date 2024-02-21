using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("PhongVan")]
    public class PhongVan : AbstractEntity
    {
        public string? CauTraLoi { get; set; }       
        public int? Rank { get; set; }
        public string? NguoiCham { get; set; }
        public DateTime? RankDate { get; set; }

        [ForeignKey("CauHoiCongNghe")]
        public string IdCauHoiCongNghe { get; set; }
        public CauhoiCongnghe CauhoiCongnghes { get; set; }

        [ForeignKey("LichPhongVan")]
        public string IdLichPhongVan { get; set; }
        public LichPhongVan LichPhongVan { get; set; }
    }
}
