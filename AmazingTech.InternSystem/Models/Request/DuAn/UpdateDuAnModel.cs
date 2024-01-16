using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.Request.DuAn
{
    public class UpdateDuAnModel
    {
       [Required]
        public string Id { get; set; }
        public string? Ten { get; set; }
        public DateTime? ThoiGianBatDau { get; set; }
        public DateTime? ThoiGianKetThuc { get; set; }
    }
}
