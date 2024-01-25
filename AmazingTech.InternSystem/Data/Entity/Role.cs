using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Data.Entity
{
    public class Role : IdentityRole
    {
        [Key]
        public string Id { get; set; }
        [NotMapped]
        public Roles Name { get; set; }
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }

    [Keyless]
    public class Roles
    {
        public const string ADMIN = "Admin";
        public const string HR = "HR";
        public const string SCHOOL = "School";
        public const string MENTOR = "Mentor";
        public const string INTERN = "Intern";
    }
}
