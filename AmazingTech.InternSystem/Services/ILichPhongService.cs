using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Services
{
    public interface ILichPhongService
    {
        IActionResult GetAllLichPhong();
        IActionResult GetLichPhongByIdNguoiDuocPhongVan(string idNguoiDuocPhongVan);
    }
}
