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
        private readonly IInternInfoRepo internRepo;
        private readonly IInternInfoService internInfoService;

        public InternInfoController(IInternInfoRepo internRepo, IInternInfoService internInfoService)
        {
            this.internRepo = internRepo;
            this.internInfoService = internInfoService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllInternsInfo()
        {
          
            return await internInfoService.GetAllInternInfo();
          
        }

        [HttpGet("{mssv}")]
        public async Task<IActionResult> GetInternInfo(string mssv)
        {

            return await internInfoService.GetInternInfo(mssv);

        }

        [HttpPost("")]
        public async Task<IActionResult> AddNewInternInfo(AddInternInfoDTO model)
        {

            return await internInfoService.AddInternInfo(model); 
        }

        [HttpDelete("{mssv}")]
        public async Task<IActionResult> DeleteInternInfo(string mssv)
        {
            return await internInfoService.DeleteInternInfo(mssv);
        }

        [HttpPut("{mssv}")]
        public async Task<IActionResult> UpdateInternInfo(UpdateInternInfoDTO model, string mssv)
        {
            return await internInfoService.UpdateInternInfo(model, mssv);
        }
    }
}
