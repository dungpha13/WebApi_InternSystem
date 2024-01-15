using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request.KiThucTap;
using AmazingTech.InternSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Controllers
{
    [Route("api/kithuctaps")]
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
        [Route("truong/{idTruong}")]
        public IActionResult GetKiThucTapsByTruong([FromRoute] string idTruong)
        {
            return _kiThucTapService.GetKiThucTapsByTruong(idTruong);
        }

        [HttpGet("{id}")]
        public IActionResult GetKiThucTap(string id)
        {
            return _kiThucTapService.GetKiThucTap(id);
        }

        [HttpPost]
        public IActionResult AddKiThucTap(AddKiThucTapDTO request)
        {
            return _kiThucTapService.AddKiThucTap(request);
        }

        [HttpPost("update")]
        public IActionResult UpdateKiThucTap(UpdateKiThucTapDTO ki)
        {
            return _kiThucTapService.UpdateKiThucTap(ki);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteKiThucTap(string id)
        {
            return _kiThucTapService.DeleteKiThucTap(id);
        }

    }
}
