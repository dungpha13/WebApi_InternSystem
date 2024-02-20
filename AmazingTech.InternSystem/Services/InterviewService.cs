using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Models.DTO;
using AmazingTech.InternSystem.Repositories;
using AutoMapper;

namespace AmazingTech.InternSystem.Services
{


    public interface IInterviewService
        {
        Task<List<QuestionDTO>> getAllQuestion(string idCongNghe);
        Task<int> AwserQuestion(string user, List<AwserQuest> cauHoi);

        }
       public class InterviewService : IInterviewService
       {
            private IInterviewRepo _interviewRepo;
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;

         public InterviewService(IServiceProvider serviceProvider, IMapper mapper)
         {
              _interviewRepo = serviceProvider.GetRequiredService<IInterviewRepo>();
            _mapper = mapper;
         }

         public async Task<List<QuestionDTO>> getAllQuestion(string idCongNghe)
         {
            List<Cauhoi> cauhoi = await _interviewRepo.GetAllCauHoiAsync(idCongNghe);
            List<QuestionDTO> result = _mapper.Map<List<QuestionDTO>>(cauhoi);
            return result;

         }

       public async Task<int> AwserQuestion(string user, List<AwserQuest> cauHoi)
       {
            List<PhongVan> traloi = _mapper.Map<List<PhongVan>>(cauHoi);
            return await _interviewRepo.AwserQuestion(user, traloi);
       }
/*
        public async Task<int> UpdateQuestion(string user, string id, string cauhoiId, QuestionDTO CauHoi)
        {
            Cauhoi cauhoi = _mapper.Map<Cauhoi>(CauHoi);
            return await _QuestionRepo.UpdateQuestionAsync(user, id, cauhoiId, cauhoi);
        }

        public async Task<int> DeleteQuestion(string user, string id, string cauhoiId)
        {
            return await _QuestionRepo.DeleteQuestionAsync(user, id, cauhoiId);
        }*/

    }
}
