using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.DTO
{
    public class QuestionDTO
    {
        [Required]
        public string? NoiDung { get; set; }
    }
}
