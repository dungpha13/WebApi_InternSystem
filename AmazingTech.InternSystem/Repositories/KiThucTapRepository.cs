using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace AmazingTech.InternSystem.Repositories
{
    public class KiThucTapRepository : IKiThucTapRepository
    {

        public KiThucTapRepository() { }

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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
