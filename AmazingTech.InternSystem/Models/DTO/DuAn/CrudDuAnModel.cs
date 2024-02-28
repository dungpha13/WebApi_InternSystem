using System.ComponentModel.DataAnnotations;

namespace AmazingTech.InternSystem.Models.DTO.DuAn
{
    public class CrudDuAnModel : IValidatableObject
    {
        public string? Ten { get; set; }
        public string LeaderId { get; set; }

        [Required(ErrorMessage = "ThoiGianBatDau la bat buoc")]
        [DataType(DataType.DateTime, ErrorMessage = "Sai kieu du lieu")]
        public DateTime? ThoiGianBatDau { get; set; }

        [Required(ErrorMessage = "ThoiGianKetThuc la bat buoc")]
        [DataType(DataType.DateTime, ErrorMessage = "Sai kieu du lieu")]
        public DateTime? ThoiGianKetThuc { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ThoiGianKetThuc < ThoiGianBatDau)
            {
                yield return new ValidationResult("ThoiGianKetThuc phai lon hon hoac bang ThoiGianBatDau", new[] { nameof(ThoiGianKetThuc) });
            }
        }
    }
}
