using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace AmazingTech.InternSystem.Data.Entity
{
    public class User : IdentityUser
    {
        public string? HoVaTen { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime? CreatedTime { get; set; } = DateTime.Now;
        public DateTime? DeletedTime { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpdatedTime { get; set; }
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public string? VerificationToken { get; set; }
        public DateTime? VerificationTokenExpires { get; set; }
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
        public virtual ICollection<ViTri> ViTris { get; set; } = new List<ViTri>();
        public ICollection<Comment> Comments { get; set; }
        public InternInfo? InternInfo { get; set; }
        public ICollection<LichPhongVan> LichPhongVans_PhongVan { get; set; }
        public ICollection<LichPhongVan> LichPhongVans_DuocPhongVan { get; set; }
        public ICollection<ThongBao> ThongBao_NguoiGui { get; set; }
        public ICollection<ThongBao> ThongBao_NguoiNhan { get; set; }
        public ICollection<UserDuAn> UserDuAns { get; set; }
        public ICollection<UserNhomZalo> UserNhomZalos { get; set; } = new List<UserNhomZalo>();
    }
}
