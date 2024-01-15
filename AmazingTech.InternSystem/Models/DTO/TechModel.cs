using AmazingTech.InternSystem.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;


namespace AmazingTech.InternSystem.Models.DTO
{
    public class TechModel : Entity
    {
        public string Ten { get; set; }
        public string IdViTri { get; set; }

        public string? ImgUrl { get; set; }

    }
}
