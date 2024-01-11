using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;

namespace AmazingTech.InternSystem.Repositories
{
    public class InternRepository : IInternRepository
    {
        private readonly AppDbContext _context;

        public InternRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddListIntern(List<InternInfo> list)
        {
            foreach (var item in list)
            {
                _context.InternInfos.Add(item);
            }
            _context.SaveChanges();
        }

    }
}