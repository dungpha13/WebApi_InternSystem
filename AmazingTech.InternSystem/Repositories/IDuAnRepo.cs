using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request.DuAn;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmazingTech.InternSystem.Repositories
{
    public interface IDuAnRepo
    {
        // DuAn methods
        List<DuAn> GetAllDuAns();
        DuAn GetDuAnById(string id);
        List<DuAnModel> SearchProject(string ten, string leaderName, DateTime? startDate, DateTime? endDate);
        DuAn GetDuAnByName(string projectName);
        int CreateDuAn(string user, DuAn createDuAn);
        int UpdateDuAn(string duAnId, string user, DuAn updatedDuAn);
        int DeleteDuAn(string duAnId, string user);
    }
}
