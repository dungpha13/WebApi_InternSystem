using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("EmailUserStatus")]
    public class EmailUserStatus : AbstractEntity
    {
        [ForeignKey("idNguoiGui")]
        public string idNguoiGui { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        [ForeignKey("idNguoiNhan")]
        public string idNguoiNhan { get; set; }
        
        public InternInfo Intern { get; set; }
        public bool? EmailLoai1 { get; set; }
        public bool? EmailLoai2 { get; set; }
        public bool? EmailLoai3 { get; set; }
    }
}
