using AmazingTech.InternSystem.Models.Request.InternInfo;
using AmazingTech.InternSystem.Models.Response.InternInfo;
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
            this._internRepo = internRepo;
            this._internInfoService = internInfoService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllInternsInfo()
        {
          
            return await _internInfoService.GetAllInternInfo();
          
        }

        [HttpGet("{mssv}")]
        public async Task<IActionResult> GetInternInfo(string mssv)
        {
            return await _internInfoService.GetInternInfo(mssv);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddNewInternInfo(AddInternInfoDTO model)
        {

            return await _internInfoService.AddInternInfo(model); 
        }

        [HttpDelete("{mssv}")]
        public async Task<IActionResult> DeleteInternInfo(string mssv)
        {
            return await _internInfoService.DeleteInternInfo(mssv);
        }

        [HttpPut("{mssv}")]
        public async Task<IActionResult> UpdateInternInfo(UpdateInternInfoDTO model, string mssv)
        {
            return await _internInfoService.UpdateInternInfo(model, mssv);
        }
    }
}
