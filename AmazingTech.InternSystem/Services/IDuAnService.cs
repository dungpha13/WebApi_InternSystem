using AmazingTech.InternSystem.Models.Request.DuAn;
using AmazingTech.InternSystem.Models.Request.KiThucTap;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmazingTech.InternSystem.Services
{
    public interface IDuAnService
    {
        IActionResult SearchProject(string ten, string leaderName, DateTime? startDate, DateTime? endDate);
        IActionResult GetAllDuAns();
        IActionResult GetDuAnById(string id);
        IActionResult CreateDuAn(string user, DuAnModel createDuAn);
        IActionResult UpdateDuAn(string id, string user, DuAnModel updatedDuAn);
        IActionResult DeleteDuAn(string id, string user);
    }
}
