using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Controllers
{

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
        [Route("api/[controller]/count-intern-send-cv-in-one-year")]
        public IActionResult CountInternSendCVInAYear(int year) 
        {
            try
            {
                var result = _dashboardService.CountInternSendCVInAYear(year);
                return Ok(result);

            }catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        [Route("api/[controller]/count-intern-send-cv-in-a-precious-of-a-year")]
        public IActionResult CountInternSendCVInPreciousOfAYear(int year,int precious)
        {
            try
            {
                var result = _dashboardService.CountInternSendCVInPreciousOfYear(year, precious);
                return Ok(result);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("api/[controller]/count-intern-be-interviewed-in-year")]
        public IActionResult CountInternInterviewedInAYear(int year)
        
        {
            try
            {
                var result = _dashboardService.CountInternInterviewedInAYear(year);
                return Ok(result);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("api/[controller]/count-intern-be-interviewed-in-precious-of-year")]
        public IActionResult CountInternInterviewedInPreciousOfYear(int year,int precious)
        
        {
            try
            {
                var result = _dashboardService.CountInternInterviewedInPreciousOfYear(year, precious);
                return Ok(result);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("api/[controller]/count-all-interned")]

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
        [Route("api/[controller]/count-interned-in-year")]
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
