using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("NhomZalo")]
    public class NhomZalo : AbstractEntity
    {
        public string? TenNhom { get; set; }

        public string? LinkNhom { get; set; }

        [JsonIgnore]
        public ICollection<UserNhomZalo> UserNhomZalos { get; set; }
    }
}
