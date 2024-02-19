using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.DTO
{
    public class AwserQuest
    {
        [Required]
        public string IdCauHoiCongNghe { get; set; }

        [Required]
        public string CauTraLoi { get; set; }
    }
}
