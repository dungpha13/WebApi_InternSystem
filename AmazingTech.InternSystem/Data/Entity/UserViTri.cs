using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("UserViTri")]
    public class UserViTri : AbstractEntity
    {
        [ForeignKey("User")]
        public string UsersId { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        [ForeignKey("ViTri")]
        public string ViTrisId { get; set; }
        public ViTri ViTri { get; set; }
    }
}
