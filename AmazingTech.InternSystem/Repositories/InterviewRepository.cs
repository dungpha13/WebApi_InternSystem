using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Data;
using Microsoft.EntityFrameworkCore;
using AmazingTech.InternSystem.Models.DTO;
using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Repositories
{
    public interface IInterviewRepo
    {
        Task<List<CauhoiCongnghe>> GetAllCauHoiAsync(string CongNghe);
        Task<int> AwserQuestion(string user, List<PhongVan> phongVan);
        Task<List<ViewAnswer>> ViewAnwser(string idUser);
        Task<IActionResult> RatingQuestion(string user, List<PhongVan> phongVan);
        Task<IActionResult> UpdateRating(string user, List<PhongVan> phongVan);
        Task<IActionResult> DeleteRating(string user, string UserID);
    }
    public class InterviewRepository : IInterviewRepo
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public InterviewRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<CauhoiCongnghe>> GetAllCauHoiAsync(string congnghe)
        {
            return await _context.cauhoiCongnghes.Where(c => c.IdCongNghe == congnghe && c.DeletedBy == null).Include(x => x.cauhoi).ToListAsync();
        }



        public async Task<int> AwserQuestion(string user, List<PhongVan> phongVan)
        {
            var LichPhongvan = _context.LichPhongVans.Where(p => p.IdNguoiDuocPhongVan == user).FirstOrDefault();
            if(LichPhongvan == null) { return 2; }
            foreach (var phongvan in phongVan)
            {
                var check = _context.cauhoiCongnghes.Where(x => x.Id == phongvan.IdCauHoiCongNghe && x.DeletedBy == null).FirstOrDefault();
                if (check == null)
                {
                    throw new Exception();
                }
                var checkDuplicate = _context.phongVans.Where(x => x.CreatedBy == user && x.IdCauHoiCongNghe == phongvan.IdCauHoiCongNghe).FirstOrDefault();
                if (checkDuplicate != null) { return 0; }
                phongvan.CreatedBy = user;
                phongvan.LastUpdatedBy = user;
                phongvan.IdLichPhongVan = LichPhongvan.Id;
                
                _context.phongVans.Add(phongvan);
            }
           
            await _context.SaveChangesAsync();
            return 1; 
        }

        public async Task<List<ViewAnswer>> ViewAnwser(string idUser) 
        {
            List<Cauhoi> cauhois = _context.cauhois.Where(x =>x.DeletedBy == null ).ToList();
            List<CauhoiCongnghe> cauhoiCongNghes = _context.cauhoiCongnghes.Where(x => x.DeletedBy == null).ToList();
            List<PhongVan> phongVans = _context.phongVans.Where(x => x.CreatedBy == idUser &&  x.DeletedBy == null).ToList();
            List<ViewAnswer> viewAnswers = new List<ViewAnswer>();

            var query =  (from s in cauhois
                         join c in cauhoiCongNghes on s.Id equals c.IdCauhoi
                         join d in phongVans on c.Id equals d.IdCauHoiCongNghe 
                         select new
                         {
                             d.Id,
                             s.NoiDung,
                             d.CauTraLoi,
                             d.Rank,
                             d.CreatedBy,                            
                         }).ToList();
   
            foreach (var Query in query) 
            {
              var viewAnswer = new ViewAnswer 
              {
                  Id = Query.Id,
                  CauTraLoi = Query.CauTraLoi,
                  CreatedBy = Query.CreatedBy,
                  Rank = Query.Rank,
                  NoiDung = Query.NoiDung
              };

                viewAnswers.Add(viewAnswer);
            }
            return viewAnswers;
        }
           
        


        public async Task<IActionResult> RatingQuestion(string user, List<PhongVan> phongVan) 
        {
            foreach (var phongvan in phongVan)
            {
                var check = _context.phongVans.Where(x => x.Id == phongvan.Id && x.DeletedBy == null).FirstOrDefault();
                if (check == null)
                {
                    throw new Exception();
                }
                var checkRating = _context.phongVans.Where(x => x.Id == phongvan.Id && x.NguoiCham != null && x.DeletedBy == null).FirstOrDefault();
                if (checkRating != null) { return new BadRequestObjectResult($"Interns were graded by {checkRating.NguoiCham}" ); }
               check.NguoiCham = user;
               check.Rank = phongvan.Rank;
                check.RankDate = DateTime.Now;            
                
            }
           await _context.SaveChangesAsync();

            return new OkObjectResult("Success!");
        }

        public async Task<IActionResult> UpdateRating(string user, List<PhongVan> phongVan)
        {
            foreach (var phongvan in phongVan)
            {
                var check = _context.phongVans.Where(x => x.Id == phongvan.Id && x.DeletedBy == null).FirstOrDefault();
                if (check == null)
                {
                    throw new Exception();
                }               
                check.NguoiCham = user;
                check.Rank = phongvan.Rank;
                check.RankDate = DateTime.Now;

            }
            await _context.SaveChangesAsync();

            return new OkObjectResult("Success!");
        }
        public async Task<IActionResult> DeleteRating(string user, string UserID)
        {
            var phongVan = _context.phongVans.Where(x => x.CreatedBy == UserID).ToList();
            foreach (var phongvan in phongVan)
            {
                var check = _context.phongVans.Where(x => x.Id == phongvan.Id && x.DeletedBy == null).FirstOrDefault();
                if (check == null)
                {
                    throw new Exception();
                }
                check.DeletedBy = user;
                check.DeletedTime = DateTime.Now;

            }
            await _context.SaveChangesAsync();

            return new OkObjectResult("Success!");
        }


    }
}
