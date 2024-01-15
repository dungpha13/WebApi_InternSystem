using System.Text;
using AmazingTech.InternSystem.Data.Entity;
using OfficeOpenXml;

namespace AmazingTech.InternSystem.Services
{
    public class FileReaderService : IFileReaderService
    {
        public List<InternInfo>? ReadFile(IFormFile file)
        {
            try
            {
                List<InternInfo> list = new List<InternInfo>();
                using (var stream = new MemoryStream())
                {
                    file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];

                        int rowCount = worksheet.Dimension.Rows;
                        int colCount = worksheet.Dimension.Columns;

                        StringBuilder result = new StringBuilder();
                        for (int row = 2; row <= rowCount; row++)
                        {
                            DateTime parsedDate = new DateTime();
                            DateTime.TryParseExact(worksheet.Cells[row, 3].Value.ToString()!, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out parsedDate);
                            InternInfo intern = new InternInfo
                            {
                                HoTen = worksheet.Cells[row, 2].Value.ToString()!,
                                NgaySinh = parsedDate,
                                GioiTinh = bool.Parse(worksheet.Cells[row, 4].Value.ToString()!),
                                MSSV = worksheet.Cells[row, 5].Value.ToString()!,
                                EmailTruong = worksheet.Cells[row, 6].Value.ToString()!,
                                EmailCaNhan = worksheet.Cells[row, 7].Value.ToString()!,
                                SdtNguoiThan = worksheet.Cells[row, 8].Value.ToString()!,
                                DiaChi = worksheet.Cells[row, 9].Value.ToString()!,
                                GPA = decimal.Parse(worksheet.Cells[row, 10].Value.ToString()!),
                                TrinhDoTiengAnh = worksheet.Cells[row, 11].Value.ToString()!,
                                ChungChi = worksheet.Cells[row, 12].Value.ToString()!,
                                LinkFacebook = worksheet.Cells[row, 13].Value.ToString()!,
                                LinkCV = worksheet.Cells[row, 14].Value.ToString()!,
                                NganhHoc = worksheet.Cells[row, 15].Value.ToString()!,
                                Sdt = worksheet.Cells[row, 16].Value.ToString()!,
                                Round = 0,
                                Status = "true",
                                // KiThucTapId = worksheet.Cells[row, 15].Value.ToString()!,
                                StartDate = DateTime.Now,
                                EndDate = DateTime.Now,
                            };

                            list.Add(intern);
                        }
                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return null;
            }
        }
    }
}