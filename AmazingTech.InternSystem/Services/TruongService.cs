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

            var result = _truongRepository.AddTruong(truong);

            if (result == 0)
            {
                return new BadRequestObjectResult("Unsuccess");
            }

            return new OkObjectResult("Success");
        }

        public IActionResult DeleteTruong(string id)
        {
            var existingTruong = _truongRepository.GetTruong(id);

            if (existingTruong is null)
            {
                return new BadRequestObjectResult($"Truong voi id {id} khong ton tai");
            }

            var result = _truongRepository.DeleteTruong(existingTruong);

            if (result == 0)
            {
                return new BadRequestObjectResult("Unsuccess");
            }

            return new OkObjectResult("Success");
        }

        public IActionResult GetAllTruongs()
        {
            List<TruongHoc> truongs = _truongRepository.GetAllTruongs();
            return new OkObjectResult(truongs);
        }

        public IActionResult GetTruong(string id)
        {
            var truong = _truongRepository.GetTruong(id);

            if (truong is null)
            {
                return new BadRequestObjectResult($"Truong voi id {id} khong ton tai");
            }

            return new OkObjectResult(truong);
        }

        public IActionResult UpdateTruong(UpdateTruongHocDTO truong)
        {
            var existingTruong = _truongRepository.GetTruong(truong.Id);

            if (existingTruong is null)
            {
                return new BadRequestObjectResult($"Truong voi id {truong.Id} khong ton tai");
            }

            existingTruong.Ten = truong.Ten;
            existingTruong.SoTuanThucTap = truong.SoTuanThucTap;

            var result = _truongRepository.UpdateTruong(existingTruong);

            if (result == 0)
            {
                return new BadRequestObjectResult("Unsuccess");
            }

            return new OkObjectResult("Success");
        }

    }
}
