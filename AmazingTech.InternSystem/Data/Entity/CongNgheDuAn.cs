using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Data
{
    [Table("CongNgheDuAn")]
    public class CongNgheDuAn : Entity
    {
        public string TenCongNghe { get; set; }
        public string IdDuAn { get; set; }
        public DuAn DuAn { get; set; }
    }
}