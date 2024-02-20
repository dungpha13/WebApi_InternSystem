using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Data;
using Microsoft.EntityFrameworkCore;
using AmazingTech.InternSystem.Models.DTO;
using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Office2010.ExcelAc;

namespace AmazingTech.InternSystem.Repositories
{
    public interface IInterviewRepo
    {
        Task<List<Cauhoi>> GetAllCauHoiAsync(string CongNghe);
        Task<int> AwserQuestion(string user, List<PhongVan> phongVan);

    }
    public class InterviewRepository : IInterviewRepo
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public InterviewRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<Cauhoi>> GetAllCauHoiAsync(string congnghe)
        {
            return await _context.cauhois.Where(x => x.CauhoiCongnghe.Where(c => c.IdCongNghe == congnghe && x.DeletedBy == null).Any()).ToListAsync();
        }



        public async Task<int> AwserQuestion(string user, List<PhongVan> phongVan)
        {
            var LichPhongvan = _context.LichPhongVans.Where(p => p.IdNguoiDuocPhongVan == user).FirstOrDefault();
            foreach (var phongvan in phongVan)
            {
                var check = _context.cauhoiCongnghes.Where(x => x.Id == phongvan.IdCauHoiCongNghe && x.DeletedBy == null).FirstOrDefault();
                if (check == null)
                {
                    throw new Exception();
                }
                phongvan.CreatedBy = user;
                phongvan.LastUpdatedBy = user;
                phongvan.IdLichPhongVan = LichPhongvan.Id;
                _context.phongVans.Add(phongvan);
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<List<ViewAnswer>> ViewAnwser(string idUser) 
        {
            List<Cauhoi> cauhois = _context.cauhois.Where(x =>x.DeletedBy != null ).ToList();
            List<CauhoiCongnghe> cauhoiCongNghes = _context.cauhoiCongnghes.Where(x => x.DeletedBy != null).ToList();
            List<PhongVan> phongVans = _context.phongVans.Where(x => x.CreatedBy == idUser ).ToList();
            List<ViewAnswer> viewAnswers = new List<ViewAnswer>();

            var query =  (from s in cauhois
                         join c in cauhoiCongNghes on s.Id equals c.IdCauhoi
                         join d in phongVans on c.Id equals d.IdCauHoiCongNghe 
                         select new
                         {
                             d.Id,
                             s.NoiDung,
                             d.CauTraLoi,
                             d.CreatedBy,                            
                         }).ToList();
            foreach (var viewAnswer in viewAnswers) 
            {
                foreach (var Query in query) 
                {
                    viewAnswer.Id = Query.Id;
                    viewAnswer.NoiDung = Query.NoiDung;
                    viewAnswer.CauTraLoi = Query.CauTraLoi;
                    viewAnswer.CreatedBy = Query.CreatedBy;                
                }
                viewAnswers.Add(viewAnswer);
            }
            return viewAnswers;
        }


        public async Task<int> RatingQuestion(string user, List<PhongVan> phongVan) 
        {
            var LichPhongvan = _context.LichPhongVans.Where(p => p.IdNguoiDuocPhongVan == user).FirstOrDefault();
            foreach (var phongvan in phongVan)
            {
                var check = _context.cauhoiCongnghes.Where(x => x.Id == phongvan.IdCauHoiCongNghe && x.DeletedBy == null).FirstOrDefault();
                if (check == null)
                {
                    throw new Exception();
                }
                phongvan.CreatedBy = user;
                phongvan.LastUpdatedBy = user;
                phongvan.IdLichPhongVan = LichPhongvan.Id;
                _context.phongVans.Add(phongvan);
            }

            return await _context.SaveChangesAsync();

        }


        /*       public async Task<int> AddListQuestionAsync(List<Cauhoi> cauhois, string user, string id)
               {
                   var existingKi = await _context.CongNghes
                       .Where(x => x.Id == id && x.DeletedBy == null)
                       .FirstOrDefaultAsync();

                   if (existingKi == null)
                   {
                       return 0;
                   }

                   foreach (var cauhoi in cauhois)
                   {
                       cauhoi.CreatedBy = user;
                       cauhoi.LastUpdatedBy = user;
                       // Add the question to the context
                       _context.cauhois.Add(cauhoi);

                       // Create a relationship between the question and the technology
                       CauhoiCongnghe cauhoiconghe = new CauhoiCongnghe()
                       {
                           IdCauhoi = cauhoi.Id,
                           IdCongNghe = existingKi.Id,
                           CreatedBy = user,
                           LastUpdatedBy = user,
                       };
                       _context.cauhoiCongnghes.Add(cauhoiconghe);

                   }

                   return await _context.SaveChangesAsync();
               }*/
    }
}
