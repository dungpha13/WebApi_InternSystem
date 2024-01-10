using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Services
{
    public interface IKiThucTapService
    {
        IActionResult GetAllKiThucTaps();
        IActionResult GetKiThucTapsByTruong(string idTruong);
    }
}
