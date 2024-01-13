using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request.TruongHoc;
using AmazingTech.InternSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Services
{
    public class TruongService : ITruongService
    {
        private ITruongRepository _truongRepository;

        public TruongService(ITruongRepository truongRepository)
        {
            _truongRepository = truongRepository;
        }

        public IActionResult AddTruong(AddTruongHocDTO request)
        {
            TruongHoc truong = new TruongHoc()
            {
                Ten = request.Ten,
                SoTuanThucTap = request.SoTuanThucTap
            };

            _truongRepository.AddTruong(truong);
            return new OkResult();
        }

        public IActionResult DeleteTruong(string id)
        {
            var existingTruong = _truongRepository.GetTruong(id);

            if (existingTruong is not null)
            {
                _truongRepository.DeleteTruong(existingTruong);
            }

            return new NoContentResult();
        }

        public IActionResult GetAllTruongs()
        {
            List<TruongHoc> truongs = _truongRepository.GetAllTruongs();
            return new OkObjectResult(truongs);
        }

        public IActionResult GetTruong(string id)
        {
            var truong = _truongRepository.GetTruong(id);
            return new OkObjectResult(truong);
        }

        public IActionResult UpdateTruong(UpdateTruongHocDTO truong)
        {
            var existingTruong = _truongRepository.GetTruong(truong.Id);

            if (existingTruong is null)
            {
                return new BadRequestObjectResult("Truong doesn't exist!");
            }

            existingTruong.Ten = truong.Ten;
            existingTruong.SoTuanThucTap = truong.SoTuanThucTap;

            _truongRepository.UpdateTruong(existingTruong);
            return new NoContentResult();
        }

    }
}
