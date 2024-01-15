using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("ViTri")]
    public class ViTri : Entity
    {
        public string? Ten { get; set; }
        public string? LinkNhomZalo { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
