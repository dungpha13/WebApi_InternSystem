using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Models.Request.InternInfo;
using AmazingTech.InternSystem.Models.Response.InternInfo;
using AmazingTech.InternSystem.Repositories;
using AmazingTech.InternSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Net.Http.Headers;
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
        [Authorize(Roles = "Admin, HR")]
        public async Task<IActionResult> GetAllInternsInfo()
        {

            return await _internInfoService.GetAllInternInfo();

        }

        [HttpGet("getAllDeletedIntern")]
        [Authorize(Roles = "Admin, HR")]
        public async Task<IActionResult> GetAllDeletedInternsInfo()
        {
            return await _internInfoService.GetAllDeletedInternInfo();
        }

        [HttpGet("get/{mssv}")]
        [Authorize(Roles = "Admin, HR")]
        public async Task<IActionResult> GetInternInfo(string mssv)
        {
            return await _internInfoService.GetInternInfo(mssv);
        }

        [HttpGet("getDeletedIntern/{mssv}")]
        [Authorize(Roles = "Admin, HR")]
        public async Task<IActionResult> GetDeletedInternInfo(string mssv)
        {
            return await _internInfoService.GetDeletedInternInfo(mssv);
        }


        [HttpPost("create")]
        [Authorize(Roles = "Admin, HR")]
        public async Task<IActionResult> AddNewInternInfo(AddInternInfoDTO model)
        {
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _internInfoService.AddInternInfo(userId, model);
        }

        [HttpDelete("delete/{mssv}")]
        [Authorize(Roles = "Admin, HR")]
        public async Task<IActionResult> DeleteInternInfo(string mssv)
        {
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _internInfoService.DeleteInternInfo(userId, mssv);
        }

        [HttpPut("update/{mssv}")]
        [Authorize(Roles = "Admin, HR")]
        public async Task<IActionResult> UpdateInternInfo(UpdateInternInfoDTO model, string mssv)
        {
            try
            {
                string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                return await _internInfoService.UpdateInternInfo(userId, model, mssv);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }

        [HttpPost("list/{kiThucTapId}")]
        [Authorize(Roles = "School,HR,Admin")]
        public async Task<IActionResult> AddListInternInfo(IFormFile file, string kiThucTapId)
        {
            return await _internInfoService.AddListInternInfo(file, kiThucTapId);
        }

        [HttpPost("{mssv}/comments")]
        [Authorize(Roles = "Admin, HR")]
        public async Task<IActionResult> AddCommentsInternInfo(CommentInternInfoDTO comment, string mssv)
        {
            string userIdCommentor = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _internInfoService.AddCommentInternInfo(comment, userIdCommentor, mssv);
        }

        [HttpGet("{mssv}/getCommentsByMSSV")]
        [Authorize(Roles = "Admin, HR")]
        public async Task<IActionResult> GetCommentsByMssv(string mssv)
        {
            return await _internInfoService.GetCommentsByMssv(mssv);
        }

        [HttpGet("createtemplatefile")]
        [Authorize(Roles = "School")]
        public IActionResult CreateTemplateFile()
        {
            var memoryStream = new MemoryStream();

            using (ExcelPackage package = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet;
                worksheet = package.Workbook.Worksheets.Add("TemplateFile");

                worksheet.Name = "TemplateFile";

                worksheet.Cells["A1"].Value = "STT";
                worksheet.Cells["B1"].Value = "HoVaTen";
                worksheet.Cells["C1"].Value = "NgaySinh";
                worksheet.Cells["D1"].Value = "GioiTinh";
                worksheet.Cells["E1"].Value = "SDT";
                worksheet.Cells["F1"].Value = "EmailTruong";
                worksheet.Cells["G1"].Value = "EmailCaNhan";
                worksheet.Cells["H1"].Value = "MSSV";
                worksheet.Cells["I1"].Value = "DiaChi";
                worksheet.Cells["J1"].Value = "SdtNguoiThan";
                worksheet.Cells["K1"].Value = "GPA";
                worksheet.Cells["L1"].Value = "TrinhDoTiengAnh";
                worksheet.Cells["M1"].Value = "NganhHoc";
                worksheet.Cells["N1"].Value = "ViTriMongMuon";
                worksheet.Cells["O1"].Value = "LinkFacebook";
                worksheet.Cells["P1"].Value = "LinkCV";

                package.Save();
            }

            memoryStream.Position = 0;
            var contentType = "application/octet-stream";
            var fileName = "templatefile.xlsx";
            return File(memoryStream, contentType, fileName);
        }


        //Gửi gmail cho tất cả sinh viên có trong kỳ thực tập
        [HttpPost("sendEmailForSignUpAccount")]
        [Authorize(Roles = "Admin, HR")]
        public async Task<IActionResult> SendEmailForSignUpAccountIntern([FromQuery] string idKyThucTap)
        {
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _internInfoService.SendMailForIntern(userId, idKyThucTap);
        }



    }
}
