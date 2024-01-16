using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("UserDuAn")]
    public class UserDuAn : AbstractEntity
    {
        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("DuAn")]
        public string IdDuAn { get; set; }
        public string? ViTri { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public DuAn DuAn { get; set; }
    }
}