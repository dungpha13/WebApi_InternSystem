using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Data;
using Microsoft.EntityFrameworkCore;
using AmazingTech.InternSystem.Repositories;

namespace AmazingTech.InternSystem.Repositories
{
    public class ViTriRepository : IViTriRepository
    {
        private readonly AppDbContext _context;

        public ViTriRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ViTri>> GetAllVitri()
        {
            return await _context.ViTris.Where(x => x.DeletedBy == null).ToListAsync();
        }
        public async Task<int> CreateViTri(ViTri viTri)
        {
            _context.ViTris.Add(viTri);
            return await _context.SaveChangesAsync();

        }
        public async Task<int> UpdateViTri(string viTriId, ViTri updatedViTri)
        {
            var vitri = _context.ViTris.FirstOrDefault(x => x.Id == viTriId && x.DeletedBy == null);
            if (vitri == null)
            {
                return 0;
            }
            vitri.Ten = updatedViTri.Ten;
            vitri.LinkNhomZalo = updatedViTri.LinkNhomZalo;
            return await _context.SaveChangesAsync();
        }
        public async Task<int> DeleteViTri(string viTriId, string user)
        {
            var vitri = _context.ViTris.FirstOrDefault(x => x.Id == viTriId && x.DeletedBy == null);
            if (vitri == null)
            {
                return 0;
            }
            vitri.DeletedBy = user;
            vitri.DeletedTime = DateTime.Now;
            return await _context.SaveChangesAsync();

        }
        public async Task<List<InternInfo>> UserViTriView(string id)
        {
            var user = await _context.Users.Where(x => x.UserViTris.Where(p => p.UsersId == x.Id).Any() && x.DeletedTime == null).ToListAsync();
            var intern = await _context.InternInfos.Where(x => x.DeletedTime == null).ToListAsync();
            List<InternInfo> internInfos = new List<InternInfo>();
            foreach (User obj in user)
            {
                foreach (InternInfo obj1 in intern)
                {
                    if (obj1.UserId == obj.Id)
                    {
                        internInfos.Add(obj1);
                    }
                }
            }
            return internInfos;
        }
        public ViTri GetViTriByName(string name)
        {
            return (ViTri)_context.ViTris.Where(x => x.Ten == name).FirstOrDefault();
        }
    }
}