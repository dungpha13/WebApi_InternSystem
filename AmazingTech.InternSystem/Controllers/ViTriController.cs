using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Models.VItri;
using AmazingTech.InternSystem.Repositories;
using AmazingTech.InternSystem.Service;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AmazingTech.InternSystem.Controller
{
    [Route("api/vi-tris")]
    [ApiController]
    public class ViTriController : ControllerBase
    {
        private readonly IViTriService _viTriService;
        private readonly IViTriRepository _viTriRepository;
        private readonly AppDbContext _appDbContext;
        public ViTriController(IViTriService viTriService, IViTriRepository viTriRepository)
        {
            _viTriService = viTriService;
            _viTriRepository = viTriRepository;
        }
        [HttpGet("get")]

        public async Task<IActionResult> GetViTriList()
        {
            var vitri = await _viTriService.GetViTriList();
            return Ok(vitri);
        }
        [HttpPost]

        [Route("create")]
        public async Task<IActionResult> AddViTri([FromBody] VitriModel vitriModel)
        {
            string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int save = await _viTriService.AddVitri(vitriModel, user);
            return Ok(save == 1 ? "Success" : "failed");
        }
        [HttpPut]

        [Route("update/{id}")]
        public async Task<IActionResult> UpdateViTri(string id, [FromBody] VitriModel updateVitri)
        {
            string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int save = await _viTriService.UpdateVitri(updateVitri, id, user);
            return Ok(save == 1 ? "Success" : "failed");
        }
        [HttpDelete]

        [Route("delete/{id}")]

        public async Task<IActionResult> DeleteVitri(string id)
        {
            string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int save = await _viTriService.DeleteVitri(id, user);
            return Ok(save == 1 ? "Success" : "failed");
        }
        [HttpGet]

        [Route("viewVitri/{id}")]
        public async Task<IActionResult> UserViTriView(string id)
        {

            var save = await _viTriService.UserViTriView(id);
            return Ok(save);
        }
    }
}
