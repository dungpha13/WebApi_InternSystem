using AmazingTech.InternSystem.Data.Entity;

namespace AmazingTech.InternSystem.Repositories
{
    public interface IKiThucTapRepository
    {
        KiThucTap? GetKiThucTap(string id);
        List<KiThucTap> GetAllKiThucTaps();
        int AddKiThucTap(KiThucTap kiThucTap);
        int UpdateKiThucTap(KiThucTap kiThucTap);
        Task<int> DeleteKiThucTap(KiThucTap kiThucTap);
    }
}
