using AmazingTech.InternSystem.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace AmazingTech.InternSystem.Models
{
    public class LichPhongVanRequestModel
    {
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ThoiGianPhongVan { get; set; }

        public string DiaDiemPhongVan { get; set; }
    }
}
