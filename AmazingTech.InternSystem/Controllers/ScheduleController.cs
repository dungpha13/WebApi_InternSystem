using AmazingTech.InternSystem.Models;
using AmazingTech.InternSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Controllers
{

    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IGuiLichPhongVanService _guiLichPhongVanService;
        public ScheduleController(IGuiLichPhongVanService guiLichPhongVanService)
        {
            _guiLichPhongVanService = guiLichPhongVanService;
        }
        [HttpPost]
        [Route("api/[controller]/Send-Interview-Schedule")]
        public IActionResult SendInterviewSchedule(LichPhongVanRequestModel model)
        {
             _guiLichPhongVanService.AddLichPhongVan(model);
            return Ok("Send Successful");
        }
    }
}
