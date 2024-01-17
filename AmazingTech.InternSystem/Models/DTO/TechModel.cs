using AmazingTech.InternSystem.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


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
