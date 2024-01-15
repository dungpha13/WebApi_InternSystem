using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.Request.KiThucTap
{
    public class AddKiThucTapDTO
    {
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }

        [Required]
        public string IdTruong { get; set; }
    }
}