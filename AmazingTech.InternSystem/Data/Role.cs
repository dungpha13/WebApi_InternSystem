using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Data
{
    [Table("Role")]
    public class Role
    {
        [Key]
        public string Id { get; set; }
        public Roles Name { get; set; }
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }

    public enum Roles
    {
        Admin,
        HR,
        Mentor,
        Intern,
        School
    }
}
