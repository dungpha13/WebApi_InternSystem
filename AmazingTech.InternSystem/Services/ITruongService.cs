using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request.TruongHoc;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Services
{
    public interface ITruongService
    {
        IActionResult GetAllTruongs();
        IActionResult GetTruong(string id);
        IActionResult AddTruong(AddTruongHocDTO truong);
        IActionResult DeleteTruong(string id);
        IActionResult UpdateTruong(UpdateTruongHocDTO truong);
    }
}
