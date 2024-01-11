using AmazingTech.InternSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;

namespace AmazingTech.InternSystem.Controllers
{
    [Route("api/truong")]
    [ApiController]
    public class TruongController : ControllerBase
    {
        private readonly ITruongService _truongService;

        public TruongController(ITruongService truongService)
        {
            _truongService = truongService;
        }

        [HttpGet]
        public IActionResult GetAllTruongs()
        {
            return _truongService.GetAllTruongs();
        }
    }
}
