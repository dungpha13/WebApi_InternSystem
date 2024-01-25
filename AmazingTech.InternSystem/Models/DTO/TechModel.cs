using System.ComponentModel.DataAnnotations;


namespace AmazingTech.InternSystem.Models.DTO
{
    public class TechModel 
    {
        [Required]
        public string Ten { get; set; }
        
        [Required]
        public string IdViTri { get; set; }

        [Required]
        public string? ImgUrl { get; set; }

    }
}
