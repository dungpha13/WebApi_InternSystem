using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using Microsoft.EntityFrameworkCore;
namespace AmazingTech.InternSystem.Repositories
{
    public interface IUserRepository
    {
        public string GetUserIdByEmail(string email);
        public User GetUserById(string id);
        public User GetUserByName(string name);
        public List<User> GetInternWithoutInterview(DateTime startTime, DateTime endTime);
        public List<User> GetHrOrMentorWithoutInterview(DateTime startTime, DateTime endTime);
        public List<User> GetUsersWithoutInterview(DateTime startTime, DateTime endTime);
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
                var user = context.Set<User>().AsNoTracking().Where(x => x.Email == email).FirstOrDefault();
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
        public string GetUserIdByUserName(string userName)
        {
            using (var context = new AppDbContext())
            {
                var user = context.Set<User>().AsNoTracking().Where(x => x.HoVaTen == userName).FirstOrDefault();
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
                var user = context.Set<User>().AsNoTracking().Where(x => x.Id == id).FirstOrDefault();
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
                var user = context.Set<User>().AsNoTracking().Where(x => x.HoVaTen == name).FirstOrDefault();
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
        public List<User> GetInternWithoutInterview(DateTime startTime, DateTime endTime)
        {
            using (var context = new AppDbContext())
            {
                var result = context.Set<User>()
                      .Where(x => x.Roles.Any(r => r.Name.Equals(Roles.INTERN)) && !context.Set<LichPhongVan>().AsNoTracking().Any(l => l.NguoiDuocPhongVan.Id == x.Id && startTime <= l.ThoiGianPhongVan && l.ThoiGianPhongVan <= endTime))
                      .ToList();
                return result;
            }
        }
        public List<User> GetUsersWithoutInterview(DateTime startTime, DateTime endTime)
        {
            using (var context = new AppDbContext())
            {
                var usersWithoutInterview = context.Set<User>()
                    .Where(user => !context.Set<LichPhongVan>().Any(interview => interview.NguoiDuocPhongVan.Id == user.Id && startTime <= interview.ThoiGianPhongVan && interview.ThoiGianPhongVan <= endTime))
                    .ToList();
                return usersWithoutInterview;
            }
        }
        public List<User> GetHrOrMentorWithoutInterview(DateTime startTime, DateTime endTime)
        {
            using (var context = new AppDbContext())
            {
                var result = context.Set<User>()
                      .Where(x => x.Roles.Any(r => r.Name.Equals(Roles.HR.ToUpper()) || r.Name.Equals(Roles.MENTOR)) && !context.Set<LichPhongVan>().AsNoTracking().Any(l => l.NguoiPhongVan.Id == x.Id && startTime <= l.ThoiGianPhongVan && l.ThoiGianPhongVan <= endTime))
                      .Take(5)
                      .ToList();
                return result;
            }
        }
    }
}