using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Models.DTO;
using AmazingTech.InternSystem.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Services
{


    public interface IInterviewService
    {
        Task<List<ViewQuestionInterview>> getAllQuestion(string idCongNghe);
        Task<IActionResult> AwserQuestion(string user, List<AwserQuest> cauHoi);
        Task<List<ViewAnswer>> ViewAnswers(string user);

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

         public async Task<List<ViewQuestionInterview>> getAllQuestion(string idCongNghe)
         {
            List<CauhoiCongnghe> cauhoi = await _interviewRepo.GetAllCauHoiAsync(idCongNghe);
            List<ViewQuestionInterview> result = _mapper.Map<List<ViewQuestionInterview>>(cauhoi);
            return result;

         }

        public async Task<IActionResult> AwserQuestion(string user, List<AwserQuest> cauHoi)
        {
            List<PhongVan> traloi = _mapper.Map<List<PhongVan>>(cauHoi);
            var check = await _interviewRepo.AwserQuestion(user, traloi);
            if (check == 0) { return new BadRequestObjectResult("You have completed this interview"); }
            return new OkObjectResult("Thank you for participating in the interview at our company ");
       }

        public async Task<List<ViewAnswer>> ViewAnswers(string user)
        {
           
            return await _interviewRepo.ViewAnwser(user);
        }

/*        public async Task<int> DeleteQuestion(string user, string id, string cauhoiId)
        {
            return await _QuestionRepo.DeleteQuestionAsync(user, id, cauhoiId);
        }*/

    }
}
