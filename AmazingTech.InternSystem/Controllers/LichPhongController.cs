using AmazingTech.InternSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LichPhongController : ControllerBase
    {
        private readonly ILichPhongService _lichPhongService;

        public LichPhongController(ILichPhongService lichPhongService)
        {
            _lichPhongService = lichPhongService;
        }

        [HttpGet]
        public IActionResult GetAllLichPhong()
        {
            return _lichPhongService.GetAllLichPhong();
        }

        [HttpGet]
        [Route("{idNguoiDuocPhongVan}")]

        public IActionResult GetLichPhongByIdNguoiDuocPhongVan([FromRoute] string idNguoiDuocPhongVan) 
        {
            return _lichPhongService.GetLichPhongByIdNguoiDuocPhongVan(idNguoiDuocPhongVan);
        }
    }
}
