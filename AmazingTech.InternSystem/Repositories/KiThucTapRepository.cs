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

        public int DeleteKiThucTap(KiThucTap kiThucTap)
        {
            _context.KiThucTaps.Remove(kiThucTap);
            return _context.SaveChanges();
        }

        public List<KiThucTap> GetAllKiThucTaps()
        {
            using (var context = new AppDbContext())
            {
                var kis = context.Set<KiThucTap>()
                    .Include(ki => ki.Truong)
                    .Include(ki => ki.Interns)
                    .ToList();
                return kis;
            }
        }

        public KiThucTap? GetKiThucTap(string id)
        {
            using (var context = new AppDbContext())
            {
                return context.KiThucTaps
                        .Include(ktt => ktt.Truong)
                        .Include(ktt => ktt.Interns)
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
