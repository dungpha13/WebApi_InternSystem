using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Data.Entity
{
    public class CauhoiCongnghe :AbstractEntity
    {
        [ForeignKey("CongNghe")]
        public string IdCongNghe { get; set; }
        public CongNghe CongNghe { get; set; }

        [ForeignKey("Cauhoi")]
        public string IdCauhoi { get; set; }
        public Cauhoi cauhoi { get; set; }
    }
}
