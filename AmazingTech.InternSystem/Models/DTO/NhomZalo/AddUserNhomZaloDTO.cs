using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.DTO.NhomZalo
{
    public class AddUserNhomZaloDTO
    {
        [Required]
        public string UserId { get; set; }
        public bool IsMentor { get; set; }
    }
}
