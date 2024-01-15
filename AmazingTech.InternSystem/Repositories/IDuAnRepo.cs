using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models;
using AmazingTech.InternSystem.Models.Request.DuAn;

namespace AmazingTech.InternSystem.Repositories
{
    public interface IDuAnRepo
    {
        // DuAn methods

        //Searches for projects based on the specified criteria
        public Task<List<DuAn>> SearchProjectsAsync(DuAnFilterCriteria criteria);
        // Cleans the applied filters for project search
        public Task CleanFiltersAsync();
        // Creates a new project with the specified details
        public Task<string> CreateDuAnAsync(DuAn duAn, List<string> viTriIds, List<string> congNgheIds, List<string> leaderUserIds);
        public Task UpdateDuAnAsync(string id, DuAn updatedDuAn, List<string> viTriIds, List<string> congNgheIds, List<string> leaderUserIds);
        public Task DeleteDuAnAsync(string id);
        public Task<DuAn?> GetDuAnForEditAsync(string id);
        //public Task<byte[]> ExportProjectsToExcelAsync(List<string> duAnIds);

        //// DuAn in ZaloGroup methods
        //public Task<List<DuAnModel>> GetProjectsInGroupAsync(string nhomZaloId);
        //public Task AddProjectToGroupAsync(string nhomZaloId, DuAnModel duAnModel);
        //public Task<DuAnModel?> GetProjectInGroupAsync(string nhomZaloId, string duAnId);
        //public Task UpdateProjectInGroupAsync(string nhomZaloId, DuAnModel updatedDuAn);
        //public Task RemoveProjectFromGroupAsync(string nhomZaloId, string duAnId);

        //// User in DuAn methods
        //public Task<List<UserDuAn>> GetUsersInProjectAsync(string duAnId);
        //public Task AddUserToProjectAsync(string duAnId, string userId, string viTriId);
        //public Task<UserDuAn?> GetUserInProjectAsync(string duAnId, string userId);
        //public Task UpdateUserInProjectAsync(string duAnId, UserDuAn updatedUserDuAn);
        //public Task RemoveUserFromProjectAsync(string duAnId, string userId);
    }
}
