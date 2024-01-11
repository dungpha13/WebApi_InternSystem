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

        public int DeleteKiThucTap(string id)
        {
            var kiThucTap = _context.KiThucTaps.FirstOrDefault(ktt => ktt.Id == id);

            if (kiThucTap is not null)
            {
                _context.KiThucTaps.Remove(kiThucTap);
                return _context.SaveChanges();
            }

            return 0;

        }

        public List<KiThucTap> GetAllKiThucTaps()
        {
            using (var context = new AppDbContext())
            {
                var kis = context.Set<KiThucTap>().ToList();
                return kis;
            }
        }

        public KiThucTap? GetKiThucTap(string id)
        {
            using (var context = new AppDbContext())
            {
                return context.KiThucTaps.FirstOrDefault(ktt => ktt.Id == id);
            }
        }

        public int UpdateKiThucTap(KiThucTap kiThucTap)
        {
            var existingKiThuTap = _context.KiThucTaps.FirstOrDefault(ktt => ktt.Id == kiThucTap.Id);

            if (existingKiThuTap is not null)
            {
                _context.Entry(existingKiThuTap).CurrentValues.SetValues(kiThucTap);
                return _context.SaveChanges();
            }

            return 0;
        }
    }
}
