using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request.InternInfo;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Services
{
    public interface IInternInfoService
    {
        Task<IActionResult> AddInternInfo(AddInternInfoDTO model);
        Task<IActionResult> GetAllInternInfo();
        Task<IActionResult> GetInternInfo(string mssv);

        Task<IActionResult> DeleteInternInfo(string mssv);

        Task<IActionResult> UpdateInternInfo(UpdateInternInfoDTO model, string mssv);
    }
}
