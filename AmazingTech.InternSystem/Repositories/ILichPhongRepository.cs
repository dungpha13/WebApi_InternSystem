using AmazingTech.InternSystem.Data.Entity;

namespace AmazingTech.InternSystem.Repositories
{
    public interface ILichPhongRepository
    {
        LichPhongVan GetLichPhong(string id);
        List<LichPhongVan> GetAllLichPhong();

        int AddLichPhong(LichPhongVan lich);
        int UpdateLichPhong(LichPhongVan lich);
        int DeleteLichPhong(string id);

    }
}
