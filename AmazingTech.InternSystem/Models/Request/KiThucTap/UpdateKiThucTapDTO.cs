using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.Request.KiThucTap
{
    public class UpdateKiThucTapDTO
    {
        [Required(ErrorMessage = "Id la bat buoc")]
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? NgayBatDau { get; set; }

        [Compare(nameof(NgayBatDau), ErrorMessage = "NgayKetThuc phai lon hon hoac bang NgayBatDau")]
        public DateTime? NgayKetThuc { get; set; }
    }
}