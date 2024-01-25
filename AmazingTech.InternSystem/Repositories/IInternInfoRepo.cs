using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request;
using AmazingTech.InternSystem.Models.Request.InternInfo;
using AmazingTech.InternSystem.Models.Response;
using AmazingTech.InternSystem.Models.Response.InternInfo;

namespace AmazingTech.InternSystem.Repositories
{
    public interface IInternInfoRepo
    {
        public Task<List<InternInfo>> GetAllInternsInfoAsync();
        public Task<InternInfo> GetInternInfoAsync(string MSSV);

        public Task<int> AddInternInfoAsync(string user, InternInfo entity);

        public Task<int> UpdateInternInfoAsync(InternInfo model);

        public Task<int> DeleteInternInfoAsync(InternInfo entity);

        public Task<int> AddListInternInfoAsync(List<InternInfo> interns);

        public Task<InternInfo> GetCommentByMssv(string mssv);
    }
}
