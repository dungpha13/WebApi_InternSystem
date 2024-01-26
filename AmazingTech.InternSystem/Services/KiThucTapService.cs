using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request.KiThucTap;
using AmazingTech.InternSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Services
{
    public class KiThucTapService : IKiThucTapService
    {
        private IKiThucTapRepository _kiRepository;
        private readonly ITruongRepository _truongRepository;

        public KiThucTapService(IKiThucTapRepository kiThucTapRepository, ITruongRepository truongRepository)
        {
            _kiRepository = kiThucTapRepository;
            _truongRepository = truongRepository;
        }

        public IActionResult AddKiThucTap(AddKiThucTapDTO request)
        {

            var existingTruong = _truongRepository.GetTruong(request.IdTruong);

            if (existingTruong is null)
            {
                return new BadRequestObjectResult($"Truong voi id {request.IdTruong} khong ton tai");
            }

            KiThucTap ki = new KiThucTap()
            {
                NgayBatDau = request.NgayBatDau,
                NgayKetThuc = request.NgayKetThuc,
                //IdTruong = existingTruong.Id
            };

            var result = _kiRepository.AddKiThucTap(ki);

            if (result == 0)
            {
                return new BadRequestObjectResult("Unsuccess");
            }

            return new OkObjectResult("Success");
        }

        public async Task<IActionResult> DeleteKiThucTap(string id)
        {
            var existingKi = _kiRepository.GetKiThucTap(id);

            if (existingKi is null)
            {
                return new BadRequestObjectResult($"KiThucTap voi id {id} khong ton tai");
            }

            var result = await _kiRepository.DeleteKiThucTap(existingKi);

            if (result == 0)
            {
                return new BadRequestObjectResult("Unsuccess");
            }

            return new OkObjectResult("Success");
        }

        public IActionResult GetAllKiThucTaps()
        {
            List<KiThucTap> kis = _kiRepository.GetAllKiThucTaps();
            return new OkObjectResult(kis);
        }

        public IActionResult GetKiThucTap(string id)
        {
            var ki = _kiRepository.GetKiThucTap(id);

            if (ki is null)
            {
                return new BadRequestObjectResult($"KiThucTap voi id {id} khong ton tai");
            }

            return new OkObjectResult(ki);
        }

        public IActionResult GetKiThucTapsByTruong(string idTruong)
        {
            List<KiThucTap> kis = _kiRepository.GetAllKiThucTaps().Where(ktt => ktt.IdTruong.Equals(idTruong)).ToList();
            return new OkObjectResult(kis);
        }

        public IActionResult UpdateKiThucTap(UpdateKiThucTapDTO ki, string id)
        {
            var existingKi = _kiRepository.GetKiThucTap(id);

            if (existingKi is null)
            {
                return new BadRequestObjectResult($"KiThucTap voi id {id} khong ton tai");
            }

            existingKi.Ten = ki.Name;
            existingKi.NgayBatDau = ki.NgayBatDau;
            existingKi.NgayKetThuc = ki.NgayKetThuc;

            var result = _kiRepository.UpdateKiThucTap(existingKi);

            if (result == 0)
            {
                return new BadRequestObjectResult("Unsuccess");
            }

            return new OkObjectResult("Success");
        }
    }
}
