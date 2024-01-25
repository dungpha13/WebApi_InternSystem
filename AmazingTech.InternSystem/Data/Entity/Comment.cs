using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("Comment")]
    public class Comment : AbstractEntity
    {
        public string? Content { get; set; }

        [ForeignKey("NguoiDuocComment")]
        public string IdNguoiDuocComment { get; set; }

        [JsonIgnore]
        public InternInfo? NguoiDuocComment { get; set; }

        [ForeignKey("NguoiComment")]
        public string IdNguoiComment { get; set; }

      
        public User NguoiComment { get; set; }
    }
}