using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.DTO;
using AmazingTech.InternSystem.Services;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace AmazingTech.InternSystem.Controllers
{

    [Route("api/cong-nghes")]
    [ApiController]
    public class TechController : ControllerBase
    {

        private readonly ITechService _service;
        private readonly AppDbContext _context;

        public TechController(IServiceProvider serviceProvider)
        {
            _service = serviceProvider.GetRequiredService<ITechService>();

        }

        [HttpGet]
        [Authorize]
        [Route("get")]
        public async Task<IActionResult> GetAllTech()
        {
            var tech = await _service.getAllTech();
            return Ok(tech);
        }

        [HttpPost]
        [Authorize]
        [Route("create")]
        public async Task<IActionResult> CreateTech([FromBody] TechModel tech)
        {
            string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var save = await _service.CreateTech(tech, user);
             return Ok(save == 1 ? "Success" : "failed");
        }

        [HttpPut]
        [Authorize]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateTech(string id, TechModel tech)
        {
            string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var save = await _service.UpdateTech(user, id, tech);
            return Ok(save == 1 ? "Success" : "failed");
        }

        [HttpDelete]
        [Authorize]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteTech(string id)
        {
            string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var save = await _service.DeleteTech(user, id);
            return Ok(save == 1 ? "Success" : "failed");

        }


    }
}
