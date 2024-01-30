using AmazingTech.InternSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Controllers
{

    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _dashboardService;
        public DashboardController(DashboardService dashboardService)
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


    }
}
