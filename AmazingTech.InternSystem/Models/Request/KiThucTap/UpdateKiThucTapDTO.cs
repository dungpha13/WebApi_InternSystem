using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace AmazingTech.InternSystem.Models.Request.KiThucTap
{
    public class UpdateKiThucTapDTO : IValidatableObject
    {
        [Required(ErrorMessage = "Ten ki thuc tap khong duoc de trong")]
        public string Name { get; set; }
        [Required(ErrorMessage = "NgayBatDau khong duoc de trong")]
        public DateTime? NgayBatDau { get; set; }

        // [Compare(nameof(NgayBatDau), ErrorMessage = "NgayKetThuc phai lon hon hoac bang NgayBatDau")]
        [Required(ErrorMessage = "NgayKetThuc khong duoc de trong")]
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