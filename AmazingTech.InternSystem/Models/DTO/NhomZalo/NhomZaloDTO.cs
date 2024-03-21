using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.DTO.NhomZalo
{
    public class NhomZaloDTO
    {
        [Required]
        public string TenNhom { get; set; }
        [Required]
        public string LinkNhom { get; set; }
    }
}
