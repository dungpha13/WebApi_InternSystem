using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace AmazingTech.InternSystem.Repositories
{
    public interface ILichPhongVanRepository
    {
        public void DeleteLichPhongVan(LichPhongVan lichPhongVan);
        public void addNewLichPhongVan(LichPhongVan entity);
        public List<LichPhongVan> GetLichPhongVansByIdNgPhongVan(string id);
        public void UpdateLichPhongVan(LichPhongVan lichPhongVan);
        public LichPhongVan GetScheduleById(string scheduleId);
 
        public LichPhongVan GetScheduleByInterviewerIdAndIntervieweeId(string interviewerid, string intervieweeid);
        public LichPhongVan GetScheduleByIntervieweeId(string intervieeid);
    }
    public class LichPhongVanRepository : ILichPhongVanRepository
    {
        private DbSet<LichPhongVan> _dbSet;
        public LichPhongVanRepository()
        {

        }
        
        public void addNewLichPhongVan(LichPhongVan entity)
        {
            using(var context = new AppDbContext())
            {
                context.Set<LichPhongVan>().Add(entity);
                context.SaveChanges();
            }
        }
        public List<LichPhongVan> GetLichPhongVansByIdNgPhongVan(string id)
        {
            using (var context = new AppDbContext())
            {
                var list = context.Set<LichPhongVan>().AsNoTracking().Where(x => x.IdNguoiPhongVan == id).ToList();
                return list;
            }
        }
        public void UpdateLichPhongVan(LichPhongVan lichPhongVan)
        {
            using (var context = new AppDbContext())
            {
                context.Set<LichPhongVan>().Update(lichPhongVan);
                context.SaveChanges();
            }
        }
        public LichPhongVan GetScheduleByInterviewerIdAndIntervieweeId(string interviewerid , string intervieweeid)
        {
            using(var context = new AppDbContext())
            {
                return context.Set<LichPhongVan>().AsNoTracking().Where(x => x.IdNguoiDuocPhongVan == intervieweeid && x.IdNguoiPhongVan == interviewerid).SingleOrDefault();
            }
        }
        public void DeleteLichPhongVan(LichPhongVan lichPhongVan)
        {
            using (var context = new AppDbContext())
            {
                context.Set<LichPhongVan>().Remove(lichPhongVan);
                context.SaveChanges();
            }
        }
        public LichPhongVan GetScheduleById(string scheduleId)
        {
            using (var context = new AppDbContext())
            {
                return context.Set<LichPhongVan>().AsNoTracking().Where(x => x.Id == scheduleId).SingleOrDefault();
            }
        }

        public LichPhongVan GetScheduleByIntervieweeId(string intervieweeid)
        {
            using (var context = new AppDbContext())
            {
                return context.Set<LichPhongVan>().AsNoTracking().Where(x => x.IdNguoiDuocPhongVan == intervieweeid).SingleOrDefault();
            }
        }
    }
}
