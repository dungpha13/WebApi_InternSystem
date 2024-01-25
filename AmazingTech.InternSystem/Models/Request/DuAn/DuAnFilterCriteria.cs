namespace AmazingTech.InternSystem.Models.Request.DuAn
{
    public class DuAnFilterCriteria
    {
        public string Id { get; set; }
        public string? Ten { get; set; }
        public string LeaderId { get; set; }
        public DateTime? ThoiGianBatDau { get; set; }
        public DateTime? ThoiGianKetThuc { get; set; }
    }
}
