using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Models.Request.InternInfo;
using AmazingTech.InternSystem.Models.Response.InternInfo;
using AmazingTech.InternSystem.Repositories;
using AmazingTech.InternSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AmazingTech.InternSystem.Controllers
{
    [Route("api/interns")]
    [ApiController]
    public class InternInfoController : ControllerBase
    {
        private readonly IInternInfoRepo _internRepo;
        private readonly IInternInfoService _internInfoService;
        private readonly AppDbContext _context;

        public InternInfoController(IInternInfoRepo internRepo, IInternInfoService internInfoService)
        {
            this._internRepo = internRepo;
            this._internInfoService = internInfoService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetAllInternsInfo()
        {

            return await _internInfoService.GetAllInternInfo();

        }

        [HttpGet("getAllDeletedIntern")]
        public async Task<IActionResult> GetAllDeletedInternsInfo()
        {
            return await _internInfoService.GetAllDeletedInternInfo();
        }

        [HttpGet("get/{mssv}")]
        public async Task<IActionResult> GetInternInfo(string mssv)
        {
            return await _internInfoService.GetInternInfo(mssv);
        }

        [HttpGet("getDeletedIntern/{mssv}")]
        public async Task<IActionResult> GetDeletedInternInfo(string mssv)
        {
            return await _internInfoService.GetDeletedInternInfo(mssv);
        }


        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> AddNewInternInfo(AddInternInfoDTO model)
        {
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _internInfoService.AddInternInfo(userId, model);
        }

        [HttpDelete("delete/{mssv}")]
        public async Task<IActionResult> DeleteInternInfo(string mssv)
        {
            return await _internInfoService.DeleteInternInfo(mssv);
        }

        [HttpPut("update/{mssv}")]
        public async Task<IActionResult> UpdateInternInfo(UpdateInternInfoDTO model, string mssv)
        {
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _internInfoService.UpdateInternInfo(userId, model, mssv);
        }

        [HttpPost("list/{kiThucTapId}")]
        [Authorize(Roles = "School,HR,Admin")]
        public async Task<IActionResult> AddListInternInfo(IFormFile file, string kiThucTapId)
        {
            return await _internInfoService.AddListInternInfo(file, kiThucTapId);
        }

        [HttpPost("{mssv}/comments")]
        [Authorize]
        public async Task<IActionResult> AddCommentsInternInfo(CommentInternInfoDTO comment, string mssv)
        {
            string userIdCommentor = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _internInfoService.AddCommentInternInfo(comment, userIdCommentor, mssv);
        }

        [HttpGet("{mssv}/getCommentsByMSSV")]
        public async Task<IActionResult> GetCommentsByMssv(string mssv)
        {
            return await _internInfoService.GetCommentsByMssv(mssv);
        }

    }
}
