using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request;
using AmazingTech.InternSystem.Models.Request.InternInfo;
using AmazingTech.InternSystem.Models.Response;
using AmazingTech.InternSystem.Models.Response.InternInfo;
using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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

        public async Task<int> AddInternInfoAsync(string userId, InternInfo entity)
        {

            var user = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            string userName = user.UserName;
            //createBy
            entity.CreatedBy = userName;

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
                .Include(intern => intern.User!.UserViTris)
                    .ThenInclude(uservitri => uservitri.ViTri)
                .Include(intern => intern.User!.UserNhomZalos)
                    .ThenInclude(usernhomzalo => usernhomzalo.NhomZalo)
                .Include(intern => intern.User!.UserDuAns)
                    .ThenInclude(userduan => userduan.DuAn)
                .ToListAsync();
            return interns;
        }

        public async Task<InternInfo> GetInternInfoAsync(string MSSV)
        {
            var intern = await _context.InternInfos
                             .Include(intern => intern.User)
                             .Include(intern => intern.User!.UserViTris)
                                .ThenInclude(uservitri => uservitri.ViTri)
                            .Include(intern => intern.User!.UserNhomZalos)
                                .ThenInclude(usernhomzalo => usernhomzalo.NhomZalo)
                            .Include(intern => intern.User!.UserDuAns)
                                .ThenInclude(userduan => userduan.DuAn)
                             .FirstOrDefaultAsync(i => i.MSSV == MSSV);

            return intern;
        }

        public async Task<int> UpdateInternInfoAsync(string mssv, UpdateInternInfoDTO model)
        {
            var intern = mapper.Map<InternInfo>(model);
            _context.InternInfos?.Update(intern);
            return await _context.SaveChangesAsync();
        }

        public async Task<InternInfo?> GetInternInfo(string id)
        {
            return await _context.InternInfos
                    .Where(intern => intern.Id == id)
                        .Include(intern => intern.KiThucTap)
                        .Include(intern => intern.Truong)
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


        public async Task<InternInfo> GetCommentByMssv(string mssv)
        {
            UserRepository _userRepo = new UserRepository();
            var intern = await _context.InternInfos
                .Include(intern => intern.Comments)
                   .ThenInclude(comment => comment.NguoiComment)
                .FirstOrDefaultAsync(i => i.MSSV == mssv);

            return intern;
        }

    }
}
