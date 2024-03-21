using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Data.Enum;
using AmazingTech.InternSystem.Models.Response;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;

namespace AmazingTech.InternSystem.Repositories
{
    public interface ILichPhongVanRepository
    {
        public List<LichPhongVan> GetScheduleOfInterviewerInPeriodTime(string interviewer, DateTime startDate, DateTime EndTime);
        public void DeleteLichPhongVan(LichPhongVan lichPhongVan);
        public void addNewLichPhongVan(LichPhongVan entity);
        public List<LichPhongVan> GetLichPhongVansByIdNgPhongVan(string id);
        public LichPhongVan GetLichPhongVansByIdNgDuocPhongVan(string id);
        public void UpdateLichPhongVan(LichPhongVan lichPhongVan);
        public LichPhongVan GetScheduleById(string scheduleId);
 
        public LichPhongVan GetScheduleByInterviewerIdAndIntervieweeId(string interviewerid, string intervieweeid);
        public LichPhongVan GetScheduleByIntervieweeId(string intervieeid);

        public List<LichPhongVan> GetAllLichPhongVan();

        public List<LichPhongVan> GetListScheduleByInterviewer(string interviewerid);

        public List<LichPhongVan> GetLichPhongVanByIdNguoiDuocPhongVan(string idNguoiDuocPhongVan);
        public List<LichPhongVan> GetLichPhongVanByKetQua(Result ketqua);
        public List<LichPhongVan> GetLichPhongVanWithConsiderResult();
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
                return context.Set<LichPhongVan>().AsNoTracking().Where(x => x.Id == scheduleId && x.DeletedBy == null).SingleOrDefault();
            }
        }

        public LichPhongVan GetScheduleByIntervieweeId(string intervieweeid)
        {
            using (var context = new AppDbContext())
            {
                return context.Set<LichPhongVan>().AsNoTracking().Where(x => x.IdNguoiDuocPhongVan == intervieweeid && x.DeletedBy == null).SingleOrDefault();
            }
        }
        public List<LichPhongVan> getScheduleInPeriodTime(DateTime startDate , DateTime EndTime)
        {
            using (var context = new AppDbContext())
            {
                return context.Set<LichPhongVan>().AsNoTracking().Where(x => startDate <= x.ThoiGianPhongVan && x.ThoiGianPhongVan <= EndTime).ToList();  
            }
        }
        public List<LichPhongVan> GetScheduleOfInterviewerInPeriodTime(string interviewer, DateTime startDate, DateTime EndTime)
        {
            using (var context = new AppDbContext())
            {
                return context.Set<LichPhongVan>().AsNoTracking().Where(x => x.IdNguoiPhongVan ==interviewer &&startDate <= x.ThoiGianPhongVan && x.ThoiGianPhongVan <= EndTime && x.DeletedBy == null).ToList();
            }
        }

        public List<LichPhongVan> GetAllLichPhongVan()
        {
            using (var context = new AppDbContext())
            {
                var lichphongvans = context.Set<LichPhongVan>().AsNoTracking().Where(t => t.DeletedTime == null && t.DeletedBy == null).ToList();
                
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
               var list = context.Set<LichPhongVan>().AsNoTracking().Where( x => x.IdNguoiDuocPhongVan == idNguoiDuocPhongVan && x.DeletedTime == null).ToList();
                return list;
            }
        }

        public LichPhongVan GetLichPhongVansByIdNgDuocPhongVan(string id)
        {
            using (var context = new AppDbContext())
            {
                var list = context.Set<LichPhongVan>().AsNoTracking().Where(x => x.IdNguoiDuocPhongVan == id && x.DeletedTime == null).SingleOrDefault();
                return list;
            }
        }

        public List<LichPhongVan> GetLichPhongVanByKetQua(Result ketqua)
        {
            using (var context = new AppDbContext())
            {
                var list = context.Set<LichPhongVan>().AsNoTracking().Where(x => x.KetQua == ketqua && x.DeletedTime == null).ToList();
                return list;
            }
        }
        public List<LichPhongVan> GetLichPhongVanWithConsiderResult()
        {
            using (var context = new AppDbContext())
            {
                var list = context.Set<LichPhongVan>().AsNoTracking().Where(x => x.KetQua == Result.Consider  && x.DeletedTime == null).ToList();
                return list;
            }
        }
    }
}
