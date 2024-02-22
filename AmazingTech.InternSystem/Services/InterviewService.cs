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
        Task<IActionResult> RatingAnwser(string user, List<RatingModel> Rating);
        Task<IActionResult> UpdateRating(string user, List<RatingModel> Rating);
        Task<IActionResult> DeleteInterView(string user, string UserId);
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
            if (check == 2) { return new BadRequestObjectResult("Your interview schedule is not yet available"); }
            if (check == 0) { return new BadRequestObjectResult("You have completed this interview"); }
            return new OkObjectResult("Thank you for participating in the interview at our company ");
       }

        public async Task<List<ViewAnswer>> ViewAnswers(string user)
        {
           
            return await _interviewRepo.ViewAnwser(user);
        }

        public async Task<IActionResult> RatingAnwser(string user, List<RatingModel> Rating)
        {
            List<PhongVan> traloi = _mapper.Map<List<PhongVan>>(Rating);
            return await _interviewRepo.RatingQuestion(user, traloi);
        }
        public async Task<IActionResult> UpdateRating(string user, List<RatingModel> Rating)
        {
            List<PhongVan> traloi = _mapper.Map<List<PhongVan>>(Rating);
            return await _interviewRepo.UpdateRating(user, traloi);
        }

        public async Task<IActionResult> DeleteInterView(string user, string UserId)
        {
            return await _interviewRepo.DeleteRating(user, UserId);
        }

    }
}
