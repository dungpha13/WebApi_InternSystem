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
        [Route("{idTruong}")]
        public IActionResult GetKiThucTapsByTruong([FromRoute]string idTruong)
        {
            return _kiThucTapService.GetKiThucTapsByTruong(idTruong);
        }
    }
}
