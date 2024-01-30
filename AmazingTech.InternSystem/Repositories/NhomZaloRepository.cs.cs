using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models;
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
        private readonly UserManager<User> _userManager;

        public NhomZaloRepository(AppDbContext appDbContext, ILogger<NhomZaloRepository> logger, UserManager<User> userManager)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            _userManager = userManager;
        }

        // Zalo methods
        public async Task<List<NhomZalo>> GetAllZaloAsync()
        {
            return await _appDbContext.NhomZalos
                .Include(nz => nz.Mentor) 
                .ToListAsync();
        }

        public async Task<NhomZalo?> GetGroupByIdAsync(string id)
        {
            var groupZalo = await _appDbContext.NhomZalos
                .Include(nz => nz.Mentor)
                .FirstOrDefaultAsync(x => x.Id == id && x.DeletedBy == null);
            if (groupZalo == null)
            {
                throw new Exception();
            }
            return groupZalo;
        }

        public async Task<int> AddNewZaloAsync(string user, NhomZalo zalo)
        {
            var nhomZalo = _appDbContext.NhomZalos.Where(x => x.TenNhom == zalo.TenNhom  && x.DeletedBy == null).FirstOrDefault();
            if (nhomZalo == null)
            {
                throw new Exception();
            }

            zalo.CreatedBy = user;
            zalo.LastUpdatedBy = user;
            zalo.LastUpdatedTime = DateTime.Now;
            _appDbContext.NhomZalos.Add(zalo);
            return await _appDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateZaloAsync(string id, string user, NhomZalo zalo)
        {
            var nhomZalo = await _appDbContext.NhomZalos.Where(x => x.Id == id && x.DeletedBy == null).FirstOrDefaultAsync();

            if (nhomZalo == null)
            {
                throw new Exception();
            }

            nhomZalo.TenNhom = zalo.TenNhom ?? nhomZalo.TenNhom;
            nhomZalo.IdMentor = zalo.IdMentor != null
                ? (await _appDbContext.Users.FindAsync(zalo.IdMentor))?.Id
                : nhomZalo.IdMentor;
            nhomZalo.LinkNhom = zalo.LinkNhom ?? nhomZalo.LinkNhom;
            nhomZalo.LastUpdatedBy = user;
            nhomZalo.LastUpdatedTime = DateTime.Now;

            return await _appDbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteZaloAsync(string id, string user)
        {
            var nhomZalo = await _appDbContext.NhomZalos.Where(x => x.Id == id && x.DeletedBy == null).FirstOrDefaultAsync();

            if (nhomZalo == null)
            {
                throw new Exception();
            }

            nhomZalo.DeletedBy = user;
            nhomZalo.DeletedTime = DateTime.Now;
            //_appDbContext.NhomZalos.Remove(nhomZalo);
            await _appDbContext.SaveChangesAsync();
            return 1;
        }

        // UserNhomZalo methods
        public async Task<List<UserNhomZalo>> GetUsersInGroupAsync(string nhomZaloId)
        {
            return await _appDbContext.UserNhomZalos
                .Where(x => x.IdNhomZalo == nhomZaloId)
                .ToListAsync();
        }

        public async Task<UserNhomZalo?> GetUserInGroupAsync(string nhomZaloId, string userId)
        {
            return await _appDbContext.UserNhomZalos
                .Where(x => x.IdNhomZalo == nhomZaloId && x.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<int> AddUserToGroupAsync(string nhomZaloId, string user, UserNhomZalo addUser)
        {
            var nhomZalo = await _appDbContext.NhomZalos.FindAsync(nhomZaloId);

            if (nhomZalo == null)
            {
                throw new Exception($"NhomZalo with ID {nhomZaloId} not found.");
            }

            addUser.IdNhomZalo = nhomZaloId;
            addUser.JoinedTime = DateTime.Now;
            addUser.CreatedBy = user;
            addUser.LastUpdatedBy = user;
            addUser.LastUpdatedTime = DateTime.Now;

            _appDbContext.UserNhomZalos.Add(addUser);
            return await _appDbContext.SaveChangesAsync();
        }
        public async Task<int> UpdateUserInGroupAsync(string nhomZaloId, string user, UserNhomZalo updatedUser)
        {
            var userNhomZalo = await _appDbContext.UserNhomZalos
                .Where(x => x.IdNhomZalo == nhomZaloId && x.UserId == updatedUser.UserId)
                .FirstOrDefaultAsync();

            if (userNhomZalo == null)
            {
                throw new Exception($"UserNhomZalo with ID {updatedUser.UserId} in NhomZalo with ID {nhomZaloId} not found.");
            }

            userNhomZalo.UserId = updatedUser.UserId;
            userNhomZalo.IdNhomZalo = updatedUser.IdNhomZalo;
            userNhomZalo.JoinedTime = updatedUser.JoinedTime ?? userNhomZalo.JoinedTime; 
            userNhomZalo.LeftTime = updatedUser.LeftTime;
            userNhomZalo.LastUpdatedBy = user;
            userNhomZalo.LastUpdatedTime = DateTime.Now;

            return await _appDbContext.SaveChangesAsync();
        }

        public async Task<int> RemoveUserFromGroupAsync(string nhomZaloId, string user, string userId)
        {
            var userNhomZalo = await _appDbContext.UserNhomZalos
                .Where(x => x.IdNhomZalo == nhomZaloId && x.UserId == userId)
                .FirstOrDefaultAsync();

            if (userNhomZalo == null)
            {
                throw new Exception($"UserNhomZalo with ID {userId} in NhomZalo with ID {nhomZaloId} not found.");
            }

            userNhomZalo.LeftTime = DateTime.Now;
            userNhomZalo.DeletedBy = user;
            userNhomZalo.DeletedTime = DateTime.Now;
            //_appDbContext.UserNhomZalos.Remove(userNhomZalo);
            await _appDbContext.SaveChangesAsync();
            return 1;
        }
    }
}
