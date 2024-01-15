using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Data.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Models.Response
{
    public class LichPhongVanResponseModel
    {
        public string ID { get; set; }
        public string NguoiPhongVan { get; set; }

        public string NguoiDuocPhongVan { get; set; }
        public DateTime? ThoiGianPhongVan { get; set; }

        public string DiaDiemPhongVan { get; set; }

        public string InterviewForm { get; set; }

        public string TrangThai { get; set; } // Chua PV/Da PV

        public string KetQua { get; set; } // Passed/Failed
    }
}
