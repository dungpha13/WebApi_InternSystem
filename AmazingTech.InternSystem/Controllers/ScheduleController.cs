using AmazingTech.InternSystem.Models.Request;
using AmazingTech.InternSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Controllers
{

    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IGuiLichPhongVanService _guiLichPhongVanService;
        private readonly IEmailService _emailService;
        public ScheduleController(IGuiLichPhongVanService guiLichPhongVanService, IEmailService emailService)
        {
            _guiLichPhongVanService = guiLichPhongVanService;
            _emailService = emailService;
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = ("Bearer"))]
        [Route("api/lich-phong-vans/Send-Interview-Schedule")]
        public IActionResult SendInterviewSchedule([FromBody] LichPhongVanRequestModel model)
        {
            try
            {
                _guiLichPhongVanService.AddLichPhongVan(model);
                return Ok("Send Successful");
            } catch (BadHttpRequestException ex)
            {
                return Ok(ex.Message);
            }

        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = ("Bearer"))]
        [Route("api/lich-phong-vans/Get-Schedule-By-InterviewerLogin")]
        public IActionResult GetScheduleByInterviewerLogin()
        {
            try
            {
                var result = _guiLichPhongVanService.getmyInterviewSchedule();
                return Ok(result);
            } catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("api/lich-phong-vans/ConfirmEmail")]
        public IActionResult ConfirmEmail([FromQuery] string id)
        {
            var result = _guiLichPhongVanService.ConfirmEmail(id);

            if (result)
            {
                return Ok(new { Message = "Email confirmed successfully." });
            }

            return BadRequest(new { Message = "Email confirmed unsuccessfully" });
        }


        [HttpPut]
        [Authorize]
        [Route("api/lich-phong-vans/Change-schudele")]
        public IActionResult UpdateScheduel(LichPhongVanRequestModel model)
        {
            try
            {
                var result = _guiLichPhongVanService.UpdateSchedule(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        [HttpDelete]
        [Authorize]
        [Route("api/lich-phong-vans/Delete-schedule")]
        public IActionResult DeleteSchedule(string scheduleId)
        {
            try
            {
                _guiLichPhongVanService.deleteSchedudle(scheduleId);
                return Ok("Delete Succesful");

            } catch (Exception ex)
            {
                return Ok(ex.Message); }
        }
        [HttpPost]
        [Route("api/send-mail-test")]
        public IActionResult SendImage(string content, string email, string subject)
        {
            _emailService.SendMail2(content, email, subject);
            return Ok("Send Successfull");
        }
    }
}


