using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.DTO;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmazingTech.InternSystem.Repositories
{
    public interface IQuestionRepository
    {
        Task<List<Cauhoi>> GetAllCauHoiAsync(string CongNgheID);
        Task<int> CreateCauHoiAsync(string user, string congngheId, Cauhoi cauhoi);
        Task<int> UpdateQuestionAsync(string user, string congNgheId, string cauhoiId, Cauhoi cauhoi);
        Task<int> DeleteQuestionAsync(string user, string congNgheId, string cauhoiId);
        Task<int> AddListQuestionAsync(List<Cauhoi> cauhois, string user, string id);
        Task<Cauhoi> GetCauHoiByIdAsync(string congNgheId, string id);
    }
    public class QuestionRepository : IQuestionRepository
    {
        private readonly AppDbContext _context;

        public QuestionRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<Cauhoi>> GetAllCauHoiAsync(string CongNgheID)
        {
            var checkCongNghe = _context.CongNghes.Where(x => x.Id == CongNgheID && x.DeletedBy == null).FirstOrDefault();
            if (checkCongNghe == null) { throw new Exception(); }
            return await _context.cauhois.Where(x => x.CauhoiCongnghe.Where(d => d.IdCongNghe == CongNgheID).Any() && x.DeletedBy == null).ToListAsync();
        }

        public async Task<int> CreateCauHoiAsync(string user, string congngheId, Cauhoi cauhoi)
        {
            var congngheExist = _context.CongNghes.Where(x => x.Id == congngheId && x.DeletedBy == null).FirstOrDefault();
            if (congngheExist == null) { return 0; }
            cauhoi.CreatedBy = user;
            cauhoi.LastUpdatedBy = user;
            cauhoi.LastUpdatedTime = DateTime.Now;
            _context.cauhois.Add(cauhoi);
           CauhoiCongnghe cauhoiCongNghe = new CauhoiCongnghe()
            {
                IdCauhoi = cauhoi.Id,
                IdCongNghe = congngheId,
                CreatedBy = user,
                LastUpdatedBy = user,
            };
            _context.cauhoiCongnghes.Add(cauhoiCongNghe);         
            await _context.SaveChangesAsync();
            return 1;
        }


        public async Task<Cauhoi> GetCauHoiByIdAsync( string congNgheId, string id)
        {
            var checkCongNghe = _context.CongNghes.Where(x => x.Id == congNgheId && x.DeletedBy == null).FirstOrDefault();
            if(checkCongNghe == null) { throw new Exception(); }             
            var Cauhoi = _context.cauhois.Where(x => x.CauhoiCongnghe.Where(d => d.IdCongNghe == congNgheId && d.IdCauhoi == id).Any() && x.DeletedBy == null).FirstOrDefault();
            return Cauhoi;
        }

        public async Task<int> UpdateQuestionAsync(string user, string congNgheId, string cauhoiId , Cauhoi cauhoi)
        {

            var existingCongNghe = await _context.CongNghes.FirstOrDefaultAsync(c => c.Id == congNgheId && c.DeletedBy == null);
            if (existingCongNghe == null) { return 0; }
            var check = _context.cauhois.Where(x => x.Id == cauhoiId && x.DeletedBy == null).FirstOrDefault();
            if (check == null) { return 0; }
            var cauhoiCongnghe = _context.cauhoiCongnghes.Where(x => x.IdCongNghe == congNgheId && x.IdCauhoi == cauhoiId).FirstOrDefault();
            check.NoiDung = cauhoi.NoiDung;
            existingCongNghe.LastUpdatedBy = user;
            existingCongNghe.LastUpdatedTime = DateTime.Now;
            cauhoiCongnghe.LastUpdatedBy = user;
            cauhoiCongnghe.LastUpdatedTime = DateTime.Now;
            await _context.SaveChangesAsync();
            return 1;

        }

        public async Task<int> DeleteQuestionAsync(string user, string congNgheId, string cauhoiId)
        {
            var congNgheToDelete = await _context.CongNghes.FirstOrDefaultAsync(c => c.Id == congNgheId && c.DeletedBy == null);
            if (congNgheToDelete == null) { return 0; }
            var check = _context.cauhois.Where(x => x.Id == cauhoiId && x.DeletedBy == null).FirstOrDefault();
            if (check == null) { return 0; }
            var cauhoiCongnghe = _context.cauhoiCongnghes.Where(x => x.IdCongNghe == congNgheId && x.IdCauhoi == cauhoiId).FirstOrDefault();
            check.DeletedBy = user;
            check.DeletedTime = DateTime.Now;
            cauhoiCongnghe.DeletedBy = user;
            cauhoiCongnghe.DeletedTime = DateTime.Now;
            await _context.SaveChangesAsync();
            return 1;

        }

        public async Task<int> AddListQuestionAsync(List<Cauhoi> cauhois, string user, string id)
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
        }


    }
}
