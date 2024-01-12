using AmazingTech.InternSystem.Models.Request.InternInfo;
using AmazingTech.InternSystem.Repositories;
using AmazingTech.InternSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Controllers
{
    [Route("api/interns")]
    [ApiController]
    public class InternInfoController : ControllerBase
    {
        private readonly IInternInfoRepo _internRepo;
        private readonly IInternInfoService _internInfoService;

        public InternInfoController(IInternInfoRepo internRepo, IInternInfoService internInfoService)
        {
            _internRepo = internRepo;
            _internInfoService = internInfoService;
        }

        [HttpPost("list/{kiThucTapId}")]
        public async Task<IActionResult> AddListInternInfo(IFormFile file, string kiThucTapId)
        {
            return await _internInfoService.AddListInternInfo(file, kiThucTapId);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInternInfo(string id)
        {
            return await _internInfoService.GetInternInfo(id);
        }

        // [HttpGet("")]
        // public async Task<IActionResult> GetAllInternsInfo()
        // {
        //     try
        //     {
        //         return Ok(await internRepo.GetAllInternsInfoAsync());
        //     }
        //     catch
        //     {
        //         return BadRequest();
        //     }
        // }

        // [HttpGet("{mssv}")]
        // public async Task<IActionResult> GetInternInfo([FromRoute]string mssv)
        // {
        //     try
        //     {
        //         var intern = await internRepo.GetInternInfoAsync(mssv);
        //         return Ok(intern);
        //     }
        //     catch
        //     {
        //         return BadRequest();
        //     }
        // }

        // [HttpPost("")]
        // public async Task<IActionResult> AddNewInternInfo(AddInternInfoDTO model)
        // {

        //     return await internInfoService.AddInternInfo(model); //Return MSSV Intern
        // }

        // [HttpDelete("{mssv}")]
        // public async Task<IActionResult> DeleteInternInfo(string mssv)
        // {
        //     try
        //     {
        //         var intern = await internRepo.GetInternInfoAsync(mssv);
        //         if (intern == null)
        //         {
        //             return NotFound("Intern does not exist!");
        //         }

        //         await internRepo.DeleteInternInfoAsync(mssv);
        //         return Ok("Delete Successfully!");
        //     }
        //     catch
        //     {
        //         return BadRequest();
        //     }
        // }

        // [HttpPut("{mssv}")]
        // public async Task<IActionResult> UpdateInternInfo(InternInfoDTO model, string mssv)
        // {
        //     if (mssv != model.MSSV)
        //     {
        //         return NotFound("Intern does not exist!");
        //     }

        //     await internRepo.UpdateInternInfoAsync(model, mssv);
        //     return Ok("Update Successfully!");
        // }

    }
}
