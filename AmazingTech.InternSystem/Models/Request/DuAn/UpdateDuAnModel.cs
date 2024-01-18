using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.Request.DuAn
{
    public class UpdateDuAnModel
    {
        [Required(ErrorMessage = "The 'Id' field is required.")]
        public string Id { get; set; }
        [Required(ErrorMessage = "The 'Ten' field is required.")]
        public string? Ten { get; set; }
        public DateTime? ThoiGianBatDau { get; set; }
        public DateTime? ThoiGianKetThuc { get; set; }
    }
}
