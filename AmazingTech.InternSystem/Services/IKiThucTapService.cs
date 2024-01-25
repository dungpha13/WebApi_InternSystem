using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request.KiThucTap;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Services
{
    public interface IKiThucTapService
    {
        IActionResult GetKiThucTapsByTruong(string idTruong);
        IActionResult GetAllKiThucTaps();
        IActionResult GetKiThucTap(string id);
        IActionResult AddKiThucTap(AddKiThucTapDTO ki);
        IActionResult DeleteKiThucTap(string id);
        IActionResult UpdateKiThucTap(UpdateKiThucTapDTO ki);
    }
}
