using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Data.Enum;
using AmazingTech.InternSystem.Models.Request.LichPhongVan;
using AmazingTech.InternSystem.Repositories;
using AmazingTech.InternSystem.Services;
using DocumentFormat.OpenXml.Office2016.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AmazingTech.InternSystem.Controllers
{

    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IGuiLichPhongVanService _guiLichPhongVanService;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        public ScheduleController(IGuiLichPhongVanService guiLichPhongVanService, IEmailService emailService, IUserRepository userRepository)
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
            }
            catch (BadHttpRequestException ex)
            {

                return BadRequest(ex.Message);
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
            }
            catch (BadHttpRequestException ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = ("Bearer"))]
        [Route("api/lich-phong-vans/Get-Schedule-By-IntervieweeLogin")]
        public IActionResult GetScheduleByIntervieweeLogin()
        {
            try
            {
                var result = _guiLichPhongVanService.GetIntervieweeLoginSchedule();
                return Ok(result);
            }
            catch (BadHttpRequestException ex)
            {

                return BadRequest(ex.Message);
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
        [Authorize(AuthenticationSchemes = ("Bearer"))]
        [Route("api/lich-phong-vans/SendResultInterviewEmail")]
        public IActionResult SendResultInterviewEmail([FromQuery] string email, string linkNhomZaloTong, string linkNhomZaloChuyenNganh)
        {
            try
            {
                _guiLichPhongVanService.SendResultInterviewEmail(email, linkNhomZaloTong, linkNhomZaloChuyenNganh);
                return Ok(new { Message = "Send email successfully." });
            }
            catch (BadHttpRequestException ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = ("Bearer"))]
        [Route("api/lich-phong-vans/GetAllUserByKetQua")]
        public async Task<IActionResult> GetAllUserByKetQua(Result ketqua)
        {
            return await _guiLichPhongVanService.GetAllUserByKetQua(ketqua);
        }


        [HttpPut]
        [Authorize(AuthenticationSchemes = ("Bearer"))]
        [Route("api/lich-phong-vans/Change-schedule")]
        public IActionResult UpdateScheduel(LichPhongVanRequestModel model)
        {
            try
            {
                var result = _guiLichPhongVanService.UpdateSchedule(model);
                return Ok(result);
            }
            catch (BadHttpRequestException ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Authorize(AuthenticationSchemes = ("Bearer"))]
        [Route("api/lich-phong-vans/Delete-schedule")]
        public IActionResult DeleteSchedule(string scheduleId)
        {
            try
            {
                _guiLichPhongVanService.deleteSchedudle(scheduleId);
                return Ok("Delete Succesful");

            }
            catch (BadHttpRequestException ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("api/send-mail-test")]
        public IActionResult SendImage(string content, string email, string subject)
        {
            content = "Kính gửi bạn " + "ĐHP" + ",<br>Đại diện bộ phận Nhân sự (HR) tại Công Ty TNHH Giải Pháp và Công nghệ Amazing, chúng tôi xin chân thành xin lỗi khi phải thông báo về việc dời lại lịch phỏng vấn. <br>Đây là lịch phỏng vấn mới của bạn " +
               "29/11/2003" +
                "<br> Khoảng thời gian phỏng vấn dự kiến :" + "15'" +
                "<br> Hình thức phỏng vấn: " + "Online" + "<br>Địa điểm phỏng vấn " +
                "FPT" +
                "<br> Xin cảm ơn sự hiểu biết và sự linh hoạt của bạn trong việc xem xét yêu cầu của tôi. Xin vui lòng cho chúng tôi  biết nếu có bất kỳ điều gì cần được điều chỉnh hoặc có bất kỳ thông tin nào khác chúng tôi cần cung cấp.<br>Trân trọng";
            _emailService.SendMail2(content, email, subject);
            return Ok("Send Successfull");
        }
        [HttpPost]
        [Authorize]
        [Route("api/lich-phong-vans/Auto-Create-Schedule")]
        public IActionResult AutoCreateSchedule([EmailAddress] string mailNguoiPhongVan, DateTime start,  string diadiemphongvan, InterviewForm interviewForm,int TimeDuration)
        {
            try
            {
                DateTime end = new DateTime(start.Year, start.Month, start.Day, 17, 0, 0);
                _guiLichPhongVanService.AutoCreateSchedule(mailNguoiPhongVan, start, end, diadiemphongvan, interviewForm,TimeDuration);
                return Ok("Successful");
            }
            catch (BadHttpRequestException ex)
            {

                return BadRequest(ex.Message);
            }
        }
        //[HttpGet]
        //[Route("test")]
        //public IActionResult GetInterUser(DateTime start , DateTime end)
        //{
        //    var result = _guiLichPhongVanService.GetHrOrMentorWithoutInternView(start, end);
        //    var result2 = 0;
        //    return Ok(result);
        //}

        [HttpGet]
        [Route("api/lich-phong-vans/GetAllLichPhongVan")]
        [Authorize(Roles = Roles.ADMIN + "," + Roles.HR)]
        public IActionResult GetAllLichPhongvan()
        {
            /*return _guiLichPhongVanService.AllLichPhongVan();*/

            var lichphongvanList = _guiLichPhongVanService.AllLichPhongVan();
            return new OkObjectResult(lichphongvanList);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = ("Bearer"))]
        [Route("api/lich-phong-vans/SendListOfInternsToMentor")]
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

        [HttpGet]
        [Authorize(AuthenticationSchemes = ("Bearer"))]
        [Route("api/lich-phong-vans/GetScheduleByIdNguoiDuocPhongVan/{id}")]

        public IActionResult GetLichPhongVanByIdNguoiDuocPhongVan(string id)
        {
            var lichPhongVanList = _guiLichPhongVanService.GetLichPhongVanByIdNguoiDuocPhongVan(id);
            return new OkObjectResult(lichPhongVanList);
        }
        [HttpGet]

        [Route("api/test/get-intern-only")]
        public IActionResult GetIntern()
        {
            var intern = _guiLichPhongVanService.GetInternOnly();
            var intern2 = _guiLichPhongVanService.GetInternWithoutInternView();
            List<int> check = new List<int>();
            check.Add(intern.Count); check.Add(intern2.Count);
            return Ok(check);
        }

        //[HttpGet]
        //[Route("api/test/get-intern-only")]
        //public IActionResult GetIntern()
        //{
        //    var intern = _guiLichPhongVanService.GetInternOnly();
        //    var intern2 = _guiLichPhongVanService.GetInternWithoutInternView();
        //    List<int> check = new List<int>();
        //   check.Add(intern.Count); check.Add(intern2.Count);
        //    return Ok(check);
        //}

        [HttpPut]
        [Authorize]
        [Route("api/lich-phong-vans/Update-Interview-Result")]
        public IActionResult UpdateInterviewResult(string idlichphongvan, Result result, string Vitri)
        {
            try
            {
                _guiLichPhongVanService.UpdateResult(idlichphongvan, result, Vitri);
                return Ok("Update sucessful");
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = ("Bearer"))]
        [Route("api/lich-phong-vans/Get-All-User-With-Consider-Result")]
        public IActionResult GetAllUserWithConsiderResult()
        {
            try
            {
                var result = _guiLichPhongVanService.UserWithConsiderResult();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}


