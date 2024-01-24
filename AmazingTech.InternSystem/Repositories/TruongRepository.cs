using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace AmazingTech.InternSystem.Repositories
{
    public class TruongRepository : ITruongRepository
    {
        private readonly AppDbContext _context;

        public TruongRepository(AppDbContext context)
        {
            _context = context;
        }

        public int AddTruong(TruongHoc truong)
        {
            using (var context = new AppDbContext())
            {
                context.Set<TruongHoc>().Add(truong);
                return context.SaveChanges();
            }
        }

        public async Task<int> DeleteTruong(TruongHoc truong)
        {
            var currentTime = DateTime.Now;

            truong.DeletedBy = "Admin";
            truong.DeletedTime = currentTime;

            return await _context.SaveChangesAsync();
        }

        public List<TruongHoc> GetAllTruongs()
        {
            using (var context = new AppDbContext())
            {
                var truongs = context.Set<TruongHoc>().Where(t => t.DeletedBy == null).ToList();
                return truongs;
            }
        }

        public TruongHoc? GetTruong(string id)
        {
            return _context.TruongHocs.FirstOrDefault(t => t.Id == id && t.DeletedBy == null);
        }

        public int UpdateTruong(TruongHoc truong)
        {
            _context.TruongHocs.Update(truong);
            return _context.SaveChanges();
        }
    }
}
