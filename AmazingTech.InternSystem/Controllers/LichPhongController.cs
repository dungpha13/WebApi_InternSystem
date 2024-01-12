using AmazingTech.InternSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class LichPhongController : ControllerBase
    {
        private readonly ILichPhongService _lichPhongService;

        public LichPhongController(ILichPhongService lichPhongService)
        {
            _lichPhongService = lichPhongService;
        }

        [HttpGet]
        [Route("api/[controller]/Get-Schedule")]
        public IActionResult GetAllLichPhong()
        {
            return _lichPhongService.GetAllLichPhong();
        }

        [HttpGet]
        [Route("api/[controller]/GET-LICHBYIDNGPHONGVAN")]

        public IActionResult GetLichPhongByIdNguoiDuocPhongVan([FromRoute] string idNguoiDuocPhongVan) 
        {
            return _lichPhongService.GetLichPhongByIdNguoiDuocPhongVan(idNguoiDuocPhongVan);
        }

        [HttpGet]
        [Route("api/[controller]/-LICHBYIDNGUOIDUOCPHONGVAN")]

        public IActionResult GetLichPhongByIdNguoiPhongVan([FromRoute] string idNguoiPhongVan)
        {
            return _lichPhongService.GetLichPhongByIdNguoiDuocPhongVan(idNguoiPhongVan);
        }
    }
}
