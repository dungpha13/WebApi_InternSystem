using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.Request.DuAn
{
    public class AddDuAnModel
    {
        [Required]
        public string? Ten { get; set; }
        public string LeaderId { get; set; }
        public DateTime? ThoiGianBatDau { get; set; }
        public DateTime? ThoiGianKetThuc { get; set; }
    }
}
