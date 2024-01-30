﻿using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models;
using AmazingTech.InternSystem.Models.DTO;

namespace AmazingTech.InternSystem.Services
{
    public interface INhomZaloService
    {
        // ZaloGroup methods
        public Task<List<NhomZalo>> GetAllZaloAsync();
        public Task<NhomZalo?> GetGroupByIdAsync(string id);
        public Task<int> AddNewZaloAsync(string user, NhomZaloDTO zaloDTO);
        public Task<int> UpdateZaloAsync(string id, string user, NhomZaloDTO zaloDTO);
        public Task<int> DeleteZaloAsync(string id, string user);

        // User in ZaloGroup methods
        public Task<List<UserNhomZalo>> GetUsersInGroupAsync(string nhomZaloId);
        public Task<UserNhomZalo?> GetUserInGroupAsync(string nhomZaloId, string userId);
        public Task<int> AddUserToGroupAsync(string nhomZaloId, string user, UserNhomZaloDTO userDTO);
        public Task<int> UpdateUserInGroupAsync(string nhomZaloId, string user, UserNhomZaloDTO updatedUserDTO);
        public Task<int> RemoveUserFromGroupAsync(string nhomZaloId, string user, string userId);
    }
}
