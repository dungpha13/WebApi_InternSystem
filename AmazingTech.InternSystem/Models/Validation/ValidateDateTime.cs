using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AmazingTech.InternSystem.Models.Validation
{
    
    public class ValidateDateTime : ValidationAttribute
    {
        public ValidateDateTime()
        {
            ErrorMessage = "";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string dateStr)
            {
                // Kiểm tra ngày hợp lệ và đúng format hay chưa
                var parsedNgayMacDinh = ParseDateString(dateStr);
                if (parsedNgayMacDinh.HasValue)
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                // Nếu giá trị không phải là chuỗi, trả về thông báo lỗi
                return new ValidationResult(ErrorMessage);
            }

            // Nếu giá trị là chuỗi nhưng không hợp lệ, trả về thông báo lỗi
            return new ValidationResult(ErrorMessage);
        }


        //Hàm kiểm tra định dạng ngày nhập vào theo format và DateTime input phải là một ngày hợp lệ
        private DateTime? ParseDateString(string dateString)
        {
            // Thử chuyển đổi theo định dạng "dd/MM/yyyy"
            if (DateTime.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out var date))
            {
                return date;
            }

            // Nếu không thành công, thử chuyển đổi theo định dạng "yyyy-MM-dd"
            if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date2))
            {
                return date2;
            }

            // Nếu cả hai đều không thành công, trả về null
            return null;
        }
    }
}
