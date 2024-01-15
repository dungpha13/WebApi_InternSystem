using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models;
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
            try
            {
                return await _nhomZaloRepo.GetAllZaloAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllZaloAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<NhomZalo?> GetGroupByIdAsync(string id)
        {
            try
            {
                return await _nhomZaloRepo.GetGroupByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetGroupByIdAsync: {ex.Message}");
                throw;
            }
        }

        public async Task AddNewZaloAsync(NhomZalo zalo)
        {
            try
            {
                await _nhomZaloRepo.AddNewZaloAsync(zalo);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AddNewZaloAsync: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateZaloAsync(string id, NhomZalo zalo)
        {
            try
            {
                await _nhomZaloRepo.UpdateZaloAsync(id, zalo);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateZaloAsync: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteZaloAsync(string id)
        {
            try
            {
                await _nhomZaloRepo.DeleteZaloAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteZaloAsync: {ex.Message}");
                throw;
            }
        }


        // User in ZaloGroup methods
        public async Task<List<UserNhomZalo>> GetUsersInGroupAsync(string nhomZaloId)
        {
            try
            {
                return await _nhomZaloRepo.GetUsersInGroupAsync(nhomZaloId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetUsersInGroupAsync: {ex.Message}");
                throw;
            }
        }

        public async Task AddUserToGroupAsync(string nhomZaloId, UserNhomZalo user)
        {
            try
            {
                await _nhomZaloRepo.AddUserToGroupAsync(nhomZaloId, user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AddUserToGroupAsync: {ex.Message}");
                throw;
            }
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

        public async Task UpdateUserInGroupAsync(string nhomZaloId, UserNhomZalo updatedUser)
        {
            try
            {
                await _nhomZaloRepo.UpdateUserInGroupAsync(nhomZaloId, updatedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateUserInGroupAsync: {ex.Message}");
                throw;
            }
        }

        public async Task RemoveUserFromGroupAsync(string nhomZaloId, string userId)
        {
            try
            {
                await _nhomZaloRepo.RemoveUserFromGroupAsync(nhomZaloId, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in RemoveUserFromGroupAsync: {ex.Message}");
                throw;
            }
        }
    }
}
