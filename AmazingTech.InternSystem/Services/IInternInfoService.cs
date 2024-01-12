using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request.InternInfo;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Services
{
    public interface IInternInfoService
    {
        Task<IActionResult> AddListInternInfo(IFormFile file, string kiThucTapId);
        Task<IActionResult> GetInternInfo(string id);
    }
}
