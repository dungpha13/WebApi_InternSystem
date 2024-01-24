using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request.DuAn;
using AmazingTech.InternSystem.Repositories;
using AutoMapper;
using DocumentFormat.OpenXml.Office2016.Excel;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmazingTech.InternSystem.Services
{
    public class DuAnService : IDuAnService
    {
        private readonly IDuAnRepo _duAnRepo;
        private readonly IMapper _mapper;

        public DuAnService(IDuAnRepo duAnRepo, IMapper mapper)
        {
            _duAnRepo = duAnRepo;
            _mapper = mapper;
        }

        public IActionResult SearchProject(DuAnFilterCriteria criteria)
        {
            var duAns = _duAnRepo.SearchProject(criteria);
            var duAnModels = _mapper.Map<List<DuAnModel>>(duAns);
            return new OkObjectResult(duAnModels);
        }

        public IActionResult GetAllDuAns()
        {
            //List<DuAn> duAns = _duAnRepo.GetAllDuAns();
            //return new OkObjectResult(duAns);
            var duAns = _duAnRepo.GetAllDuAns();
            var duAnResponseDTOs = _mapper.Map<List<DuAnResponseDTO>>(duAns);
            var formattedResponse = new { value = duAnResponseDTOs };
            return new OkObjectResult(formattedResponse);
        }

        public IActionResult GetDuAnById(string id)
        {
            //var duAn = _duAnRepo.GetDuAnById(id);
            //return new OkObjectResult(duAn);
            var duAn = _duAnRepo.GetDuAnById(id);

            if (duAn == null)
            {
                return new NotFoundResult();
            }

            var duAnResponseDTO = _mapper.Map<DuAnResponseDTO>(duAn);
            return new OkObjectResult(duAnResponseDTO);
        }

        public IActionResult CreateDuAn(AddDuAnModel createDuAn)
        {
            //_duAnRepo.CreateDuAn(_mapper.Map<DuAn>(createDuAn));
            //return new OkResult();
            int result = _duAnRepo.CreateDuAn(_mapper.Map<DuAn>(createDuAn));

            if (result == -1)
            {
                return new BadRequestObjectResult("Project with the same name already exists");
            }

            return new OkResult();
        }

        public IActionResult UpdateDuAn(UpdateDuAnModel updatedDuAn)
        {
            var existDuAn = _duAnRepo.GetDuAnById(updatedDuAn.Id);

            if (existDuAn is null)
            {
                return new BadRequestObjectResult("Id null!");
            }

            existDuAn.ThoiGianBatDau = updatedDuAn.ThoiGianBatDau;
            existDuAn.ThoiGianKetThuc = updatedDuAn.ThoiGianKetThuc;

            _duAnRepo.UpdateDuAn(existDuAn);
            return new NoContentResult();
        }

        public IActionResult DeleteDuAn(string id)
        {
            var existDuAn = _duAnRepo.GetDuAnById(id);

            if (existDuAn is not null)
            {
                _duAnRepo.DeleteDuAn(existDuAn);
            }

            return new NoContentResult();
        }

        //public Task<byte[]> ExportProjectsToExcelAsync(List<string> duAnIds)
        //{
        //    // Implement the logic for exporting projects to Excel.
        //    // Example: return _duAnRepo.ExportProjectsToExcelAsync(duAnIds);
        //}
    }
}
