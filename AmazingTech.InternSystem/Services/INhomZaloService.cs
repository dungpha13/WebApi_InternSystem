using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models;
using AmazingTech.InternSystem.Models.DTO;

namespace AmazingTech.InternSystem.Services
{
    public interface INhomZaloService
    {
        // ZaloGroup methods
        public Task<List<NhomZalo>> GetAllZaloAsync();
        public Task<NhomZalo?> GetGroupByIdAsync(string id);
        public Task AddNewZaloAsync(NhomZaloDTO zaloDTO);
        public Task UpdateZaloAsync(string id, NhomZaloDTO zaloDTO);
        public Task DeleteZaloAsync(string id);

        // User in ZaloGroup methods
        public Task<List<UserNhomZalo>> GetUsersInGroupAsync(string nhomZaloId);
        public Task AddUserToGroupAsync(string nhomZaloId, UserNhomZaloDTO userDTO);
        public Task<UserNhomZalo?> GetUserInGroupAsync(string nhomZaloId, string userId);
        public Task UpdateUserInGroupAsync(string nhomZaloId, UserNhomZaloDTO updatedUserDTO);
        public Task RemoveUserFromGroupAsync(string nhomZaloId, string userId);
    }
}
