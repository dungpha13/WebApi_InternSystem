using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("UserDuAn")]
    public class UserDuAn : Entity
    {
        [ForeignKey("UserId")]
        public string UserId { get; set; }

        [ForeignKey("IdDuAn")]
        public string IdDuAn { get; set; }
        public string ViTri { get; set; }
        public User User { get; set; }
        public DuAn DuAn { get; set; }
    }
}