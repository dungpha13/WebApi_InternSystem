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

        public void AddListIntern(List<InternInfo> list, string kiThucTapId)
        {
            var kiThucTap = _context.KiThucTaps.FirstOrDefault(ktt => ktt.Id == kiThucTapId);

            if (kiThucTap is not null)
            {
                foreach (var item in list)
                {
                    item.User = new User();
                    item.KiThucTap = kiThucTap;
                    item.StartDate = kiThucTap.NgayBatDau;
                    item.EndDate = kiThucTap.NgayKetThuc;
                    _context.InternInfos.Add(item);
                }
                _context.SaveChanges();
            }

        }

    }
}