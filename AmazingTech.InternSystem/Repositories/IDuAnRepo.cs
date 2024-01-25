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
        List<DuAn> SearchProject(string ten, string leaderId);
        DuAn GetDuAnByName(string projectName);
        int CreateDuAn(string user, DuAn createDuAn);
        int UpdateDuAn(string user, string duAnId, DuAn updatedDuAn);
        int DeleteDuAn(string user, string duAnId);
    }
}
