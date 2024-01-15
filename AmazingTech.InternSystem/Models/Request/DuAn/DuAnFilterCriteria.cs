namespace AmazingTech.InternSystem.Models.Request.DuAn
{
    public class DuAnFilterCriteria
    {
        public string? Ten { get; set; }
        public DateTime? ThoiGianBatDauFrom { get; set; }
        public DateTime? ThoiGianBatDauTo { get; set; }

        public DateTime? ThoiGianKetThucFrom { get; set; }
        public DateTime? ThoiGianKetThucTo { get; set; }
        public string? LeaderId { get; set; }
        public string? IdCongNghe { get; set; }
        public string? IdDuAn { get; set; }
        public string? UserId { get; set; }
    }
}
