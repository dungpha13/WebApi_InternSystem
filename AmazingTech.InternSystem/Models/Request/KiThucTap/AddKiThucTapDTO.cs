using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.Request.KiThucTap
{
    public class AddKiThucTapDTO : IValidatableObject
    {
        [Required]
        [StringLength(50, ErrorMessage = "Ten khong duoc vuot qua 50 ki tu")]
        public string Name { get; set; }

        [Required(ErrorMessage = "NgayBatDau la bat buoc")]
        [DataType(DataType.DateTime, ErrorMessage = "Sai kieu du lieu")]
        public DateTime NgayBatDau { get; set; }

        [Required(ErrorMessage = "NgayKetThuc la bat buoc")]
        [DataType(DataType.DateTime, ErrorMessage = "Sai kieu du lieu")]
        // [Compare(nameof(NgayBatDau), ErrorMessage = "NgayKetThuc phai lon hon hoac bang NgayBatDau")]
        public DateTime NgayKetThuc { get; set; }

        [Required(ErrorMessage = "IdTruong la bat buoc")]
        public string IdTruong { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NgayKetThuc < NgayBatDau)
            {
                yield return new ValidationResult("NgayKetThuc phai lon hon hoac bang NgayBatDau", new[] { nameof(NgayKetThuc) });
            }
        }
    }
}