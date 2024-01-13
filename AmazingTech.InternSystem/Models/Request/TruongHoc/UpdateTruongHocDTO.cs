using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.Request.TruongHoc
{
    public class UpdateTruongHocDTO
    {
        [Required]
        public string Id { get; set; }
        public string? Ten { get; set; }
        public int? SoTuanThucTap { get; set; }
    }
}