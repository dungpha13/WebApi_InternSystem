using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.Request.KiThucTap
{
    public class UpdateKiThucTapDTO
    {
        [Required]
        public string Id { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
    }
}