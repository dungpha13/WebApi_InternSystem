using AmazingTech.InternSystem.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Models.DTO
{
    public class TechView
    {
        public string Id { get; set; } 
        public string? Ten { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? LastUpdatedTime { get; set; }
        public DateTime? DeletedTime { get; set; }
        public string IdViTri { get; set; }
        public string? ImgUrl { get; set; }
        public ViTri ViTri { get; set; }
    }
}
