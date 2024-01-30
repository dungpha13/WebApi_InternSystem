using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Repositories.NhomZaloManagement
{
    public interface INhomZaloRepo
    {
        // Zalo methods
        public Task<List<NhomZalo>> GetAllZaloAsync();
        public Task<NhomZalo?> GetGroupByIdAsync(string id);
        public Task AddNewZaloAsync(NhomZalo zalo);
        public Task UpdateZaloAsync(string id, NhomZalo zalo);
        public Task DeleteZaloAsync(string id);


        // UserNhomZalo methods
        public Task<List<UserNhomZalo>> GetUsersInGroupAsync(string nhomZaloId);
        public Task AddUserToGroupAsync(string nhomZaloId, UserNhomZalo user);
        public Task<UserNhomZalo?> GetUserInGroupAsync(string nhomZaloId, string userId);
        public Task UpdateUserInGroupAsync(string nhomZaloId, UserNhomZalo updatedUser);
        public Task RemoveUserFromGroupAsync(string nhomZaloId, string userId);
    }
}
