using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.Request.TruongHoc
{
    public class UpdateTruongHocDTO
    {

        [Required(ErrorMessage = "Id is required")]
        public string Id { get; set; }

        public string? Ten { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The number of internship weeks must be a positive integer")]
        public int? SoTuanThucTap { get; set; }
    }
}