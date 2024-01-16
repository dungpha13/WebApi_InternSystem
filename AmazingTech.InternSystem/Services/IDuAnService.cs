using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models;
using AmazingTech.InternSystem.Models.Request.DuAn;

namespace AmazingTech.InternSystem.Services
{
    public interface IDuAnService
    {
        public Task<List<DuAnModel>> GetAllDuAnsAsync();
        public Task<DuAnModel> GetDuAnByIdAsync(string id);
        public Task CreateDuAnAsync(DuAnModel createDuAn);
        public Task UpdateDuAnAsync(string id, DuAnModel updatedDuAn);
        public Task DeleteDuAnAsync(string id, DuAnModel deleteDuAn);
        public Task<List<DuAnModel>> SearchProjectsAsync(DuAnFilterCriteria criteria);
        //public Task<byte[]> ExportProjectsToExcelAsync(List<string> duAnIds);
    }
}
