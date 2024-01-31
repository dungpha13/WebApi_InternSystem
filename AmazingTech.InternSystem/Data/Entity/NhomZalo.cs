using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("NhomZalo")]
    public class NhomZalo : AbstractEntity
    {
        public string? TenNhom { get; set; }

        public string? LinkNhom { get; set; }
        public ICollection<UserNhomZalo> UserNhomZalos { get; set; }
    }
}
