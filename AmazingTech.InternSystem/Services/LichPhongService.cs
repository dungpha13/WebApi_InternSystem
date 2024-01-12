using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Services
{
    public interface ILichPhongService
    {
        IActionResult GetAllLichPhong();
        IActionResult GetLichPhongByIdNguoiDuocPhongVan(string idNguoiDuocPhongVan);

        IActionResult GetLichPhongByIdNguoiPhongVan(string idNguoiPhongVan);
    }
    public class LichPhongService : ILichPhongService
    {
        private ILichPhongRepository _lirepository;
        public LichPhongService(ILichPhongRepository lichPhongRepository)
        {
            _lirepository = lichPhongRepository;
        }

        public IActionResult GetAllLichPhong()
        {
            List<LichPhongVan> lis = _lirepository.GetAllLichPhong();
            return new OkObjectResult(lis);
        }

        public IActionResult GetLichPhongByIdNguoiDuocPhongVan(string idNguoiDuocPhongVan)
        {
            List<LichPhongVan> lis = _lirepository.GetAllLichPhong().Where(u => u.IdNguoiDuocPhongVan.Equals(idNguoiDuocPhongVan)).ToList();
            return new OkObjectResult(lis);
        }

        public IActionResult GetLichPhongByIdNguoiPhongVan(string idNguoiPhongVan)
        {
            List<LichPhongVan> lis = _lirepository.GetAllLichPhong().Where(u => u.IdNguoiDuocPhongVan.Equals(idNguoiPhongVan)).ToList();
            return new OkObjectResult(lis);
        }
    }
}
