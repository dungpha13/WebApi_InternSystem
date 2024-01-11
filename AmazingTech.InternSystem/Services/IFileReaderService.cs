using AmazingTech.InternSystem.Data.Entity;

namespace AmazingTech.InternSystem.Services
{
    public interface IFileReaderService
    {
        List<InternInfo> ReadFile(IFormFile file);
    }
}