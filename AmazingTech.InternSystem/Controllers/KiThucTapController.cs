using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request.KiThucTap;
using AmazingTech.InternSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Controllers
{
    [Route("api/ki-thuc-taps")]
    [ApiController]
    public class KiThucTapController : ControllerBase
    {
        private readonly IKiThucTapService _kiThucTapService;

        public KiThucTapController(IKiThucTapService kiThucTapService)
        {
            _kiThucTapService = kiThucTapService;
        }

        [HttpGet]
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

        [HttpPost("update/{id}")]
        public IActionResult UpdateKiThucTap(UpdateKiThucTapDTO ki)
        {
            return _kiThucTapService.UpdateKiThucTap(ki);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteKiThucTap(string id)
        {
            return _kiThucTapService.DeleteKiThucTap(id);
        }

    }
}
