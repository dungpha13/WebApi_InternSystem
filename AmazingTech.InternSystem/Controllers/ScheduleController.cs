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
        public ScheduleController(IGuiLichPhongVanService guiLichPhongVanService)
        {
            _guiLichPhongVanService = guiLichPhongVanService;
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = ("Bearer"))]
        [Route("api/lich-phong-vans/Send-Interview-Schedule")]
        public IActionResult SendInterviewSchedule([FromBody]LichPhongVanRequestModel model)
        {
            try
            {
                _guiLichPhongVanService.AddLichPhongVan(model);
                return Ok("Send Successful");
            }catch(Exception ex)
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
               var result =  _guiLichPhongVanService.getLichPhongVanByIdNgPhongVan();
                return Ok(result);
            }catch(Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        [HttpPut]
        [Route("api/lich-phong-vans/Change-schudele")]
        public IActionResult UpdateScheduel(LichPhongVanRequestModel model)
        {
            try
            {
                var result = _guiLichPhongVanService.UpdateSchedule(model);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        [HttpDelete]
        [Route("api/lich-phong-vans/Delete-schedule")]
        public IActionResult DeleteSchedule(string scheduleId)
        {
            try
            {
                _guiLichPhongVanService.deleteSchedudle(scheduleId);
                return Ok("Delete Succesful");

            }catch(Exception ex ) 
            {
            return Ok(ex.Message);}
             }
    }
}
