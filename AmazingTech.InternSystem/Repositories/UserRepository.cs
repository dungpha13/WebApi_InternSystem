using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
namespace AmazingTech.InternSystem.Repositories
{
    public interface IUserRepository
    {
        public string GetUserIdByEmail(string email);
        public User GetUserById(string id);
        public User GetUserByName(string name);
        //public List<User> GetInternWithoutInterview(DateTime startTime, DateTime endTime);
        public List<User> GetHrOrMentorWithoutInterview(DateTime startTime, DateTime endTime);
        public List<User> GetInternsWithoutInterview();
        public User GetUserByEmail(string email);
        public List<User> GetUserHavingInterviewScheduleAndStatusDoneInAYear(int year);
        public List<User> GetUsersHavingInterviewScheduleAndStatusDoneInAQuarter(int year, int quarter);

    }
    public class UserRepository : IUserRepository
    {
        private readonly DbSet<User> DbSet;
        public UserRepository()
        {
        }
        public string GetUserIdByEmail(string email)
        {
            using (var context = new AppDbContext())
            {
                var user = context.Set<User>().AsNoTracking().Where(x => x.Email == email && x.DeletedTime == null).FirstOrDefault();
                if (user == null)
                {
                    return null;
                }
                else
                {
                    return user.Id;
                }
            }
        }
        public User GetUserByEmail(string email)
        {
            using (var context = new AppDbContext())
            {
                var user = context.Set<User>().AsNoTracking().Where(x => x.Email == email && x.DeletedTime == null).FirstOrDefault();
                if (user == null)
                {
                    return null;
                }
                else
                {
                    return user;
                }
            }
        }
        public string GetUserIdByUserName(string userName)
        {
            using (var context = new AppDbContext())
            {
                var user = context.Set<User>().AsNoTracking().Where(x => x.HoVaTen == userName && x.DeletedTime == null).FirstOrDefault();
                if (user == null)
                {
                    return null;
                }
                else
                {
                    return user.Id;
                }
            }
        }
        public User GetUserById(string id)
        {
            using (var context = new AppDbContext())
            {
                var user = context.Set<User>().AsNoTracking().Where(x => x.Id == id && x.DeletedTime == null).FirstOrDefault();
                if (user == null)
                {
                    return null;
                }
                else
                {
                    return user;
                }
            }
        }
        public User GetUserByName(string name)
        {
            using (var context = new AppDbContext())
            {
                var user = context.Set<User>().AsNoTracking().Where(x => x.HoVaTen == name && x.DeletedTime == null).FirstOrDefault();
                if (user == null)
                {
                    return null;
                }
                else
                {
                    return user;
                }
            }
        }
        //public List<User> GetInternWithoutInterview(DateTime startTime, DateTime endTime)
        //{
        //    using (var context = new AppDbContext())
        //    {
        //        var result = context.Set<User>()
        //              .Where(x => x.Roles.Any(r => r.Name.Equals(Roles.INTERN)) && !context.Set<LichPhongVan>().AsNoTracking().Any(l => l.NguoiDuocPhongVan.Id == x.Id && startTime <= l.ThoiGianPhongVan && l.ThoiGianPhongVan <= endTime))
        //              .ToList();
        //        return result;
        //    }
        //}
        public List<User> GetInternsWithoutInterview()
        {

            using (var context = new AppDbContext())
            {
                var usersWithoutInterviewOrDeletedInterview = context.Set<User>()
                    .Where(user => !context.Set<LichPhongVan>()
                        .Any(interview => interview.NguoiDuocPhongVan.Id == user.Id)
                        || context.Set<LichPhongVan>()
                            .Any(interview => interview.NguoiDuocPhongVan.Id == user.Id && interview.DeletedBy != null))
                    .ToList();
                return usersWithoutInterviewOrDeletedInterview;
            }
        }
        public List<User> GetHrOrMentorWithoutInterview(DateTime startTime, DateTime endTime)
        {
            using (var context = new AppDbContext())
            {
                var usersWithoutInterview = context.Set<User>()
                    .Include(user => user.LichPhongVans_PhongVan) // Thêm phương thức Include để join bảng LichPhongVan
                    .Where(user => !context.Set<LichPhongVan>().Any(interview => interview.NguoiPhongVan.Id == user.Id && startTime <= interview.ThoiGianPhongVan && interview.ThoiGianPhongVan <= endTime))
                    .ToList();
                return usersWithoutInterview;
            }
        }
        public List<User> GetUserHavingInterviewScheduleAndStatusDoneInAYear(int year)
        {
            using(var context = new AppDbContext()) 
            {
                var user = context.Set<User>().Where(user => context.Set<LichPhongVan>()
                .Any(interview => interview.NguoiDuocPhongVan.Id == user.Id 
                && interview.LastUpdatedTime.HasValue && interview.LastUpdatedTime.Value.Year == year 
                && interview.DeletedTime == null && interview.TrangThai == Data.Enum.Status.Done)).ToList();
                return user;
            }
        }
        
        public List<User> GetUsersHavingInterviewScheduleAndStatusDoneInAQuarter(int year, int quarter)
        {
            using (var context = new AppDbContext())
            {
                var startDate = new DateTime(year, 3 * quarter - 2, 1);
                var endDate = startDate.AddMonths(3).AddDays(-1);

                var users = context.Set<User>().Where(user => context.Set<LichPhongVan>()
                    .Any(interview => interview.NguoiDuocPhongVan.Id == user.Id
                        && interview.LastUpdatedTime.HasValue
                        && interview.LastUpdatedTime.Value >= startDate
                        && interview.LastUpdatedTime.Value <= endDate
                        && interview.DeletedTime == null
                        && interview.TrangThai == Data.Enum.Status.Done)).ToList();

                return users;
            }
        }
    }

}