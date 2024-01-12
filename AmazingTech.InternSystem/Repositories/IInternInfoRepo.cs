using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request;
using AmazingTech.InternSystem.Models.Request.InternInfo;
using AmazingTech.InternSystem.Models.Response;
// using AmazingTech.InternSystem.Models.Response.InternInfo;

namespace AmazingTech.InternSystem.Repositories
{
    public interface IInternInfoRepo
    {
        // public Task<List<InternInfoDTO>> GetAllInternsInfoAsync();
        // public Task<InternInfoDTO> GetInternInfoAsync(string MSSV);

        public Task<int> AddInternInfoAsync(InternInfo entity);

        // public Task UpdateInternInfoAsync(InternInfoDTO model, string MSSV);

        public Task DeleteInternInfoAsync(string MSSV);

        public Task AddListInternInfoAsync(List<InternInfo> list);

        public Task<InternInfo?> GetInternInfo(string id);
    }
}
