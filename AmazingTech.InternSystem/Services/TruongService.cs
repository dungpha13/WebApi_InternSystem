﻿using System.Security.Claims;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request.TruongHoc;
using AmazingTech.InternSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Services
{
    public class TruongService : ITruongService
    {
        private ITruongRepository _truongRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TruongService(ITruongRepository truongRepository, IHttpContextAccessor httpContextAccessor)
        {
            _truongRepository = truongRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult AddTruong(AddTruongHocDTO request)
        {
            var uId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            TruongHoc truong = new TruongHoc()
            {
                CreatedBy = uId,
                Ten = request.Ten,
                SoTuanThucTap = request.SoTuanThucTap
            };

            var listTruong = _truongRepository.GetAllTruongs();

            if (listTruong.Any() && listTruong != null)
            {
                foreach (var trg in _truongRepository.GetAllTruongs())
                {
                    if (trg.Ten.Equals(request.Ten))
                    {
                        return new BadRequestObjectResult("Truong da ton tai.");
                    }
                }
            }

            var result = _truongRepository.AddTruong(truong);

            if (result == 0)
            {
                return new BadRequestObjectResult("Unsuccess");
            }

            return new OkObjectResult("Success");
        }

        public async Task<IActionResult> DeleteTruong(string id)
        {
            var uId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var existingTruong = _truongRepository.GetTruong(id);

            if (existingTruong is null)
            {
                return new BadRequestObjectResult($"Truong voi id {id} khong ton tai");
            }

            var result = await _truongRepository.DeleteTruong(existingTruong, uId);

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
            var uId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var existingTruong = _truongRepository.GetTruong(truong.Id);

            if (existingTruong is null)
            {
                return new BadRequestObjectResult($"Truong voi id {truong.Id} khong ton tai");
            }

            foreach (var truongInList in _truongRepository.GetAllTruongs())
            {
                if (truongInList.Ten.Equals(truong.Ten))
                {
                    return new BadRequestObjectResult("Truong da ton tai.");
                }
            }

            existingTruong.Ten = truong.Ten ?? existingTruong.Ten;
            existingTruong.SoTuanThucTap = truong.SoTuanThucTap;
            existingTruong.LastUpdatedBy = uId;

            var result = _truongRepository.UpdateTruong(existingTruong);

            if (result == 0)
            {
                return new BadRequestObjectResult("Unsuccess");
            }

            return new OkObjectResult("Success");
        }

    }
}