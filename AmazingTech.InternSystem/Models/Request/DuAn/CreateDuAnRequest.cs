using AmazingTech.InternSystem.Data.Entity;

namespace AmazingTech.InternSystem.Models.Request.DuAn
{
    public class CreateDuAnRequest
    {
        public AmazingTech.InternSystem.Data.Entity.DuAn CreatedDuAn { get; set; }
        public List<string> ViTriIds { get; set; }
        public List<string> CongNgheIds { get; set; }
        public List<string> LeaderUserIds { get; set; }
    }
}
