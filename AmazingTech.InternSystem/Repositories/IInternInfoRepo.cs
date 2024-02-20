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
        public int GetInternSendCVInPrecious(int year, int precious);
        public int GetInternSendCVInAYear(int year);
        public Task<List<InternInfo>> GetAllInternsInfoAsync();
        public Task<List<InternInfo>> GetAllDeletedInternsInfoAsync();
        public Task<InternInfo> GetInternInfoAsync(string MSSV);
        public Task<InternInfo> GetDeletedInternInfoAsync(string MSSV);

        public Task<int> AddInternInfoAsync(string userId, InternInfo entity);

        public Task<int> UpdateInternInfoAsync(string userId, InternInfo model);

        public Task<int> DeleteInternInfoAsync(string userId, InternInfo entity);

        public Task<int> AddListInternInfoAsync(List<InternInfo> interns);

        public Task<InternInfo> GetCommentByMssv(string mssv);
    }
}
