using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.DTO;
using AmazingTech.InternSystem.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using static AmazingTech.InternSystem.Data.Enum.Enums;

namespace AmazingTech.InternSystem.Controllers
{
    [Route("api/Phong-vans")]
    [ApiController]
    public class InterviewController : ControllerBase
    {
        private readonly IInterviewService _service;
        private readonly AppDbContext _context;

        public InterviewController(IServiceProvider serviceProvider)
        {
            _service = serviceProvider.GetRequiredService<IInterviewService>();

        }


        [HttpGet]
        [Authorize]
        [Route("List-Question/{idCongnghe}")]
        public async Task<List<ViewQuestionInterview>> CreateQuestion(string idCongnghe)
        {
            return await _service.getAllQuestion(idCongnghe);
        }

        [HttpPost]
        [Authorize]
        [Route("Anwser")]
        public async Task<IActionResult> CreateQuestion( [FromBody] List<AwserQuest> traloi)
        {
           
            try
            {                
                string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                return await _service.AwserQuestion(user, traloi);
            }
            catch (Exception ex)
            {
                return BadRequest("ID QUESTION IS NOT CORRECT");
            }
        }

        [HttpGet]

        [Route("List-Anwser/{IdUser}")]
        public async Task<List<ViewAnswer>> ViewAnswer(string IdUser)
        {
            return await _service.ViewAnswers(IdUser);
        }
    }
}
