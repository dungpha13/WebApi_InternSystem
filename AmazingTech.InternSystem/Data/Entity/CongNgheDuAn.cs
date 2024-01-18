using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("CongNgheDuAn")]
    public class CongNgheDuAn : AbstractEntity
    {
        [ForeignKey("CongNghe")]
        public string IdCongNghe { get; set; }
        public CongNghe CongNghe { get; set; }

        [ForeignKey("DuAn")]
        public string IdDuAn { get; set; }
        public DuAn DuAn { get; set; }
    }
}