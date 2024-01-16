using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("DuAn")]
    public class DuAn : Entity
    {
        public string? Ten { get; set; }

        [ForeignKey("Leader")]
        public string LeaderId { get; set; }
        public User Leader { get; set; }
        public DateTime? ThoiGianBatDau { get; set; }
        public DateTime? ThoiGianKetThuc { get; set; }
        public ICollection<UserDuAn> UserDuAns { get; set; }
        public ICollection<ViTri> ViTris { get; set; }
        public ICollection<CongNgheDuAn> CongNgheDuAn { get; set; }

    }
}