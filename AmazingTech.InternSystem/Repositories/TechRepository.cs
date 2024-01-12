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
            Task<CongNghe> GetCongNgheByIdAsync(string congNgheId);
            Task CreateCongNgheAsync(CongNghe CongNgheModel);
            Task UpdateCongNgheAsync(string congNgheId, CongNghe updatedCongNghe);

            Task DeleteCongNgheAsync(string congNgheId, CongNghe techdelete);
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

        public async Task CreateCongNgheAsync(CongNghe CongNgheModel)
        {
            var congNgheEntity = new CongNghe
            {
                Id = Guid.NewGuid().ToString("N"),
                Ten = CongNgheModel.Ten,
                IdViTri = CongNgheModel.IdViTri,
                ImgUrl = CongNgheModel.ImgUrl,
                CreatedBy = CongNgheModel.CreatedBy,
                LastUpdatedBy = CongNgheModel.CreatedBy,
        };         
            _context.CongNghes.Add(congNgheEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCongNgheAsync(string congNgheId, CongNghe updatedCongNghe)
        {
            var existingCongNghe = await _context.CongNghes.FirstOrDefaultAsync(c => c.Id == congNgheId);
           

            if (existingCongNghe != null)
            {
                if (updatedCongNghe.Ten != null) existingCongNghe.Ten = updatedCongNghe.Ten;
                if (updatedCongNghe.IdViTri != null) existingCongNghe.IdViTri = updatedCongNghe.IdViTri;
                if (updatedCongNghe.ImgUrl != null) existingCongNghe.ImgUrl = updatedCongNghe.ImgUrl;
                existingCongNghe.LastUpdatedBy = updatedCongNghe.LastUpdatedBy;                
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCongNgheAsync(string congNgheId, CongNghe techdelete)
        {
            var congNgheToDelete = await _context.CongNghes.FirstOrDefaultAsync(c => c.Id == congNgheId);

            if (congNgheToDelete != null)
            {
                congNgheToDelete.DeletedBy = techdelete.DeletedBy;
                congNgheToDelete.DeletedTime = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            
        }

  }
         
}