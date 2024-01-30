using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request.KiThucTap;
using AmazingTech.InternSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Controllers
{
    [Route("api/ki-thuc-taps")]
    [ApiController]
    [Authorize(Roles = "Admin, School, HR")]
    public class KiThucTapController : ControllerBase
    {
        private readonly IKiThucTapService _kiThucTapService;

        public KiThucTapController(IKiThucTapService kiThucTapService)
        {
            _kiThucTapService = kiThucTapService;
        }

        [HttpGet("get")]
        public IActionResult GetAllKiThucTaps()
        {
            return _kiThucTapService.GetAllKiThucTaps();
        }

        [HttpGet]
        [Route("get-by-truong/{idTruong}")]
        public IActionResult GetKiThucTapsByTruong([FromRoute] string idTruong)
        {
            return _kiThucTapService.GetKiThucTapsByTruong(idTruong);
        }

        [HttpGet("get/{id}")]
        public IActionResult GetKiThucTap(string id)
        {
            return _kiThucTapService.GetKiThucTap(id);
        }

        [HttpPost("create")]
        public IActionResult AddKiThucTap(AddKiThucTapDTO request)
        {
            return _kiThucTapService.AddKiThucTap(request);
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateKiThucTap(UpdateKiThucTapDTO ki, string id)
        {
            return _kiThucTapService.UpdateKiThucTap(ki, id);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteKiThucTap(string id)
        {
            return await _kiThucTapService.DeleteKiThucTap(id);
        }

    }
}
