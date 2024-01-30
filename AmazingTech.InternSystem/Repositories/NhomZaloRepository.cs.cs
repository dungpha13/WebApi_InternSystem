﻿using AmazingTech.InternSystem.Data;
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
            return await _appDbContext.NhomZalos.ToListAsync();
        }

        public async Task<NhomZalo?> GetGroupByIdAsync(string id)
        {
            var groupZalo = await _appDbContext.NhomZalos.Where(x => x.Id == id && x.DeletedBy == null).FirstOrDefaultAsync();
            if (groupZalo == null)
            {
                throw new Exception();
            }
            return groupZalo;
        }

        public async Task AddNewZaloAsync(NhomZalo zalo)
        {
            await _appDbContext.NhomZalos.AddAsync(zalo);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateZaloAsync(string id, NhomZalo zalo)
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
            nhomZalo.LastUpdatedBy = zalo.LastUpdatedBy;
            nhomZalo.LastUpdatedTime = DateTime.Now;

            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteZaloAsync(string id)
        {
            var nhomZalo = await _appDbContext.NhomZalos.Where(x => x.Id == id && x.DeletedBy == null).FirstOrDefaultAsync();

            if (nhomZalo == null)
            {
                throw new Exception();
            }

            nhomZalo.DeletedTime = DateTime.Now;
            //_appDbContext.NhomZalos.Remove(nhomZalo);
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

            user.IdNhomZalo = nhomZaloId;
            user.JoinedTime = DateTime.Now;

            await _appDbContext.UserNhomZalos.AddAsync(user);
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
            userNhomZalo.JoinedTime = updatedUser.JoinedTime ?? userNhomZalo.JoinedTime; 
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

            userNhomZalo.LeftTime = DateTime.Now;
            //_appDbContext.UserNhomZalos.Remove(userNhomZalo);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
