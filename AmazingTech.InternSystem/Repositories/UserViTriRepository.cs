using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace AmazingTech.InternSystem.Repositories
{
    public interface IUserViTriRepository
    {
        public void AddUserViTriRepository(UserViTri userViTri);
    }
    public class UserViTriRepository : IUserViTriRepository
    {
        private readonly DbSet<UserViTri> _DbSet;
        public UserViTriRepository()
        {

        }
        public void AddUserViTriRepository(UserViTri userViTri)
        {
            using(var context = new AppDbContext())
            {
                context.Set<UserViTri>().Add(userViTri);
                context.SaveChanges();
            }
        }
    }
}
