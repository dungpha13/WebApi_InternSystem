using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.DTO.NhomZalo
{
    public class UpdateUserNhomZaloDTO
    {
        [Required]
        public string UserId { get; set; }
        public bool IsMentor { get; set; }
        public DateTime? JoinedTime { get; set; }
        public DateTime? LeftTime { get; set; }
    }
}
