﻿using AmazingTech.InternSystem.Data.Enum;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
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
        [DefaultValue("GETDATE()")]
        public DateTime? CreatedTime { get; set; } = DateTime.Now;
        public DateTime? DeletedTime { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpdatedTime { get; set; }
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public string? VerificationToken { get; set; }
        public DateTime? VerificationTokenExpires { get; set; }
        public bool isConfirmed { get; set; }
        [ForeignKey("InternInfo")]
        public string? InternInfoId { get; set; }
        public InternInfo? InternInfo { get; set; }
        public TrangThaiThucTap TrangThaiThucTap { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<LichPhongVan> LichPhongVans_PhongVan { get; set; }
        public ICollection<LichPhongVan> LichPhongVans_DuocPhongVan { get; set; }
        public ICollection<ThongBao> ThongBao_NguoiGui { get; set; }
        public ICollection<ThongBao> ThongBao_NguoiNhan { get; set; }
        public ICollection<UserDuAn> UserDuAns { get; set; }
        public ICollection<UserViTri> UserViTris { get; set; }
        public ICollection<UserNhomZalo> UserNhomZalos { get; set; }

        public ICollection<EmailUserStatus> EmailUserStatuses { get; set; }


    }
}
