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
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public InternInfoRepository(AppDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<int> AddInternInfoAsync(InternInfo entity)
        {         
            context.InternInfos.Add(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteInternInfoAsync(InternInfo entity)
        {
            var currentTime = DateTime.Now;

            entity.DeletedBy = "Admin";
            entity.DeletedTime = currentTime;

            return await context.SaveChangesAsync();
            
        }

        public async Task<List<InternInfo>> GetAllInternsInfoAsync()
        {
                var interns = await context.InternInfos!
                    .Where(intern => intern.DeletedBy == null)
                    .OrderByDescending(intern => intern.CreatedTime)
                    .Include(intern => intern.User)
                    .ThenInclude(user => user.ViTris)
                    .ToListAsync();
                return interns;
        }

        public async Task<InternInfo> GetInternInfoAsync(string MSSV)
        {
           var intern = await context.InternInfos
                            .Include(intern => intern.User)
                            .ThenInclude(user => user.ViTris)
                            .FirstOrDefaultAsync(i => i.MSSV == MSSV); 
           
           return intern;
        }

        public async Task<int> UpdateInternInfoAsync(string mssv, UpdateInternInfoDTO model)
        {
            
            var intern =  await context.InternInfos.FirstOrDefaultAsync(x => x.MSSV == mssv);
            if (intern == null)
            {
                return 0;
            }
            intern.LastUpdatedBy = "Admin";
     
            mapper.Map(model, intern);

            context.InternInfos?.Update(intern);
            return await context.SaveChangesAsync();
            

        }
    }
}
