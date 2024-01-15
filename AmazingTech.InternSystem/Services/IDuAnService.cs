using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models;
using AmazingTech.InternSystem.Models.Request.DuAn;

namespace AmazingTech.InternSystem.Services
{
    public interface IDuAnService
    {
        public Task<List<DuAn>> SearchProjectsAsync(DuAnFilterCriteria criteria);
        public Task CleanFiltersAsync();
        public Task<string> CreateDuAnAsync(DuAn duAn, List<string> viTriIds, List<string> congNgheIds, List<string> leaderUserIds);
        public Task UpdateDuAnAsync(string id, DuAn updatedDuAn, List<string> viTriIds, List<string> congNgheIds, List<string> leaderUserIds);
        public Task DeleteDuAnAsync(string id);
        public Task<DuAn?> GetDuAnForEditAsync(string id);
        //public Task<byte[]> ExportProjectsToExcelAsync(List<string> duAnIds);
    }
}
