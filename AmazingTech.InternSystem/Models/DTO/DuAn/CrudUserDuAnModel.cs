using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.DTO.DuAn
{
    public class CrudUserDuAnModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string? ViTri { get; set; }
    }
}
