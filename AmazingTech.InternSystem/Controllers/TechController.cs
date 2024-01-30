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
        [Authorize(Roles = "Admin, HR")]
        
        [Route("get/{idViTri}")]
        public async Task<IActionResult> GetAllTech(string idViTri)
        {
            var tech = await _service.getAllTech(idViTri);
            return Ok(tech);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, HR")]
        [Route("create/{idViTri}")]
        public async Task<IActionResult> CreateTech(string idViTri, [FromBody] TechModel tech)
        {
            string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var save = await _service.CreateTech(idViTri, tech, user);
             return Ok(save == 1 ? "Success" : "failed or Tech is Existed");
        }

        [HttpPut]
        [Authorize(Roles = "Admin, HR")]
        [Route("update/{idViTri}/{id}")]
        public async Task<IActionResult> UpdateTech(string idViTri,string id, TechModel tech)
        {
            string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var save = await _service.UpdateTech(idViTri,user, id, tech);
            return Ok(save == 1 ? "Success" : "failed or Tech is Existed");
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, HR")]
        [Route("delete/{idViTri}/{id}")]
        public async Task<IActionResult> DeleteTech(string idViTri, string id)
        {
            string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var save = await _service.DeleteTech(idViTri,user, id);
            return Ok(save == 1 ? "Success" : "failed");

        }


    }
}
