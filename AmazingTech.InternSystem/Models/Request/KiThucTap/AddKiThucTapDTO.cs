using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.Request.KiThucTap
{
    public class AddKiThucTapDTO
    {
        [Required(ErrorMessage = "NgayBatDau la bat buoc")]
        public DateTime NgayBatDau { get; set; }

        [Required(ErrorMessage = "NgayKetThuc la bat buoc")]
        [Compare(nameof(NgayBatDau), ErrorMessage = "NgayKetThuc phai lon hon hoac bang NgayBatDau")]
        public DateTime NgayKetThuc { get; set; }

        [Required(ErrorMessage = "IdTruong la bat buoc")]
        public string IdTruong { get; set; }
    }
}