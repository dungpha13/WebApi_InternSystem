using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Services;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
        [Route("\"api/[controller]/count-all-intern-be-interviewd")]
        
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
        [Authorize(Roles = "Admin")]
        [Route("api/[controller]/count-intern-be-interviewed-in-year")]
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
