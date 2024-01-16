using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace AmazingTech.InternSystem.Data.Entity
{
    public abstract class AbstractEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("GETDATE()")]
        public DateTime? CreatedTime { get; set; } 
      
        public DateTime? LastUpdatedTime { get; set; } 

        public DateTime? DeletedTime { get; set; } 
    }
}
