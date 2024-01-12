// using AmazingTech.InternSystem.Data.Entity;
// using AmazingTech.InternSystem.Repositories;
// using Microsoft.AspNetCore.Mvc;
// using OfficeOpenXml;

// namespace AmazingTech.InternSystem.Services
// {
//     public class FileReaderService : IFileReaderService
//     {
//         private readonly IInternRepository _repository;

//         public FileReaderService(IInternRepository repository)
//         {
//             _repository = repository;
//         }

//         public IActionResult ReadFile(IFormFile file, string kiThucTapId)
//         {
//             _repository.AddListIntern(ReadFile(file), kiThucTapId);
//             return new OkResult();
//         }

//         public List<InternInfo> ReadFile(IFormFile file)
//         {
//             try
//             {
//                 using (var stream = new MemoryStream())
//                 {
//                     file.CopyToAsync(stream);

//                     using (var package = new ExcelPackage(stream))
//                     {
//                         var worksheet = package.Workbook.Worksheets[0];

//                         // var start = worksheet.Dimension.Start;
//                         // var end = worksheet.Dimension.End;

//                         IEnumerable<InternInfo> exportedPersons = worksheet.Cells["A1:O21"].ToCollectionWithMappings<InternInfo>(row =>
//                         {
//                             var intern = new InternInfo
//                             {
//                                 HoTen = row.GetText("HoVaTen"),
//                                 NgaySinh = DateTime.FromOADate(row.GetValue<double>("NgaySinh")),
//                                 GioiTinh = row.GetText("GioiTinh") == "Nam" ? true : false,
//                                 MSSV = row.GetText("MSSV"),
//                                 EmailTruong = row.GetText("EmailTruong"),
//                                 EmailCaNhan = row.GetText("EmailCaNhan"),
//                                 Sdt = row.GetText("SDT"),
//                                 DiaChi = row.GetText("DiaChi"),
//                                 SdtNguoiThan = row.GetText("SdtNguoiThan"),
//                                 GPA = row.GetValue<decimal>("GPA"),
//                                 TrinhDoTiengAnh = row.GetText("TrinhDoTiengAnh"),
//                                 NganhHoc = row.GetText("NganhHoc"),
//                                 ChungChi = "a",
//                                 LinkFacebook = row.GetText("LinkFacebook"),
//                                 LinkCV = row.GetText("LinkCV"),
//                                 Round = 0,
//                                 Status = "true",
//                             };

//                             return intern;
//                         }, options => options.HeaderRow = 0);

//                         return exportedPersons.ToList();
//                     }
//                 }
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine(ex);
//                 return new List<InternInfo>();
//             }
//         }

//     }
// }
