using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace AmazingTech.InternSystem.Repositories
{
    public interface IRoleRepository
    {

    }
    public class RoleRepository : IRoleRepository
    {
        private readonly DbSet<Role> _DbSet;
        public RoleRepository()
        {

        }
    
    }
}
