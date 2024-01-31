using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.DTO;
using AmazingTech.InternSystem.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AmazingTech.InternSystem.Controllers
{

    [Route("api/Cau-hois")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _service;
        private readonly AppDbContext _context;

        public QuestionController(IServiceProvider serviceProvider)
        {
            _service = serviceProvider.GetRequiredService<IQuestionService>();

        }

        [HttpGet]
        [Authorize(Roles = "Admin, HR")]
        [Route("get/{idCongNghe}")]
        public async Task<IActionResult> GetAllQuestion(string idCongNghe)
        {
            var question = await _service.getAllQuestion(idCongNghe);
            return Ok(question);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, HR")]
        [Route("create/{idCongNghe}")]
        public async Task<IActionResult> CreateQuestion(string idCongNghe,[FromBody] QuestionDTO cauhoi)
        {
            string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var save = await _service.CreateQuestion(user, idCongNghe, cauhoi);
            return Ok(save == 1 ? "Success" : "failed");
        }

        [HttpPut]
        [Authorize(Roles = "Admin, HR")]
        [Route("update/{idCongNghe}/{idCauHoi}")]
        public async Task<IActionResult> UpdateTech(string idCongNghe, string idCauHoi, [FromBody] QuestionDTO cauhoi)
        {
            string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var save = await _service.UpdateQuestion(user, idCongNghe, idCauHoi, cauhoi);
            return Ok(save == 1 ? "Success" : "failed");
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, HR")]
        [Route("delete/{idCongNghe}/{idCauHoi}")]
        public async Task<IActionResult> DeleteTech(string idCongNghe, string idCauHoi)
        {
            string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var save = await _service.DeleteQuestion(user, idCongNghe, idCauHoi);
            return Ok(save == 1 ? "Success" : "failed");

        }
        [HttpPost("{idCongNghe}/list")]
        [Authorize(Roles = "Admin, HR")]
        public async Task<IActionResult> AddListInternInfo(IFormFile file, string idCongNghe)
        {
            string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _service.AddListQuest(file, user ,idCongNghe);
        }
    }
}
