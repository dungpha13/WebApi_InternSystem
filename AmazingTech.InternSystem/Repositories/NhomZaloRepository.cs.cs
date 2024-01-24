using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models;
using Microsoft.EntityFrameworkCore;

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
            return await _appDbContext.NhomZalos.ToListAsync();
        }

        public async Task<NhomZalo?> GetGroupByIdAsync(string id)
        {
            return await _appDbContext.NhomZalos.FindAsync(id);
        }

        public async Task AddNewZaloAsync(NhomZalo zalo)
        {
            var checkMentor = await _appDbContext.Users
                .Where(x => x.Id == zalo.IdMentor && x.Roles.Equals("Mentor"))
                .FirstOrDefaultAsync();

            var zaloAdd = new NhomZalo()
            {
                Id = Guid.NewGuid().ToString("N"),
                TenNhom = zalo.TenNhom,
                CreatedBy = zalo.CreatedBy,
                IdMentor = checkMentor?.Id,
                LinkNhom = zalo.LinkNhom,
                LastUpdatedBy = zalo.CreatedBy,
                LastUpdatedTime = DateTime.Now
            };

            await _appDbContext.NhomZalos.AddAsync(zaloAdd);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateZaloAsync(string id, NhomZalo zalo)
        {
            var nhomZalo = await _appDbContext.NhomZalos.FindAsync(id);

            if (nhomZalo == null)
            {
                _logger.LogError($"NhomZalo with ID {id} not found.");
                return;
            }

            nhomZalo.TenNhom = zalo.TenNhom ?? nhomZalo.TenNhom;
            nhomZalo.IdMentor = zalo.IdMentor != null
                ? (await _appDbContext.Users.FindAsync(zalo.IdMentor))?.Id
                : nhomZalo.IdMentor;
            nhomZalo.LinkNhom = zalo.LinkNhom ?? nhomZalo.LinkNhom;
            nhomZalo.LastUpdatedBy = zalo.LastUpdatedBy;
            nhomZalo.LastUpdatedTime = DateTime.Now;

            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteZaloAsync(string id)
        {
            var nhomZalo = await _appDbContext.NhomZalos.FindAsync(id);

            if (nhomZalo == null)
            {
                _logger.LogError($"NhomZalo with ID {id} not found.");
                return;
            }

            _appDbContext.NhomZalos.Remove(nhomZalo);
            nhomZalo.DeletedTime = DateTime.Now;
            await _appDbContext.SaveChangesAsync();
        }

        // UserNhomZalo methods
        public async Task<List<UserNhomZalo>> GetUsersInGroupAsync(string nhomZaloId)
        {
            return await _appDbContext.UserNhomZalos
                .Where(x => x.IdNhomZalo == nhomZaloId)
                .ToListAsync();
        }

        public async Task AddUserToGroupAsync(string nhomZaloId, UserNhomZalo user)
        {
            var nhomZalo = await _appDbContext.NhomZalos.FindAsync(nhomZaloId);

            if (nhomZalo == null)
            {
                _logger.LogError($"NhomZalo with ID {nhomZaloId} not found.");
                return;
            }

            var userNhomZalo = new UserNhomZalo()
            {
                UserId = user.UserId,
                IdNhomZalo = nhomZaloId,
                JoinedTime = DateTime.Now,
            };

            await _appDbContext.UserNhomZalos.AddAsync(userNhomZalo);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<UserNhomZalo?> GetUserInGroupAsync(string nhomZaloId, string userId)
        {
            return await _appDbContext.UserNhomZalos
                .Where(x => x.IdNhomZalo == nhomZaloId && x.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateUserInGroupAsync(string nhomZaloId, UserNhomZalo updatedUser)
        {
            var userNhomZalo = await _appDbContext.UserNhomZalos
                .Where(x => x.IdNhomZalo == nhomZaloId && x.UserId == updatedUser.UserId)
                .FirstOrDefaultAsync();

            if (userNhomZalo == null)
            {
                _logger.LogError($"UserNhomZalo with ID {updatedUser.UserId} in NhomZalo with ID {nhomZaloId} not found.");
                return;
            }

            userNhomZalo.UserId = updatedUser.UserId;
            userNhomZalo.IdNhomZalo = updatedUser.IdNhomZalo;
            userNhomZalo.JoinedTime = updatedUser.JoinedTime;
            userNhomZalo.LeftTime = updatedUser.LeftTime;

            await _appDbContext.SaveChangesAsync();
        }

        public async Task RemoveUserFromGroupAsync(string nhomZaloId, string userId)
        {
            var userNhomZalo = await _appDbContext.UserNhomZalos
                .Where(x => x.IdNhomZalo == nhomZaloId && x.UserId == userId)
                .FirstOrDefaultAsync();

            if (userNhomZalo == null)
            {
                _logger.LogError($"UserNhomZalo with ID {userId} in NhomZalo with ID {nhomZaloId} not found.");
                return;
            }

            _appDbContext.UserNhomZalos.Remove(userNhomZalo);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
