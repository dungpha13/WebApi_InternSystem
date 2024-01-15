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
                return new BadRequestObjectResult("Truong khong ton tai!");
            }

            KiThucTap ki = new KiThucTap()
            {
                NgayBatDau = request.NgayBatDau,
                NgayKetThuc = request.NgayKetThuc,
                IdTruong = existingTruong.Id
            };

            _kiRepository.AddKiThucTap(ki);
            return new OkResult();
        }

        public IActionResult DeleteKiThucTap(string id)
        {
            var existingKi = _kiRepository.GetKiThucTap(id);

            if (existingKi is not null)
            {
                _kiRepository.DeleteKiThucTap(existingKi);
            }

            return new NoContentResult();
        }

        public IActionResult GetAllKiThucTaps()
        {
            List<KiThucTap> kis = _kiRepository.GetAllKiThucTaps();
            return new OkObjectResult(kis);
        }

        public IActionResult GetKiThucTap(string id)
        {
            var ki = _kiRepository.GetKiThucTap(id);
            return new OkObjectResult(ki);
        }

        public IActionResult GetKiThucTapsByTruong(string idTruong)
        {
            List<KiThucTap> kis = _kiRepository.GetAllKiThucTaps().Where(_ => _.IdTruong.Equals(idTruong)).ToList();
            return new OkObjectResult(kis);
        }

        public IActionResult UpdateKiThucTap(UpdateKiThucTapDTO ki)
        {
            var existingKi = _kiRepository.GetKiThucTap(ki.Id);

            if (existingKi is null)
            {
                return new BadRequestObjectResult("Id null!");
            }

            existingKi.NgayBatDau = ki.NgayBatDau;
            existingKi.NgayKetThuc = ki.NgayKetThuc;

            _kiRepository.UpdateKiThucTap(existingKi);
            return new NoContentResult();
        }
    }
}
