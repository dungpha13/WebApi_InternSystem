using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AmazingTech.InternSystem.Data
{
    public interface IAppDbContext : IDisposable
    {
        DatabaseFacade DatabaseFacade { get; }

        EntityEntry Add(object entity);
    }
}
