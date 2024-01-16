using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models;
using AmazingTech.InternSystem.Models.Request.DuAn;
using AmazingTech.InternSystem.Repositories;
using AutoMapper;
using Serilog;

namespace AmazingTech.InternSystem.Services
{
    public class DuAnService : IDuAnService
    {
        private readonly IDuAnRepo _duAnRepo;
        private readonly IMapper _mapper;

        public DuAnService(IDuAnRepo duAnRepo, IMapper mapper)
        {
            _duAnRepo = duAnRepo;
            _mapper = mapper;
        }

        public async Task<List<DuAnModel>> GetAllDuAnsAsync()
        {
            try
            {
                var duAns = await _duAnRepo.GetAllDuAnsAsync();
                return _mapper.Map<List<DuAnModel>>(duAns);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in DuAnService.GetAllDuAnsAsync");
                throw;
            }
        }

        public async Task<DuAnModel> GetDuAnByIdAsync(string id)
        {
            try
            {
                var duAn = await _duAnRepo.GetDuAnByIdAsync(id);
                return _mapper.Map<DuAnModel>(duAn);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error in DuAnService.GetDuAnByIdAsync for ID: {id}");
                throw;
            }
        }

        public async Task<List<DuAnModel>> SearchProjectsAsync(DuAnFilterCriteria criteria)
        {
            try
            {
                var duAns = await _duAnRepo.SearchProjectsAsync(criteria);
                return _mapper.Map<List<DuAnModel>>(duAns);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in DuAnService.SearchProjectsAsync");
                throw;
            }
        }

        public async Task CreateDuAnAsync(DuAnModel createDuAn)
        {
            try
            {
                DuAn duAn = _mapper.Map<DuAn>(createDuAn);
                await _duAnRepo.CreateDuAnAsync(duAn);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in DuAnService.CreateDuAnAsync");
                throw;
            }
        }

        public async Task UpdateDuAnAsync(string id, DuAnModel updatedDuAn)
        {
            try
            {
                DuAn duAn = _mapper.Map<DuAn>(updatedDuAn);
                await _duAnRepo.UpdateDuAnAsync(id, duAn);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in DuAnService.UpdateDuAnAsync");
                throw;
            }
        }

        public async Task DeleteDuAnAsync(string id, DuAnModel deleteDuAn)
        {
            try
            {
                DuAn duAn = _mapper.Map<DuAn>(deleteDuAn);
                await _duAnRepo.DeleteDuAnAsync(id, duAn);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in DuAnService.DeleteDuAnAsync");
                throw;
            }
        }

        //public Task<byte[]> ExportProjectsToExcelAsync(List<string> duAnIds)
        //{
        //    return _duAnRepo.ExportProjectsToExcelAsync(duAnIds);
        //}
    }
}
