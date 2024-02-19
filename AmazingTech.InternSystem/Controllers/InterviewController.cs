using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.DTO;
using AmazingTech.InternSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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


        [HttpPost]
        [Authorize]
        [Route("Anwser")]
        public async Task<IActionResult> CreateQuestion( [FromBody] List<AwserQuest> traloi)
        {
            string user = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var save = await _service.AwserQuestion(user, traloi);
            return Ok(save == 1 ? "Success" : "failed");
        }
    }
}
