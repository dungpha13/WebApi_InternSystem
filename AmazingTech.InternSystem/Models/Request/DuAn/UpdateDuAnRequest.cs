using AmazingTech.InternSystem.Data.Entity;

namespace AmazingTech.InternSystem.Models.Request.DuAn
{
    public class UpdateDuAnRequest
    {
        public AmazingTech.InternSystem.Data.Entity.DuAn UpdatedDuAn { get; set; }
        public List<string> ViTriIds { get; set; }
        public List<string> CongNgheIds { get; set; }
        public List<string> LeaderUserIds { get; set; }
    }
}
