using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request.DuAn;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmazingTech.InternSystem.Repositories
{
    public interface IDuAnRepo
    {
        // DuAn methods
        List<DuAn> GetAllDuAns();
        DuAn GetDuAnById(string id);
        List<DuAn> SearchProject(DuAnFilterCriteria criteria);
        int CreateDuAn(DuAn createDuAn);
        int UpdateDuAn(DuAn updatedDuAn);
        int DeleteDuAn(DuAn deleteDuAn);
        // Task<byte[]> ExportProjectsToExcelAsync(List<string> duAnIds);

        //// DuAn in ZaloGroup methods
        // Task<List<DuAnModel>> GetProjectsInGroupAsync(string nhomZaloId);
        // Task AddProjectToGroupAsync(string nhomZaloId, DuAnModel duAnModel);
        // Task<DuAnModel?> GetProjectInGroupAsync(string nhomZaloId, string duAnId);
        // Task UpdateProjectInGroupAsync(string nhomZaloId, DuAnModel updatedDuAn);
        // Task RemoveProjectFromGroupAsync(string nhomZaloId, string duAnId);

        //// User in DuAn methods
        // Task<List<UserDuAn>> GetUsersInProjectAsync(string duAnId);
        // Task AddUserToProjectAsync(string duAnId, string userId, string viTriId);
        // Task<UserDuAn?> GetUserInProjectAsync(string duAnId, string userId);
        // Task UpdateUserInProjectAsync(string duAnId, UserDuAn updatedUserDuAn);
        // Task RemoveUserFromProjectAsync(string duAnId, string userId);
    }
}
