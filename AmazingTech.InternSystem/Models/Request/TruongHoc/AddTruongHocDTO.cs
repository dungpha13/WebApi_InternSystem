using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.Request.TruongHoc
{
    public class AddTruongHocDTO
    {
        [Required(ErrorMessage = "Ten khong duoc trong")]
        public string Ten { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "SoTuanThucTap khong duoc am")]
        public int SoTuanThucTap { get; set; }
    }
}