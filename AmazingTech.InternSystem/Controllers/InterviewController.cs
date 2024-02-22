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
        public async Task<IActionResult> CreateQuestion([FromBody] List<AwserQuest> traloi)
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


        [HttpPost]
        [Authorize]
        [Route("Rating")]
        public async Task<IActionResult> RatingAnwser([FromBody] List<RatingModel> traloi)
        {

            try
            {
                string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                return await _service.RatingAnwser(user, traloi);
            }
            catch (Exception ex)
            {
                return BadRequest("ID ANWSER IS NOT CORRECT");
            }

        }

        [HttpPut]
        [Authorize]
        [Route("Update-Rating")]
        public async Task<IActionResult> UpdateRating([FromBody] List<RatingModel> traloi)
        {

            try
            {
                string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                return await _service.RatingAnwser(user, traloi);
            }
            catch (Exception ex)
            {
                return BadRequest("ID ANWSER IS NOT CORRECT");
            }
        }
        [HttpDelete]
        [Authorize]
        [Route("Delete-Interview/{UserID}")]
        public async Task<IActionResult>DeleteRating(string UserID)
        {

            try
            {
                string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                return await _service.DeleteInterView(user, UserID);
            }
            catch (Exception ex)
            {
                return BadRequest("ID USER IS NOT CORRECT OR HAVE NOT BEEN INTERVIEWED YET");
            }
        }
    }
}
