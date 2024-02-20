using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.DTO;
using AmazingTech.InternSystem.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Collections.Generic;

namespace AmazingTech.InternSystem.Services
{
    public interface IQuestionService
    {
        Task<List<QuestionDTO>> getAllQuestion(string idCongNghe);
        Task<int> CreateQuestion(string user, string congngheId, QuestionDTO cauHoi);
        Task<int> UpdateQuestion(string user, string id, string cauhoiId, QuestionDTO CauHoi);
        Task<int> DeleteQuestion(string user, string id, string cauhoiId);
        Task<IActionResult> AddListQuest(IFormFile file, string CongNgheId, string UserId);

    }
    public class QuestionService: IQuestionService
    {
        private IQuestionRepository _QuestionRepo;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public QuestionService(IServiceProvider serviceProvider, IMapper mapper)
        {
            _QuestionRepo = serviceProvider.GetRequiredService<IQuestionRepository>();
            _mapper = mapper;
        }

        public async Task<List<QuestionDTO>> getAllQuestion(string idCongNghe)
        {
            List<Cauhoi> cauhoi = await _QuestionRepo.GetAllCauHoiAsync(idCongNghe);
            List<QuestionDTO> result = _mapper.Map<List<QuestionDTO>>(cauhoi);
           return result;

        }

        public async Task<int> CreateQuestion(string user, string congngheId, QuestionDTO cauHoi)
        {          
            Cauhoi cauhoi = _mapper.Map<Cauhoi>(cauHoi);
            return await _QuestionRepo.CreateCauHoiAsync(user, congngheId, cauhoi);
        }

        public async Task<int> UpdateQuestion(string user, string id, string cauhoiId, QuestionDTO CauHoi)
        {
            Cauhoi cauhoi = _mapper.Map<Cauhoi>(CauHoi);
            return await _QuestionRepo.UpdateQuestionAsync(user, id, cauhoiId, cauhoi);
        }

        public async Task<int> DeleteQuestion(string user, string id, string cauhoiId)
        {
            return await _QuestionRepo.DeleteQuestionAsync(user, id, cauhoiId);
        }

        private List<Cauhoi> ReadFile(IFormFile file)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyToAsync(stream);

                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];

                        var startRow = worksheet.Dimension.Start.Row;
                        var startCol = worksheet.Dimension.Start.Column;
                        var endRow = worksheet.Dimension.End.Row;
                        var endCol = worksheet.Dimension.End.Column;

                        var range = worksheet.Cells[startRow,startCol,endRow,endCol];


                        List<Cauhoi> QuesionList = range.ToCollectionWithMappings<Cauhoi>(row =>
                        {
                            var cauhoi = new Cauhoi
                            {
                                NoiDung = row.GetText("Câu hỏi"),
                            };

                            return cauhoi;
                        }, options => options.HeaderRow = 0);

                        return QuesionList;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<Cauhoi>();
            }
        }

        public async Task<IActionResult> AddListQuest(IFormFile file, string UserId, string CongNgheId)
        {
            List<Cauhoi> cauhoi = ReadFile(file);            

            foreach (var Cauhoi in cauhoi)
            {
                var cauHoi = new Cauhoi
                {
                    NoiDung = Cauhoi.NoiDung,
                };
            }

            var result = await _QuestionRepo.AddListQuestionAsync(cauhoi, UserId, CongNgheId);

            if (result == 0)
            {
                return new BadRequestObjectResult("Something went wrong!");
            }

            return new OkObjectResult("Success");
        }
    }
}
