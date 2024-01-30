using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Response.InternInfo;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Models.Response
{
    public class KiThucTapResponseDTO
    {
        public string Id { get; set; }
        public string? Ten { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public ICollection<InternInfoDTO> Interns { get; set; }
        public string? TenTruong { get; set; }
    }
}
