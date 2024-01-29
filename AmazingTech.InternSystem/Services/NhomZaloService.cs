using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models;
using AmazingTech.InternSystem.Models.DTO;
using AmazingTech.InternSystem.Repositories.NhomZaloManagement;
using System.Collections.Generic;

namespace AmazingTech.InternSystem.Services
{
    public class NhomZaloService : INhomZaloService
    {
        private readonly INhomZaloRepo _nhomZaloRepo;
        private readonly ILogger<NhomZaloService> _logger;

        public NhomZaloService(INhomZaloRepo nhomZaloRepo, ILogger<NhomZaloService> logger)
        {
            _nhomZaloRepo = nhomZaloRepo;
            _logger = logger;
        }

        // ZaloGroup methods
        public async Task<List<NhomZalo>> GetAllZaloAsync()
        {
            return await _nhomZaloRepo.GetAllZaloAsync();
        }

        public async Task<NhomZalo?> GetGroupByIdAsync(string id)
        {
            return await _nhomZaloRepo.GetGroupByIdAsync(id);
        }

        public async Task AddNewZaloAsync(NhomZaloDTO zaloDTO)
        {
            var zalo = new NhomZalo
            {
                TenNhom = zaloDTO.TenNhom,
                LinkNhom = zaloDTO.LinkNhom,
                IdMentor = zaloDTO.IdMentor
            };

            await _nhomZaloRepo.AddNewZaloAsync(zalo);
        }

        public async Task UpdateZaloAsync(string id, NhomZaloDTO zaloDTO)
        {
            var zalo = new NhomZalo
            {
                Id = id,
                TenNhom = zaloDTO.TenNhom,
                LinkNhom = zaloDTO.LinkNhom,
                IdMentor = zaloDTO.IdMentor
            };

            await _nhomZaloRepo.UpdateZaloAsync(id, zalo);
        }

        public async Task DeleteZaloAsync(string id)
        {
            await _nhomZaloRepo.DeleteZaloAsync(id);
        }


        // User in ZaloGroup methods
        public async Task<List<UserNhomZalo>> GetUsersInGroupAsync(string nhomZaloId)
        {
            return await _nhomZaloRepo.GetUsersInGroupAsync(nhomZaloId);
        }

        public async Task AddUserToGroupAsync(string nhomZaloId, UserNhomZaloDTO userDTO)
        {
            var user = new UserNhomZalo
            {
                UserId = userDTO.UserId,
                IdNhomZalo = nhomZaloId,
                JoinedTime = userDTO.JoinedTime
            };

            await _nhomZaloRepo.AddUserToGroupAsync(nhomZaloId, user);
        }

        public async Task<UserNhomZalo?> GetUserInGroupAsync(string nhomZaloId, string userId)
        {
            try
            {
                return await _nhomZaloRepo.GetUserInGroupAsync(nhomZaloId, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetUserInGroupAsync: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateUserInGroupAsync(string nhomZaloId, UserNhomZaloDTO updatedUserDTO)
        {
            var updatedUser = new UserNhomZalo
            {
                UserId = updatedUserDTO.UserId,
                IdNhomZalo = nhomZaloId,
                JoinedTime = updatedUserDTO.JoinedTime,
                LeftTime = updatedUserDTO.LeftTime
            };

            await _nhomZaloRepo.UpdateUserInGroupAsync(nhomZaloId, updatedUser);
        }

        public async Task RemoveUserFromGroupAsync(string nhomZaloId, string userId)
        {
            await _nhomZaloRepo.RemoveUserFromGroupAsync(nhomZaloId, userId);
        }
    }
}
