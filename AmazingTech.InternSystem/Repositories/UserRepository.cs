using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace AmazingTech.InternSystem.Repositories
{
    public interface IUserRepository
    {
        public string GetUserIdByEmail(string email);
    }
    public class UserRepository : IUserRepository
    {
        private readonly DbSet<User> DbSet;
        public UserRepository()
        {

        }
        public string GetUserIdByEmail(string email)
        {
            using(var context = new AppDbContext())
            {
                var user = context.Set<User>().AsNoTracking().Where(x => x.Email == email).FirstOrDefault();    
                if(user == null)
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
    }

}
