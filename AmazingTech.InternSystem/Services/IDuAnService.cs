using AmazingTech.InternSystem.Models.Request.DuAn;
using AmazingTech.InternSystem.Models.Request.KiThucTap;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmazingTech.InternSystem.Services
{
    public interface IDuAnService
    {
        IActionResult SearchProject(string ten, string leaderName);
        IActionResult GetAllDuAns();
        IActionResult GetDuAnById(string id);
        IActionResult CreateDuAn(DuAnModel createDuAn);
        IActionResult UpdateDuAn(string id, DuAnModel updatedDuAn);
        IActionResult DeleteDuAn(string id);
    }
}
