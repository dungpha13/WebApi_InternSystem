using AmazingTech.InternSystem.Models;
using AmazingTech.InternSystem.Services;
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
        [Route("api/[controller]/Send-Interview-Schedule")]
        public IActionResult SendInterviewSchedule(LichPhongVanRequestModel model)
        {
            try
            {
                _guiLichPhongVanService.AddLichPhongVan(model);
                return Ok("Send Successful");
            }catch (Exception ex)
            {
                return Ok(ex.Message);
            }
            
        }
        [HttpGet]
        [Route("api/[controller]/Get-Schedule-By-InterviewerLogin")]
        public IActionResult GetScheduleByInterviewerLogin()
        {
            try
            {
                var result = _guiLichPhongVanService.getLichPhongVanByIdNgPhongVan();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("api/[controller]/Get-Schedule-By-IdNguoiDuocPhongVan")]
        public IActionResult GetScheduleByIdNguoiDuocPhongVan()
        {
            try
            {
                var result = _guiLichPhongVanService.getLichPhongVanByIdNguoiDuocPhongVan();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        [HttpDelete]
        [Route("api/[controller]/Delete-Schedule-By-IdNguoiDuocPhongVan/{id}")]
        public IActionResult DeleteScheduleByIdNguoiDuocPhongVan(string id)
        {
            try
            {
                _guiLichPhongVanService.DeleteLichPhongVanByIdNguoiDuocPhongVan(id);
                return Ok("Delete Successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
