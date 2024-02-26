using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Validation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AmazingTech.InternSystem.Models.Request.InternInfo
{
    public class UpdateInternInfoDTO
    {
        //[Required]
        //public string MSSV { get; set; }
        [Required]
        public string HoTena { get; set; }
        [Required]
        [ValidateDateTime(ErrorMessage = "Vui lòng nhập vào một ngày hợp lệ và đúng định dạng dd/MM/yyyy (hoặc yyyy-MM-dd)")]
        public string NgaySinh { get; set; }
        [Required]
        [ValidateDateTime(ErrorMessage = "Vui lòng nhập vào một ngày hợp lệ và đúng định dạng dd/MM/yyyy (hoặc yyyy-MM-dd)")]
        public string? StartDate { get; set; }
        [Required]
        [CompareDateTime("StartDate", ErrorMessage = "Vui lòng nhập vào một ngày hợp lệ và đúng định dạng dd/MM/yyyy (hoặc yyyy-MM-dd)")]  //
        public string? EndDate { get; set; }
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Vui lòng nhập số điện thoại hợp lệ!")]
        public string Sdt { get; set; }
        [Required]
        public string DiaChi { get; set; }
        [Required]
        public bool GioiTinh { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email không đúng định dạng!")]
        public string? EmailCaNhan { get; set; }
        
        [Required]
        public string LinkCV { get; set; }
        [Required]
        public string TrinhDoTiengAnh { get; set; }

        //[JsonPropertyName("idViTri")]
        //[Required]
        //public string[] ViTrisId { get; set; }

        //[Required]
        //public string[] IdDuAn { get; set; }

        //[Required]
        //public string[] IdNhomZalo { get; set; }

        [Required]
        public string IdTruong { get; set; }

        [Required]
        [Range(0,2, ErrorMessage = "Hãy nhập chính xác vòng sơ tuyển Intern: \\n" +
            "- 0: Chờ xét duyệt CV \\n" +
            "- 1: Chờ phỏng vấn \\n" +
            "- 2: Đậu phỏng vấn")]
        //Round 0: Vòng duyệt CV, Round 1: Pass CV (Vòng Phỏng Vấn), Round 2: Đậu Phỏng Vấn (Intern chính thức)
        public int Round { get; set; }
        
        //[Required]
        //public string? Status { get; set; }
       
    }
}
