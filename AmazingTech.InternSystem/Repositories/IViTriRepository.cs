using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.VItri;

namespace AmazingTech.InternSystem.Repositories
{
    public interface IViTriRepository
    {
        Task<List<ViTri>> GetAllVitri();

        Task<int> CreateViTri(ViTri viTri);

        Task<int> UpdateViTri(string viTriId, ViTri updatedViTri);

        Task<int> DeleteViTri(string viTriId, string user);
        Task<List<InternInfo>> UserViTriView(string id);
    }
}
