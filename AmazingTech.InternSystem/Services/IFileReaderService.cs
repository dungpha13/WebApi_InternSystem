using AmazingTech.InternSystem.Data.Entity;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Services
{
    public interface IFileReaderService
    {
        // List<InternInfo> ReadFile(IFormFile file);
        IActionResult ReadFile(IFormFile file, string kiThucTapId);
    }
}