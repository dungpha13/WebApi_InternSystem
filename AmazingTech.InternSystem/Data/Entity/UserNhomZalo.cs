using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("UserNhomZalo")]
    public class UserNhomZalo : AbstractEntity
    {
        [ForeignKey("User")]
        public string UserId { get; set; }
        public bool IsMentor{ get; set; }

        [JsonIgnore]
        public User User { get; set; }

        [ForeignKey("NhomZalo")]
        public string IdNhomZalo { get; set; }
        public NhomZalo NhomZalo { get; set; }

        public DateTime? JoinedTime { get; set; }

        public DateTime? LeftTime { get; set; }
    }
}
