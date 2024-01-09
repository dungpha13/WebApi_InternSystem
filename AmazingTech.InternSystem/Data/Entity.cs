using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Data
{
    public abstract class Entity
    {
        [Key]
        public string Id { get; set; }
        public User CreatedBy { get; set; }
        public User LastUpdatedBy { get; set; }
        public User? DeletedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public DateTime? DeletedTime { get; set; }
    }
}
