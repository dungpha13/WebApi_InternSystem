using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace AmazingTech.InternSystem.Models.Validation
{
    public class CompareDateTime : ValidationAttribute
    {
        private readonly string _thamSoTruyenVao;

        public CompareDateTime(string comparisonProperty)
        {
            _thamSoTruyenVao = comparisonProperty;
            ErrorMessage = "";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string dateStr)
            {
                //Kiểm tra ngày hợp lệ và đúng format hay chưa
                var parsedNgayDuocValid = ParseDateString(dateStr);

                if (parsedNgayDuocValid.HasValue)
                {
                    //[CompareDateTime("StartDate")]  là nó sẽ lấy cái string StartDate vào đây
                    var thamSoTruyenVaoInfo = validationContext.ObjectType.GetProperty(_thamSoTruyenVao);

                    if (thamSoTruyenVaoInfo == null)
                    {
                        return new ValidationResult($"{_thamSoTruyenVao} không được là null!");
                    }

                    var NgayDuocSoSanh = thamSoTruyenVaoInfo.GetValue(validationContext.ObjectInstance);

                    if (NgayDuocSoSanh is string chuoiDuocSoSanh)
                    {
                        // Thực hiện chuyển đổi từ chuỗi sang DateTime
                        var parsedNgayTruyenVao = ParseDateString(chuoiDuocSoSanh);


                        if (parsedNgayTruyenVao.HasValue && parsedNgayTruyenVao.Value > parsedNgayDuocValid.Value)
                        {
                            
                            return new ValidationResult($"{validationContext.DisplayName} không được xảy ra trước {_thamSoTruyenVao}.");
                        }
                    }

                    return ValidationResult.Success!;
                }
                else
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success!;
        }


        //Hàm kiểm tra định dạng ngày nhập vào theo format và DateTime input phải là một ngày hợp lệ
        private DateTime? ParseDateString(string dateString)
        {
            //// Thử chuyển đổi theo định dạng "dd/MM/yyyy"
            //if (DateTime.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            //{
            //    return date;
            //}

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
