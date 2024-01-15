using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request;
using AmazingTech.InternSystem.Models.Request.InternInfo;
using AmazingTech.InternSystem.Models.Response;
using AmazingTech.InternSystem.Models.Response.InternInfo;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace AmazingTech.InternSystem.Repositories
{
    public class InternInfoRepository : IInternInfoRepo
    {
        private readonly AppDbContext _context;
        private readonly IMapper mapper;

        public InternInfoRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        public async Task<int> AddInternInfoAsync(InternInfo entity)
        {
            _context.InternInfos.Add(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteInternInfoAsync(InternInfo entity)
        {
            var currentTime = DateTime.Now;

            entity.DeletedBy = "Admin";
            entity.DeletedTime = currentTime;

            return await _context.SaveChangesAsync();

        }

        public async Task<List<InternInfo>> GetAllInternsInfoAsync()
        {
            var interns = await _context.InternInfos!
                .Where(intern => intern.DeletedBy == null)
                .OrderByDescending(intern => intern.CreatedTime)
                .Include(intern => intern.User)
                .ThenInclude(user => user.ViTris)
                .ToListAsync();
            return interns;
        }

        public async Task<InternInfo> GetInternInfoAsync(string MSSV)
        {
            var intern = await _context.InternInfos
                             .Include(intern => intern.User)
                             .ThenInclude(user => user.ViTris)
                             .FirstOrDefaultAsync(i => i.MSSV == MSSV);

            return intern;
        }

        public async Task<int> UpdateInternInfoAsync(string mssv, UpdateInternInfoDTO model)
        {

            var intern = await _context.InternInfos.FirstOrDefaultAsync(x => x.MSSV == mssv);
            if (intern == null)
            {
                return 0;
            }
            intern.LastUpdatedBy = "Admin";

            mapper.Map(model, intern);

            _context.InternInfos?.Update(intern);
            return await _context.SaveChangesAsync();

            await _context.SaveChangesAsync();
        }

        public async Task<InternInfo?> GetInternInfo(string id)
        {
            return await _context.InternInfos
                    .Where(intern => intern.Id == id)
                        .Include(intern => intern.KiThucTap)
                    .FirstOrDefaultAsync();
        }

        public Task<int> AddListInternInfoAsync(List<InternInfo> interns)
        {
            foreach (var intern in interns)
            {
                _context.InternInfos.Add(intern);
            }

            return _context.SaveChangesAsync();
        }

    }
}
