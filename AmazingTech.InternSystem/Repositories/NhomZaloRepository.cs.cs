using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models;
using AmazingTech.InternSystem.Models.Request.User;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using static AmazingTech.InternSystem.Data.Enum.Enums;

namespace AmazingTech.InternSystem.Repositories.NhomZaloManagement
{
    public class NhomZaloRepository : INhomZaloRepo
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<NhomZaloRepository> _logger;

        public NhomZaloRepository(AppDbContext appDbContext, ILogger<NhomZaloRepository> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        // Zalo methods
        public async Task<List<NhomZalo>> GetAllZaloAsync()
        {
            return await _appDbContext.NhomZalos.Where(zl => zl.DeletedTime == null)
                                                .OrderByDescending(zl => zl.CreatedTime)
                                                .ToListAsync();
        }

        public async Task<NhomZalo?> GetGroupByIdAsync(string id)
        {
            return await _appDbContext.NhomZalos.Where(zl => zl.DeletedTime == null)
                                                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> AddNewZaloAsync(string user, NhomZalo zalo)
        {
            var existingZalo = await _appDbContext.NhomZalos.FirstOrDefaultAsync(x => x.TenNhom == zalo.TenNhom && x.DeletedTime == null);
            if (existingZalo != null)
            {
                throw new Exception("A group with the same name has already exists.");
            }

            zalo.CreatedBy = user;
            zalo.LastUpdatedBy = user;
            zalo.LastUpdatedTime = DateTime.Now;

            _appDbContext.NhomZalos.Add(zalo);
            return await _appDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateZaloAsync(string id, string user, NhomZalo zalo)
        {
            var nhomZalo = await _appDbContext.NhomZalos.FirstOrDefaultAsync(x => x.Id == id && x.DeletedTime == null);
            if (nhomZalo == null)
            {
                throw new Exception($"NhomZalo with ID ({id}) not found.");
            }

            nhomZalo.TenNhom = zalo.TenNhom ?? nhomZalo.TenNhom;
            nhomZalo.LinkNhom = zalo.LinkNhom ?? nhomZalo.LinkNhom;
            nhomZalo.LastUpdatedBy = user;
            nhomZalo.LastUpdatedTime = DateTime.Now;

            return await _appDbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteZaloAsync(string id, string user)
        {
            var nhomZalo = await _appDbContext.NhomZalos.FirstOrDefaultAsync(x => x.Id == id && x.DeletedTime == null);

            if (nhomZalo == null)
            {
                throw new Exception($"NhomZalo with ID ({id}) not found.");
            }

            nhomZalo.DeletedBy = user;
            nhomZalo.DeletedTime = DateTime.Now;
            //_appDbContext.NhomZalos.Remove(nhomZalo);
            await _appDbContext.SaveChangesAsync();
            return 1;
        }

        //UserNhomZalo methods
        public async Task<List<UserNhomZalo>> GetUsersInGroupAsync(string nhomZaloId)
        {
            return await _appDbContext.UserNhomZalos.Where(x => (x.IdNhomZaloChung == nhomZaloId || x.IdNhomZaloRieng == nhomZaloId) && x.DeletedTime == null)
                                                    .OrderByDescending(nz => nz.JoinedTime)
                                                    .Include(nz => nz.NhomZalo)
                                                    .Include(nz => nz.User)
                                                    .ToListAsync();
        }

        public async Task<UserNhomZalo?> GetUserInGroupAsync(string nhomZaloId, string userId)
        {
            return await _appDbContext.UserNhomZalos.Include(nz => nz.NhomZalo)
                                                    .FirstOrDefaultAsync(x => x.IdNhomZaloChung == nhomZaloId || x.IdNhomZaloRieng == nhomZaloId
                                                                           && x.UserId == userId
                                                                           && x.DeletedTime == null);
        }

        public async Task<int> AddUserToGroupAsync(string idNhomZaloChung, string idNhomZaloRieng, string user, UserNhomZalo addUser)
        {
            var existingUser = _appDbContext.Users.FirstOrDefault(u => u.Id == addUser.UserId && u.DeletedTime == null);
            if (existingUser == null)
            {
                throw new Exception($"User with ID ({addUser.UserId}) not found.");
            }

            var userNhomZalo = await _appDbContext.UserNhomZalos.FirstOrDefaultAsync(x => x.IdNhomZaloChung == idNhomZaloChung || x.IdNhomZaloRieng == idNhomZaloRieng
                                                                                       && x.UserId == addUser.UserId
                                                                                       && x.DeletedTime == null);

            if (userNhomZalo != null)
            {
                throw new Exception($"User with ID ({addUser.UserId}) has already existed in this GroupZalo.");
            }

            addUser.IdNhomZaloChung = idNhomZaloChung;
            addUser.IdNhomZaloRieng = idNhomZaloRieng;
            addUser.JoinedTime = DateTime.Now;
            addUser.CreatedBy = user;
            addUser.LastUpdatedBy = user;
            addUser.LastUpdatedTime = DateTime.Now;

            _appDbContext.UserNhomZalos.Add(addUser);
            return await _appDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateUserInGroupAsync(string nhomZaloId, string user, UserNhomZalo updatedUser)
        {

            var existingUser = _appDbContext.Users.FirstOrDefault(u => u.Id == updatedUser.UserId && u.DeletedTime == null);
            if (existingUser == null)
            {
                throw new Exception($"User with ID ({updatedUser.UserId}) not found.");
            }

            var userNhomZalo = await _appDbContext.UserNhomZalos.FirstOrDefaultAsync(x => x.IdNhomZaloChung == nhomZaloId || x.IdNhomZaloRieng == nhomZaloId
                                                                                       && x.UserId == updatedUser.UserId
                                                                                       && x.DeletedTime == null);

            if (userNhomZalo == null)
            {
                throw new Exception($"User with ID ({updatedUser.UserId}) does not exist in this GroupZalo.");
            }

            //userNhomZalo.UserId = updatedUser.UserId;
            //userNhomZalo.IdNhomZalo = updatedUser.IdNhomZalo;
            userNhomZalo.IsMentor = updatedUser.IsMentor;
            userNhomZalo.JoinedTime = updatedUser.JoinedTime ?? userNhomZalo.JoinedTime;
            userNhomZalo.LeftTime = updatedUser.LeftTime;
            userNhomZalo.LastUpdatedBy = user;
            userNhomZalo.LastUpdatedTime = DateTime.Now;

            return await _appDbContext.SaveChangesAsync();
        }

        public async Task<int> RemoveUserFromGroupAsync(string nhomZaloId, string user, string userId)
        {
            var existingUser = _appDbContext.Users.FirstOrDefault(u => u.Id == userId && u.DeletedTime == null);
            if (existingUser == null)
            {
                throw new Exception($"User with ID ({userId}) not found.");
            }

            var userNhomZalo = await _appDbContext.UserNhomZalos.FirstOrDefaultAsync(x => x.IdNhomZaloChung == nhomZaloId || x.IdNhomZaloRieng == nhomZaloId
                                                                                       && x.UserId == userId
                                                                                       && x.DeletedTime == null);

            if (userNhomZalo == null)
            {
                throw new Exception($"User with ID ({userId}) does not exist in this GroupZalo.");
            }

            userNhomZalo.LeftTime = DateTime.Now;
            userNhomZalo.DeletedBy = user;
            userNhomZalo.DeletedTime = DateTime.Now;

            //_appDbContext.UserNhomZalos.Remove(userNhomZalo);
            return await _appDbContext.SaveChangesAsync();
        }
    }
}
