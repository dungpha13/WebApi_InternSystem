using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request.InternInfo;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Services
{
    public interface IInternInfoService
    {
        Task<IActionResult> AddInternInfo(string user, AddInternInfoDTO model);
        Task<IActionResult> GetAllInternInfo();
        Task<IActionResult> GetInternInfo(string mssv);
        Task<IActionResult> DeleteInternInfo(string mssv);
        Task<IActionResult> UpdateInternInfo(UpdateInternInfoDTO model, string mssv);
        Task<IActionResult> AddListInternInfo(IFormFile file, string kiThucTapId);

        Task<IActionResult> AddCommentInternInfo(CommentInternInfoDTO comment, string idCommentor, string mssv);
        Task<IActionResult> GetCommentsByMssv(string mssv);
    }
}
