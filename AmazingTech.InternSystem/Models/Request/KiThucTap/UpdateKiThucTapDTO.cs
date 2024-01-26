using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace AmazingTech.InternSystem.Models.Request.KiThucTap
{
    public class UpdateKiThucTapDTO : IValidatableObject
    {
        public string Name { get; set; }
        public DateTime? NgayBatDau { get; set; }
        
        // [Compare(nameof(NgayBatDau), ErrorMessage = "NgayKetThuc phai lon hon hoac bang NgayBatDau")]
        public DateTime? NgayKetThuc { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NgayKetThuc is not null && NgayBatDau is not null && NgayKetThuc < NgayBatDau)
            {
                yield return new ValidationResult("NgayKetThuc phai lon hon hoac bang NgayBatDau", new[] { nameof(NgayKetThuc) });
            }
        }
    }
}