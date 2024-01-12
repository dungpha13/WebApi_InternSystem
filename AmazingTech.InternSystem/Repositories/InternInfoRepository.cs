using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request;
using AmazingTech.InternSystem.Models.Request.InternInfo;
using AmazingTech.InternSystem.Models.Response;
// using AmazingTech.InternSystem.Models.Response.InternInfo;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace AmazingTech.InternSystem.Repositories
{
    public class InternInfoRepository : IInternInfoRepo
    {
        private readonly AppDbContext _context;

        public InternInfoRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
        }

        public async Task<int> AddInternInfoAsync(InternInfo entity)
        {

            //var newIntern = mapper.Map<InternInfo>(model);

            _context.InternInfos.Add(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task DeleteInternInfoAsync(string MSSV)
        {
            var intern = await _context.InternInfos!.FirstOrDefaultAsync(i => i.MSSV == MSSV);
            if (intern != null)
            {
                _context.InternInfos.Remove(intern);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddListInternInfoAsync(List<InternInfo> list)
        {
            foreach (var item in list)
            {
                _context.InternInfos.Add(item);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<InternInfo?> GetInternInfo(string id)
        {
            return await _context.InternInfos
                    .Where(intern => intern.Id == id)
                        .Include(intern => intern.KiThucTap)
                    .FirstOrDefaultAsync();
        }
    }
}
