using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Data
{
    [Table("Users")]
    public class User
    {
        [Key]
        public string Id { get; set; }
        public string HoVaTen { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Password { get; set; }
        public string Sdt { get; set; }
        public bool SdtXacNhan {  get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime DeletedTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public string ResetToken { get; set; }
        public DateTime ResetTokenExpires { get; set; }
        public string VerificationToken { get; set; }
        public DateTime VerificationTokenExpires { get; set; }
        public virtual ICollection<Role> Users { get; set; } = new List<Role>();
    }
}
