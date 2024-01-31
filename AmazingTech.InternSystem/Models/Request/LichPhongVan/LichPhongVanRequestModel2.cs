using AmazingTech.InternSystem.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.Request.LichPhongVan
{
    public class LichPhongVanRequestModel2
    {
        [EmailAddress]
        public List<string> Email { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ThoiGianPhongVan { get; set; }
        public InterviewForm interviewForm { get; set; }
        public int TimeDuration { get; set; }
        public string MailNgPhongVan { get; set; }
        public string DiaDiemPhongVan { get; set; }
    }
}
