using AmazingTech.InternSystem.Models.DTO;
using AmazingTech.InternSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AmazingTech.InternSystem.Controllers
{
    [Route("api/cong-nghes")]
    [ApiController]
    public class TechController : ControllerBase
    {

        private readonly ITechService _service;

        public TechController(IServiceProvider serviceProvider)
        {
            _service = serviceProvider.GetRequiredService<ITechService>();
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllTech()
        {
            var  tech = await _service.getAllTech();
            return Ok(tech);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateTech([FromBody] TechModel tech)
        {
            await _service.CreateTech(tech);
            return Ok(tech);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateTech(String id, TechModel tech)
        {
            await _service.UpdateTech(id, tech);
            return Ok(tech);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteTech(String id, TechModel tech)
        {
            await _service.DeleteTech(id, tech);
            return Ok(tech);
        }
    }
}
