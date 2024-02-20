using AmazingTech.InternSystem.Models.Validation;
using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("DuAn")]
    public class DuAn : AbstractEntity
    {
        public string? Ten { get; set; }

        [ForeignKey("Leader")]
        public string LeaderId { get; set; }
        public User Leader { get; set; }

        [Display(Name = "Thời Gian Bắt Đầu")]
        [Required(ErrorMessage = "Thời gian bắt đầu không được trống.")]
        [ValidateDateTime(ErrorMessage = "Vui lòng nhập vào một ngày hợp lệ và đúng định dạng dd/MM/yyyy (hoặc yyyy-MM-dd)")]
        public DateTime? ThoiGianBatDau { get; set; }

        [Display(Name = "Thời Gian Kết Thúc")]
        [Required(ErrorMessage = "Thời gian kết thúc không được trống.")]
        [CompareDateTime("StartDate", ErrorMessage = "Thời gian kết thúc phải sau thời gian bắt đầu")]
        public DateTime? ThoiGianKetThuc { get; set; }

        [JsonIgnore]
        public ICollection<UserDuAn> UserDuAns { get; set; }
        public ICollection<ViTri> ViTris { get; set; }
        public ICollection<CongNgheDuAn> CongNgheDuAns { get; set; }
        public ICollection<InternInfo> InternInfos { get;  set; }
    }
}