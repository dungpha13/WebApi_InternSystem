using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("Comment")]
    public class Comment : Entity
    {
        public string? Content { get; set; }

        [ForeignKey("NguoiDuocComment")]
        public string IdNguoiDuocComment { get; set; }
        public InternInfo NguoiDuocComment { get; set; }

        [ForeignKey("NguoiComment")]
        public string IdNguoiComment { get; set; }
        public User NguoiComment { get; set; }
    }
}