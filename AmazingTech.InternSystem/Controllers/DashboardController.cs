using AmazingTech.InternSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        
        [HttpGet]
        [Route("GetAllIntern")]
        public IActionResult GetTotalUsersWithStatusTrue()
        {
            try
            {
                var result = _dashboardService.GetTotalUsersWithStatusTrue();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllInternWithYear")]
        public IActionResult GetTotalUsersWithStatusTrueAndYear(int year)
        {
            try
            {
                var result = _dashboardService.GetTotalUsersWithStatusTrueAndYear(year);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
    }
}
