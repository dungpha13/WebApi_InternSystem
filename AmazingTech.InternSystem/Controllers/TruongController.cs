// using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request.TruongHoc;
using AmazingTech.InternSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;

namespace AmazingTech.InternSystem.Controllers
{
    [Route("api/truongs")]
    [ApiController]
    // [Authorize(Roles = "School, HR, Admin")]
    public class TruongController : ControllerBase
    {
        private readonly ITruongService _truongService;

        public TruongController(ITruongService truongService)
        {
            _truongService = truongService;
        }

        [HttpGet("get")]
        public IActionResult GetAllTruongs()
        {
            return _truongService.GetAllTruongs();
        }

        [HttpGet("get/{id}")]
        public IActionResult GetTruong(string id)
        {
            return _truongService.GetTruong(id);
        }

        [HttpPost("create")]
        public IActionResult AddTruong(AddTruongHocDTO truong)
        {
            return _truongService.AddTruong(truong);
        }

        [HttpPatch("update")]
        public IActionResult UpdateTruong(UpdateTruongHocDTO truong)
        {
            return _truongService.UpdateTruong(truong);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTruong(string id)
        {
            return await _truongService.DeleteTruong(id);
        }
    }
}
