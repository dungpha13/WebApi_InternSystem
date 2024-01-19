using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models;
using AmazingTech.InternSystem.Repositories.AmazingTech.InternSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Packaging.Ionic.Zip;



namespace AmazingTech.InternSystem.Repositories
{


    namespace AmazingTech.InternSystem.Repositories
    {
        public interface ITechRepo
        {
            Task<List<CongNghe>> GetAllCongNgheAsync();
            Task<int> CreateCongNgheAsync(string user, CongNghe CongNgheModel);

            Task<int> UpdateCongNgheAsync(string user, string congNgheId, CongNghe updatedCongNghe);

            Task<int> DeleteCongNgheAsync(string user, string congNgheId);


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

        public async Task<int> CreateCongNgheAsync(string user, CongNghe CongNgheModel)
        {
            var check = _context.CongNghes.Where(x => x.Ten == CongNgheModel.Ten && x.DeletedBy == null).FirstOrDefault();
            if (check != null) { throw new Exception("Tech has been existed"); }
            CongNgheModel.CreatedBy = user;
            CongNgheModel.LastUpdatedBy = user;
            CongNgheModel.LastUpdatedTime = DateTime.Now;
            _context.CongNghes.Add(CongNgheModel);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateCongNgheAsync(string user, string congNgheId, CongNghe updatedCongNghe)
        {
           
           var existingCongNghe = await _context.CongNghes.FirstOrDefaultAsync(c => c.Id == congNgheId);
           if (existingCongNghe != null) {  return 0;}           
           if (updatedCongNghe.Ten != null) existingCongNghe.Ten = updatedCongNghe.Ten;
           var check = _context.CongNghes.Where(x => x.Ten == existingCongNghe.Ten && x.DeletedBy == null).FirstOrDefault();
           if (check != null) { throw new Exception("Tech has been existed"); }
           if (updatedCongNghe.IdViTri != null) existingCongNghe.IdViTri = updatedCongNghe.IdViTri;
           if (updatedCongNghe.ImgUrl != null) existingCongNghe.ImgUrl = updatedCongNghe.ImgUrl;
            existingCongNghe.LastUpdatedBy = user;
            existingCongNghe.LastUpdatedTime = DateTime.Now;
            return await _context.SaveChangesAsync();
            
        }

        public async Task<int> DeleteCongNgheAsync(string user,  string congNgheId )
        {
            var congNgheToDelete = await _context.CongNghes.FirstOrDefaultAsync(c => c.Id == congNgheId && c.DeletedBy == null);
            if (congNgheToDelete == null) { return 0; }
            
             congNgheToDelete.DeletedBy = user;
             congNgheToDelete.DeletedTime = DateTime.Now;
             return await _context.SaveChangesAsync();
            
            
        }

  }
         
}