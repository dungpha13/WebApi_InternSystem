using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models;
using AmazingTech.InternSystem.Repositories.AmazingTech.InternSystem.Repositories;
using Microsoft.EntityFrameworkCore;



namespace AmazingTech.InternSystem.Repositories
{


    namespace AmazingTech.InternSystem.Repositories
    {
        public interface ITechRepo
        {
            Task<List<CongNghe>> GetAllCongNgheAsync();
            Task CreateCongNgheAsync(string user, CongNghe CongNgheModel);

            Task UpdateCongNgheAsync(string user, string congNgheId, CongNghe updatedCongNghe);

            Task DeleteCongNgheAsync(string congNgheId, string user);


        }
    }
    public class TechRepository : ITechRepo
    {
        private readonly AppDbContext _context;

        public TechRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CongNghe>> GetAllCongNgheAsync()
        {
            return await _context.CongNghes.Where(x => x.DeletedBy == null).ToListAsync();
           
        }

        public async Task<CongNghe> GetCongNgheByIdAsync(string congNgheId)
        {
            var congNghe = await _context.CongNghes
                .Include(c => c.ViTri)
                .FirstOrDefaultAsync(c => c.Id == congNgheId);

            return congNghe;
        }

        public async Task CreateCongNgheAsync(string user, CongNghe CongNgheModel)
        {
            CongNgheModel.CreatedBy = user;
            CongNgheModel.LastUpdatedBy = user;
            CongNgheModel.LastUpdatedTime = DateTime.Now;
            _context.CongNghes.Add(CongNgheModel);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCongNgheAsync(string user, string congNgheId, CongNghe updatedCongNghe)
        {
            var existingCongNghe = await _context.CongNghes.FirstOrDefaultAsync(c => c.Id == congNgheId);
           

            if (existingCongNghe != null)
            {
                if (updatedCongNghe.Ten != null) existingCongNghe.Ten = updatedCongNghe.Ten;
                if (updatedCongNghe.IdViTri != null) existingCongNghe.IdViTri = updatedCongNghe.IdViTri;
                if (updatedCongNghe.ImgUrl != null) existingCongNghe.ImgUrl = updatedCongNghe.ImgUrl;
                existingCongNghe.LastUpdatedBy = user;
                existingCongNghe.LastUpdatedTime = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCongNgheAsync(string user,  string congNgheId )
        {
            var congNgheToDelete = await _context.CongNghes.FirstOrDefaultAsync(c => c.Id == congNgheId);

            if (congNgheToDelete != null)
            {
                congNgheToDelete.DeletedBy = user;
                congNgheToDelete.DeletedTime = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            
        }

  }
         
}