using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.DTO;
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
            List<DuAn> duAns = _duAnRepo.GetAllDuAns();
            return new OkObjectResult(duAns);
        }

        public IActionResult GetDuAnById(string id)
        {
            var duAn = _duAnRepo.GetDuAnById(id);
            return new OkObjectResult(duAn);
        }

        public IActionResult CreateDuAn(string user, DuAnModel createDuAn)
        {
            try
            {
                DuAn duAn = _mapper.Map<DuAn>(createDuAn);

                var existingDuAn = _duAnRepo.GetDuAnByName(duAn.Ten);
                if (existingDuAn != null)
                {
                    return new BadRequestObjectResult("The project name already exists");
                }

                var result = _duAnRepo.CreateDuAn(user, duAn);

                if (result == -1)
                {
                    return new BadRequestObjectResult("Project with the same name already exists");
                }
                else if (result == 0)
                {
                    return new BadRequestObjectResult("An error occurred while creating the project");
                }

                return new OkResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating project: {ex.Message}");

                return new StatusCodeResult(500);
            }
        }

        public IActionResult UpdateDuAn(string user, string id, DuAnModel updatedDuAn)
        {
            DuAn duAn = _mapper.Map<DuAn>(updatedDuAn);
            var result = _duAnRepo.UpdateDuAn(user, id, duAn);

            if (result == 0)
            {
                return new BadRequestObjectResult("An error occurred while updating the project");
            }

            return new OkResult();
        }

        public IActionResult DeleteDuAn(string user, string id)
        {
            var result = _duAnRepo.DeleteDuAn(user, id);

            if (result == 0)
            {
                return new BadRequestObjectResult("An error occurred while deleting the project");
            }

            return new OkResult();
        }
    }
}
