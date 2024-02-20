using System.Security.Claims;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request.KiThucTap;
using AmazingTech.InternSystem.Models.Response;
using AmazingTech.InternSystem.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Services
{
    public class KiThucTapService : IKiThucTapService
    {
        private IKiThucTapRepository _kiRepository;
        private readonly ITruongRepository _truongRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public KiThucTapService(IKiThucTapRepository kiThucTapRepository, ITruongRepository truongRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _kiRepository = kiThucTapRepository;
            _truongRepository = truongRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult AddKiThucTap(AddKiThucTapDTO request)
        {
            var uId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var existingTruong = _truongRepository.GetTruong(request.IdTruong);

            if (existingTruong is null)
            {
                return new BadRequestObjectResult($"Truong voi id {request.IdTruong} khong ton tai");
            }

            var ktt = _kiRepository.GetAllKiThucTaps();

            if (ktt.Any())
            {
                foreach (var item in _kiRepository.GetAllKiThucTaps())
                {
                    if (item.Ten != null && item.Ten.ToUpper().Equals(request.Name.ToUpper()))
                    {
                        return new BadRequestObjectResult("Name has been taken!");
                    }
                }
            }

            KiThucTap ki = new KiThucTap()
            {
                Ten = request.Name,
                NgayBatDau = request.NgayBatDau,
                NgayKetThuc = request.NgayKetThuc,
                IdTruong = existingTruong.Id,
                CreatedBy = uId
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
            return new OkObjectResult(_mapper.Map<List<KiThucTapResponseDTO>>(kis));
        }

        public IActionResult GetKiThucTap(string id)
        {
            var ki = _kiRepository.GetKiThucTap(id);

            if (ki is null)
            {
                return new BadRequestObjectResult($"KiThucTap voi id {id} khong ton tai");
            }

            return new OkObjectResult(_mapper.Map<KiThucTapResponseDTO>(ki));
        }

        public IActionResult GetKiThucTapsByTruong(string idTruong)
        {
            List<KiThucTap> kis = _kiRepository.GetAllKiThucTaps().Where(_ => _.IdTruong.Equals(idTruong)).ToList();
            return new OkObjectResult(kis);
        }

        public IActionResult UpdateKiThucTap(UpdateKiThucTapDTO ki, string id)
        {
            var uId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var existingKi = _kiRepository.GetKiThucTap(id);

            if (existingKi is null)
            {
                return new BadRequestObjectResult($"KiThucTap voi id {id} khong ton tai");
            }

            var check = _kiRepository.GetAllKiThucTaps();

            if (check.Any())
            {
                foreach (var item in _kiRepository.GetAllKiThucTaps())
                {
                    if (item is not null && item.Ten.ToUpper().Equals(ki.Name.ToUpper()))
                    {
                        return new BadRequestObjectResult("This name already taken");
                    }
                }
            }

            existingKi.Ten = ki.Name;
            existingKi.NgayBatDau = ki.NgayBatDau;
            existingKi.NgayKetThuc = ki.NgayKetThuc;
            existingKi.LastUpdatedBy = uId;

            var result = _kiRepository.UpdateKiThucTap(existingKi);

            if (result == 0)
            {
                return new BadRequestObjectResult("Unsuccess");
            }

            return new OkObjectResult("Success");
        }
    }
}
