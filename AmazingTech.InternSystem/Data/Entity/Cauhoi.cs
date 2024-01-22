using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Data.Entity
{
    public class Cauhoi : AbstractEntity
    {
        public string? NoiDung { get; set; }
       
        public ICollection<CauhoiCongnghe> CauhoiCongnghe { get; set; }

    }
}
