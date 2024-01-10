using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("Users")]
    public class User
    {
        [Key]
        public string Id { get; set; }
        public string HoVaTen { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Password { get; set; }
        public string Sdt { get; set; }
        public bool SdtXacNhan { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime DeletedTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public string ResetToken { get; set; }
        public DateTime ResetTokenExpires { get; set; }
        public string VerificationToken { get; set; }
        public DateTime VerificationTokenExpires { get; set; }
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
        public virtual ICollection<ViTri> ViTris { get; set; } = new List<ViTri>();
        public ICollection<Comment> Comments { get; set; }
        public InternInfo? InternInfo { get; set; }
        public ICollection<LichPhongVan> LichPhongVans_PhongVan { get; set; }
        public ICollection<LichPhongVan> LichPhongVans_DuocPhongVan { get; set; }
        public ICollection<ThongBao> ThongBao_NguoiGui { get; set; }
        public ICollection<ThongBao> ThongBao_NguoiNhan { get; set; }
        public ICollection<UserDuAn> UserDuAns { get; set; }
        public ICollection<UserTokens> UserTokens { get; set; } = new List<UserTokens>();
        public ICollection<UserNhomZalo> UserNhomZalos { get; set; } = new List<UserNhomZalo>();
    }
}
