using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.VItri
{
    public class VitriModel
    {
        [Required]
        public string? Ten { get; set; }
        [Required]
        public string? LinkNhomZalo { get; set; }
        //public virtual ICollection<User> Users { get; set; }


    }
}
