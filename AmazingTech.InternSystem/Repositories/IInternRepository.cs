using AmazingTech.InternSystem.Data.Entity;

namespace AmazingTech.InternSystem.Repositories
{
    public interface IInternRepository
    {
        void AddListIntern(List<InternInfo> list, string kiThucTapId);
    }
}