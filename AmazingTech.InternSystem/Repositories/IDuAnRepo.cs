using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request.DuAn;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmazingTech.InternSystem.Repositories
{
    public interface IDuAnRepo
    {
        // DuAn methods
        List<DuAn> GetAllDuAns();
        DuAn GetDuAnById(string id);
        List<DuAnModel> SearchProject(string ten, string leaderName);
        DuAn GetDuAnByName(string projectName);
        int CreateDuAn(DuAn createDuAn);
        int UpdateDuAn(string duAnId, DuAn updatedDuAn);
        int DeleteDuAn(string duAnId);
    }
}
