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

            //var intern = await _context.InternInfos.FirstOrDefaultAsync(x => x.MSSV == mssv);
            //if (intern == null)
            //{
            //    return 0;
            //}


            ////Update UserViTri
            //var existUserViTri = await _context.UserViTris
            //       .Where(uv => uv.UsersId == intern.UserId)
            //       .ToListAsync();
            //_context.UserViTris.RemoveRange(existUserViTri);  



            //foreach (var viTriId in model.ViTrisId)
            //{
            //    // Kiểm tra nếu viTriId tồn tại trong CSDL
            //    var isViTriExist = await _context.ViTris.AnyAsync(vt => vt.Id == viTriId);

            //    if (isViTriExist)
            //    {

            //        var userViTri = new UserViTri
            //        {
            //            UsersId = intern.UserId!,
            //            ViTrisId = viTriId
            //        };

            //        _context.UserViTris.Add(userViTri);
            //    }
            //    else
            //    {
            //        return 0;
            //    }
            //}


            ////Update UserNhomZalo
            //var existUserNhomZalo = await _context.UserNhomZalos
            //      .Where(unz => unz.UserId == intern.UserId)
            //      .ToListAsync();
            //_context.UserNhomZalos.RemoveRange(existUserNhomZalo);

            //foreach (var nhomZaloId in model.IdNhomZalo)
            //{
            //    var isNhomZaloExist = await _context.NhomZalos.AnyAsync(nz => nz.Id == nhomZaloId);

            //    if (isNhomZaloExist)
            //    {
            //        var userNhomZalo = new UserNhomZalo
            //        {
            //            UserId = intern.UserId!,
            //            IdNhomZalo = nhomZaloId
            //        };

            //        _context.UserNhomZalos.Add(userNhomZalo);
            //    }
            //    else
            //    {
            //        return 0;
            //    }
            //}


            ////Update UserDuAn
            //var existUserDuAn = await _context.InternDuAns
            //        .Where(uda => uda.UserId == intern.UserId)
            //        .ToListAsync();
            //_context.InternDuAns.RemoveRange(existUserDuAn);

            //foreach (var duAnId in model.IdDuAn)
            //{
            //    var userDuAn = new UserDuAn
            //    {
            //        UserId = intern.UserId,
            //        IdDuAn = duAnId
            //    };

            //    _context.InternDuAns.Add(userDuAn);
            //}


           var intern = mapper.Map<InternInfo>(model);

            _context.InternInfos?.Update(intern);
            return await _context.SaveChangesAsync();
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
