using System.Text;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Services;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace AmazingTech.InternSystem.Controllers
{
    [ApiController]
    [Route("api/file-reader")]
    public class InternController : ControllerBase
    {
        private readonly IFileReaderService _service;
        public InternController(IFileReaderService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("sheet/{kiThucTapId}")]
        public IActionResult ReadFile(IFormFile request, string kiThucTapId)
        {
            if (request == null || request.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            return _service.ReadFile(request, kiThucTapId);

        }
    }
}