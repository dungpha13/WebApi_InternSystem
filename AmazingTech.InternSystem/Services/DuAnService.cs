using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models;
using AmazingTech.InternSystem.Models.Request.DuAn;
using AmazingTech.InternSystem.Repositories;

namespace AmazingTech.InternSystem.Services
{
    public class DuAnService : IDuAnService
    {
        private readonly IDuAnRepo _duAnRepo;

        public DuAnService(IDuAnRepo duAnRepo)
        {
            _duAnRepo = duAnRepo;
        }

        public Task<List<DuAn>> SearchProjectsAsync(DuAnFilterCriteria criteria)
        {
            return _duAnRepo.SearchProjectsAsync(criteria);
        }

        public Task CleanFiltersAsync()
        {
            return _duAnRepo.CleanFiltersAsync();
        }

        public Task<string> CreateDuAnAsync(DuAn duAn, List<string> viTriIds, List<string> congNgheIds, List<string> leaderUserIds)
        {
            return _duAnRepo.CreateDuAnAsync(duAn, viTriIds, congNgheIds, leaderUserIds);
        }

        public Task UpdateDuAnAsync(string id, DuAn updatedDuAn, List<string> viTriIds, List<string> congNgheIds, List<string> leaderUserIds)
        {
            return _duAnRepo.UpdateDuAnAsync(id, updatedDuAn, viTriIds, congNgheIds, leaderUserIds);
        }

        public Task DeleteDuAnAsync(string id)
        {
            return _duAnRepo.DeleteDuAnAsync(id);
        }

        public Task<DuAn?> GetDuAnForEditAsync(string id)
        {
            return _duAnRepo.GetDuAnForEditAsync(id);
        }

        //public Task<byte[]> ExportProjectsToExcelAsync(List<string> duAnIds)
        //{
        //    return _duAnRepo.ExportProjectsToExcelAsync(duAnIds);
        //}
    }
}
