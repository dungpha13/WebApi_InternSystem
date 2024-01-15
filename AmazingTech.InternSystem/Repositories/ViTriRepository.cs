using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace AmazingTech.InternSystem.Repo
{
    public class ViTriRepository : IViTriRepository
    {
        private readonly AppDbContext _context;

        public ViTriRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateViTriAsync(ViTri viTriModel)
        {
            var vitriEntity = new ViTri {
                Id = Guid.NewGuid().ToString("N"),
                    Ten = viTriModel.Ten,
                    LinkNhomZalo = viTriModel.LinkNhomZalo,
                    Users = viTriModel.Users,
                    CreatedBy = viTriModel.CreatedBy,
                    LastUpdatedBy = viTriModel.LastUpdatedBy,
                    LastUpdatedTime = viTriModel.LastUpdatedTime,
            };
            _context.ViTris.Add(vitriEntity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteViTriAsync(string viTriId, ViTri viTridelete)
        {
            var vitridexoa = await _context.ViTris.FirstOrDefaultAsync(c => c.Id == viTriId);
            if(vitridexoa != null)
            {
                vitridexoa.DeletedBy = viTridelete.DeletedBy;
                vitridexoa.DeletedTime = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ViTri>> GetAllViTriAsync()
        {
            return await _context.ViTris.ToListAsync();
        }

        public async Task<ViTri> GetViTriByIdAsync(string vitriId)
        {
            var vitri = await _context.ViTris.Include(c => c.Users).FirstOrDefaultAsync(c => c.Id == vitriId);
            return vitri;
        }

        public async Task UpdateViTriAsync(string viTriId, ViTri updatedViTri)
        {
            var existingViTri = await _context.ViTris.FirstOrDefaultAsync(c => c.Id == viTriId);
            if(existingViTri != null)
            {
                if(updatedViTri.Ten != null)existingViTri.Ten = updatedViTri.Ten;
                if (updatedViTri.Ten != null) existingViTri.LinkNhomZalo = updatedViTri.LinkNhomZalo;
                existingViTri.LastUpdatedBy = updatedViTri.LastUpdatedBy;
                existingViTri.LastUpdatedTime = updatedViTri.LastUpdatedTime;
                await _context.SaveChangesAsync();
            }
        }
    }
}