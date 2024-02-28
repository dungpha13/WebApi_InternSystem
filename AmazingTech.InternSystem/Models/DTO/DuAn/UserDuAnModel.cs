using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.DTO.DuAn
{
    public class UserDuAnModel
    {
        [Required]
        public string UserId { get; set; }
        public string UserName { get; set; }

        //public string IdDuAn { get; set; }
        //public string TenDuAn { get; set; }
        [Required]
        public string? ViTri { get; set; }
    }
}
