using AmazingTech.InternSystem.Data.Entity;

namespace AmazingTech.InternSystem.Repositories
{
    public interface IViTriRepository 
    {
        Task<List<ViTri>> GetAllViTriAsync();
        Task<ViTri> GetViTriByIdAsync(string vitriId);
        Task CreateViTriAsync(ViTri viTri);
        Task UpdateViTriAsync(string viTriId, ViTri updatedViTri);

        Task DeleteViTriAsync(string viTriId, ViTri viTridelete);
    }
}
