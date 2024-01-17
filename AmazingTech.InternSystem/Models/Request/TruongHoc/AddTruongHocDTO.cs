using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.Request.TruongHoc
{
    public class AddTruongHocDTO
    {
        [Required(ErrorMessage = "Name cannot be empty")]
        public string Ten { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The number of internship weeks must be a positive integer")]
        public int SoTuanThucTap { get; set; }
    }
}