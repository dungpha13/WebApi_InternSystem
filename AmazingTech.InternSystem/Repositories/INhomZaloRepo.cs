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
        public Task<int> AddNewZaloAsync(string user, NhomZalo zalo);
        public Task<int> UpdateZaloAsync(string id, string user, NhomZalo zalo);
        public Task<int> DeleteZaloAsync(string id, string user);


        // UserNhomZalo methods
         public Task<List<UserNhomZalo>> GetUsersInGroupAsync(string nhomZaloId);
         public Task<UserNhomZalo?> GetUserInGroupAsync(string nhomZaloId, string userId);
         public Task<int> AddUserToGroupAsync(string nhomZaloId, string user, UserNhomZalo addUser);
         public Task<int> UpdateUserInGroupAsync(string nhomZaloId, string user, UserNhomZalo updatedUser);
         public Task<int> RemoveUserFromGroupAsync(string nhomZaloId, string user, string userId);
    }
}
