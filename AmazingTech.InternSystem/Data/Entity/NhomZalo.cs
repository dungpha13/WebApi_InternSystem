using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("NhomZalo")]
    public class NhomZalo : Entity
    {
        public string TenNhom { get; set; }

        public string LinkNhom { get; set; }

        [ForeignKey("Mentor")]
        public string MentorId { get; set; }
        public User Mentor { get; set; }

    }
}
