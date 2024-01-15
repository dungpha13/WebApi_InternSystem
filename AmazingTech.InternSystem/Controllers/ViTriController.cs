using AmazingTech.InternSystem.Service;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Controller
{
    [Route("api/viTris")]
    [ApiController]
    public class ViTriController : ControllerBase
    {
        private readonly IViTriService _viTriService;
        public ViTriController(IViTriService viTriService)
        {
            _viTriService = viTriService;
        }
        [HttpGet]
        public IActionResult GetViTriList()
        {
            return Ok(_viTriService.GetViTriList()) ;
        }
    }
}
