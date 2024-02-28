using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.Request.TruongHoc
{
    public class UpdateTruongHocDTO
    {

        [Required(ErrorMessage = "Id la bat buoc")]
        public string Id { get; set; }
        public string? Ten { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "SoTuanThucTap khong duoc am")]
        public int? SoTuanThucTap { get; set; }
    }
}