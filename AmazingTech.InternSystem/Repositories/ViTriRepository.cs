using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;

namespace AmazingTech.InternSystem.Repositories
{
    public interface IViTriRepository
    {
        List<ViTri> GetAllViTris();
    }

    public class ViTriRepository : IViTriRepository
    {
        public List<ViTri> GetAllViTris()
        {
            using (var context = new AppDbContext())
            {
                return context.Set<ViTri>().ToList();
            }
        }
    }
}
