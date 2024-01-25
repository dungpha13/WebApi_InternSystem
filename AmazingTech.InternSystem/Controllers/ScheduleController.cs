using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Data.Enum;
using AmazingTech.InternSystem.Models.Request;
using AmazingTech.InternSystem.Repositories;
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
        private readonly IUserRepository _userRepository;
        public ScheduleController(IGuiLichPhongVanService guiLichPhongVanService, IEmailService emailService,IUserRepository userRepository)
        {
            _guiLichPhongVanService = guiLichPhongVanService;
            _emailService = emailService;
            _userRepository = userRepository;
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

        [HttpGet]
        [Route("api/lich-phong-vans/SendResultInterviewEmail")]
        public IActionResult SendRessultInterviewEmail([FromQuery] string email)
        {
            var result = _guiLichPhongVanService.SendResultInterviewEmail(email);

            if (result)
            {
                return Ok(new { Message = "Send email successfully." });
            }

            return BadRequest(new { Message = "Send email unsuccessfully" });
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
        [HttpPost]
        [Authorize(Roles = Roles.ADMIN)]
        [Route("api/lich-phong-vans/Auto-Create-Schedule")]
        public IActionResult AutoCreateSchedule(DateTime start, DateTime end, string diadiemphongvan, InterviewForm interviewForm)
        {
            try
            {
                _guiLichPhongVanService.AutoCreateSchedule(start, end, diadiemphongvan,interviewForm);
                return Ok("Successful");
            }catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        [HttpGet]
        [Route("test")]
        public IActionResult GetInterUser(DateTime start , DateTime end)
        {
            var result = _guiLichPhongVanService.GetHrOrMentorWithoutInternView(start, end);
            var result2 = 0;
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAllLichPhongvan()
        {
            return _guiLichPhongVanService.AllLichPhongVan();
        }

        [HttpGet]
        [Route("api/lich-phong-vans/")]
        public IActionResult SendListOfInternsToMentor([FromQuery] string email)
        {
            try
            {
                var result = _guiLichPhongVanService.SendListOfInternsToMentor(email);
                return Ok(result);
            }
            catch (BadHttpRequestException ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}


