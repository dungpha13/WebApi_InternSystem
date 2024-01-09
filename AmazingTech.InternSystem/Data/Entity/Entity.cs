using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Data.Entity
{
        public abstract class Entity
        {
                [Key]
                public string Id { get; set; }
                // public string CreatedBy { get; set; }
                // public string LastUpdatedBy { get; set; }
                // public string? DeletedBy { get; set; }
                // public DateTime CreatedTime { get; set; }
                // public DateTime LastUpdatedTime { get; set; }
                // public DateTime? DeletedTime { get; set; }
        }
}
