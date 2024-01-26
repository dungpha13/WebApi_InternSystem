using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Response;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
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

        public List<LichPhongVan> GetAllLichPhongVan();

        public List<LichPhongVan> GetListScheduleByInterviewer(string interviewerid);

        public List<LichPhongVan> GetLichPhongVanByIdNguoiDuocPhongVan(string idNguoiDuocPhongVan);
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
                var list = context.Set<LichPhongVan>().AsNoTracking().Where(x => x.IdNguoiPhongVan == id && x.DeletedTime == null).ToList();
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
                return context.Set<LichPhongVan>().AsNoTracking().Where(x => x.IdNguoiDuocPhongVan == intervieweeid && x.DeletedTime == null).SingleOrDefault();
            }
        }
        public List<LichPhongVan> getScheduleInPeriodTime(DateTime startDate , DateTime EndTime)
        {
            using (var context = new AppDbContext())
            {
                return context.Set<LichPhongVan>().AsNoTracking().Where(x => startDate <= x.ThoiGianPhongVan && x.ThoiGianPhongVan <= EndTime).ToList();  
            }
        }

        public List<LichPhongVan> GetAllLichPhongVan()
        {
            using (var context = new AppDbContext())
            {
                var lichphongvans = context.Set<LichPhongVan>().ToList();
                Console.WriteLine("Lich Phonng Van: " + lichphongvans);
                return lichphongvans;
            }
        }

        public List<LichPhongVan> GetListScheduleByInterviewer(string interviewerid)
        {
            using (var context = new AppDbContext())
            {
                return context.Set<LichPhongVan>().AsNoTracking().Where(x => x.IdNguoiPhongVan == interviewerid).ToList();
            }
        }

        public List<LichPhongVan> GetLichPhongVanByIdNguoiDuocPhongVan(string idNguoiDuocPhongVan)
        {
            using (var context = new AppDbContext())
            {
                return context.Set<LichPhongVan>()
                    .AsNoTracking()
                    .Where(x => x.IdNguoiDuocPhongVan == idNguoiDuocPhongVan).ToList();
            }
        }
    }
}
