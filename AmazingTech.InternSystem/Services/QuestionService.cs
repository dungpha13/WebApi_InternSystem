using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.DTO;
using AmazingTech.InternSystem.Repositories;
using AmazingTech.InternSystem.Repositories.AmazingTech.InternSystem.Repositories;
using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace AmazingTech.InternSystem.Services
{
    public interface IQuestionService
    {
        Task<List<QuestionDTO>> getAllQuestion(string idCongNghe);
        Task<int> CreateQuestion(string user, string congngheId, QuestionDTO cauHoi);
        Task<int> UpdateQuestion(string user, string id, string cauhoiId, QuestionDTO CauHoi);
        Task<int> DeleteQuestion(string user, string id, string cauhoiId);

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

    }
}
