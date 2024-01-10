using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmazingTech.InternSystem.Controllers
{
    // LichPhongVanController.cs
    [Route("api/[controller]")]
    [ApiController]
    public class LichPhongVanController : ControllerBase
    {
        private readonly LichPhongVanService _lichPhongVanService;

        public LichPhongVanController(LichPhongVanService lichPhongVanService)
        {
            _lichPhongVanService = lichPhongVanService;
        }


        // Thêm các phương thức API ở đây
        [HttpPost("AddLichPhongVan")]
        public IActionResult AddLichPhongVan([FromBody] LichPhongVan lichPhongVan)
        {
            if(lichPhongVan == null)
        {
                return BadRequest("Invalid data");
            }

            //bool success = _lichPhongVanService.AddLichPhongVan(lichPhongVan);
            try
            {
                _lichPhongVanService.AddLichPhongVan(lichPhongVan);
                return StatusCode(201, "Lịch phỏng vấn đã được thêm mới");
            }
            catch (Exception ex) 
            {
                return StatusCode(500, "Đã xảy ra lỗi khi thêm lịch phỏng vấn");
            }
        }

        [HttpPut("UpdateLichPhongVan")]
        public IActionResult UpdateLichPhongVan([FromBody] LichPhongVan lichPhongVan)
        {
            if (lichPhongVan == null)
            {
                return BadRequest("Invalid data");
            }

            //bool success = _lichPhongVanService.UpdateLichPhongVan(lichPhongVan);
            try
            {
                _lichPhongVanService.UpdateLichPhongVan(lichPhongVan);
                return Ok("Lịch phỏng vấn đã được cập nhật");
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi khi cập nhật lịch phỏng vấn");
            }
        }
    }

}
