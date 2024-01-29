using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Spreadsheet;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Text.Json.Serialization;

namespace AmazingTech.InternSystem.Data.Entity
{
    public class WorkReport
    {
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("DuAn")]
        public string IdDuAn { get; set; }
        public DuAn DuAn { get; set; }
        public string NoiDungBaoCao { get; set; }
        public bool IsConfirm { get; set; } 
        public string TenAdminDuyet { get; set; }
    }
}
