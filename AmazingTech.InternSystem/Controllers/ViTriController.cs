using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Models.VItri;
using AmazingTech.InternSystem.Repositories;
using AmazingTech.InternSystem.Service;
using AmazingTech.InternSystem.Services;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [Authorize(Roles = "Admin, HR")]
        public async Task<IActionResult> GetViTriList()
        {
            try
            {
                var position = await _viTriService.GetViTriList();
                return Ok(position);
            }
            catch (Exception ex)
            {
                return BadRequest("Vi Tri is not existed");
            }


        }
        [HttpPost]
        [Authorize(Roles = "Admin, HR")]
        [Route("create")]
        public async Task<IActionResult> AddViTri( [FromBody] VitriModel vitriModel)
        {
            try
            {
                string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                int save = await _viTriService.AddVitri( user, vitriModel);
                return Ok(save == 1 ? "Success" : "failed");
            }
            catch (Exception ex)
            {
                return BadRequest("not existed");
            }


        }
        [HttpPut]
        [Authorize(Roles = "Admin, HR")]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateViTri(string id, [FromBody] VitriModel updateVitri)
        {
            try
            {
                string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                int save = await _viTriService.UpdateVitri(updateVitri, id, user);
                return Ok(save == 1 ? "Success" : "failed");
            }
            catch (Exception ex)
            {
                return BadRequest("not existed");
            }
        }
        [HttpDelete]
        [Authorize(Roles = "Admin, HR")]
        [Route("delete/{id}")]

        public async Task<IActionResult> DeleteViTri(string id)
        {
            try {
                string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                int save = await _viTriService.DeleteVitri(id, user);
                return Ok(save == 1 ? "Success" : "failed");
            }
            catch(Exception ex)
            {
                return BadRequest("not existed");
            }
            }


        [HttpGet]
        [Authorize(Roles = "Admin, HR")]
        [Route("viewVitri/{id}")]
        public async Task<IActionResult> UserViTriView(string id)
        {
            List<VitriUserViewModel> list = new List<VitriUserViewModel>();
            try {
                list = await _viTriService.UserViTriView(id);
                if (list.Count == 0)
                {
                    return BadRequest("can not find");
                }
                return Ok(list);

            }
            catch(Exception ex)
            {
                return BadRequest(" Can't find id or deleted.");
            }
            }
    }
}
