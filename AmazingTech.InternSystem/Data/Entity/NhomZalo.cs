using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("NhomZalo")]
    public class NhomZalo : Entity
    {
        public string? TenNhom { get; set; }

        public string? LinkNhom { get; set; }

        [ForeignKey("User")]
        public string IdMentor { get; set; }
        public User Mentor { get; set; }

        public ICollection<UserNhomZalo> UserNhomZalos { get; set; } = new List<UserNhomZalo>();
    }
}
