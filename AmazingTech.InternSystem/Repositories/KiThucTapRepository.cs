using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace AmazingTech.InternSystem.Repositories
{
    public class KiThucTapRepository : IKiThucTapRepository
    {
        private readonly AppDbContext _context;

        public KiThucTapRepository(AppDbContext context)
        {
            _context = context;
        }

        public int AddKiThucTap(KiThucTap ki)
        {
            using (var context = new AppDbContext())
            {
                context.Set<KiThucTap>().Add(ki);
                return context.SaveChanges();
            }
        }

        public async Task<int> DeleteKiThucTap(KiThucTap kiThucTap)
        {
            var currentTime = DateTime.Now;

            kiThucTap.DeletedBy = "Admin";
            kiThucTap.DeletedTime = currentTime;

            _context.KiThucTaps.Update(kiThucTap);
            return await _context.SaveChangesAsync();
        }

        public List<KiThucTap> GetAllKiThucTaps()
        {
            using (var context = new AppDbContext())
            {
                var kis = context.Set<KiThucTap>()
                .Include(ki => ki.Interns)
                // .ThenInclude(intern => intern.Truong)
                .Include(ktt => ktt.Truong)
                .Where(u => u.DeletedBy == null)
                .ToList();
                return kis;
            }
        }

        public KiThucTap? GetKiThucTap(string id)
        {
            using (var context = new AppDbContext())
            {
                return context.KiThucTaps
                       // .Include(ktt => ktt.InternInfos)
                        .FirstOrDefault(ktt => ktt.Id == id);
            }
        }

        public int UpdateKiThucTap(KiThucTap kiThucTap)
        {
            _context.KiThucTaps.Update(kiThucTap);
            return _context.SaveChanges();
        }
    }
}