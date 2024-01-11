using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;

namespace AmazingTech.InternSystem.Repositories
{
    public class LichPhongVanRepository
    {
        private readonly AppDbContext _context;

        public LichPhongVanRepository(AppDbContext context)
        {
            _context = context;
        }

        public LichPhongVan GetLichPhongVanById(string id)
        {
            return _context.LichPhongVans.Find(id);
        }
        public void AddLichPhongVan(LichPhongVan lichPhongVan)
        {
            _context.LichPhongVans.Add(lichPhongVan);
        }

        public void UpdateLichPhongVan(LichPhongVan lichPhongVan)
        {
            _context.LichPhongVans.Update(lichPhongVan);
        }

        public void DeleteLichPhongVan(string id)
        {
            var lichPhongVan = _context.LichPhongVans.Find(id);

            if (lichPhongVan != null)
            {
                _context.LichPhongVans.Remove(lichPhongVan);
            }
        }
        public List<LichPhongVan> GetLichPhongVans()
        {
            return _context.LichPhongVans.ToList();
        }
    }
}
