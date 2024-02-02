using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.DTO;
using AmazingTech.InternSystem.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

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

        //DuAn methods
        public IActionResult SearchProject(string ten, string leaderName, DateTime? startDate, DateTime? endDate)
        {
            var duAns = _duAnRepo.SearchProject(ten, leaderName, startDate, endDate);
            return new OkObjectResult(duAns);
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

        public IActionResult UpdateDuAn(string id, string user, DuAnModel updatedDuAn)
        {
            DuAn duAn = _mapper.Map<DuAn>(updatedDuAn);
            var result = _duAnRepo.UpdateDuAn(id, user, duAn);

            if (result == 0)
            {
                return new BadRequestObjectResult("An error occurred while updating the project");
            }

            return new OkResult();
        }

        public IActionResult DeleteDuAn(string id, string user)
        {
            var result = _duAnRepo.DeleteDuAn(id, user);

            if (result == 0)
            {
                return new BadRequestObjectResult("An error occurred while deleting the project");
            }

            return new OkResult();
        }

        // UserDuAn methods
        public IActionResult GetAllUsersInDuAn(string duAnId)
        {
            List<UserDuAn> userDuAns = _duAnRepo.GetAllUsersInDuAn(duAnId);
            return new OkObjectResult(userDuAns);
        }

        public IActionResult AddUserToDuAn(string duAnId,string user, UserDuAnModel addUserDuAn)
        {
            UserDuAn userDuAn = _mapper.Map<UserDuAn>(addUserDuAn);
            var result = _duAnRepo.AddUserToDuAn(duAnId, user, userDuAn);
            if (result == -1)
            {
                return new BadRequestObjectResult("UserDuAn already exists");
            }
            else if (result == 0)
            {
                return new BadRequestObjectResult("An error occurred while adding UserDuAn");
            }
            return new OkResult();
        }

        public IActionResult UpdateUserInDuAn(string duAnId, string user, UserDuAnModel updateUserDuAn)
        {
            UserDuAn userDuAn = _mapper.Map<UserDuAn>(updateUserDuAn);
            var result = _duAnRepo.UpdateUserInDuAn(duAnId, user, userDuAn);
            if (result == 0)
            {
                return new BadRequestObjectResult("UserDuAn does not exist");
            }
            else if (result == -1)
            {
                return new BadRequestObjectResult("An error occurred while updating UserDuAn");
            }
            return new OkResult();
        }

        public IActionResult DeleteUserFromDuAn(string user, string userId, string duAnId)
        {
            var result = _duAnRepo.DeleteUserFromDuAn(user, userId, duAnId);
            if (result == 0)
            {
                return new BadRequestObjectResult("UserDuAn does not exist");
            }
            else if (result == -1)
            {
                return new BadRequestObjectResult("An error occurred while deleting UserDuAn");
            }
            return new OkResult();
        }

        //Intern DuAn methods
        //public IActionResult AddInternToDuAn(string duAnId, InternInfo internInfo)
        //{
        //    var result = _duAnRepo.AddInternToDuAn(duAnId, internInfo);
        //    if (result == 0)
        //    {
        //        return new BadRequestObjectResult("DuAn does not exist");
        //    }
        //    else if (result == -1)
        //    {
        //        return new BadRequestObjectResult("An error occurred while adding intern to DuAn");
        //    }
        //    return new OkResult();
        //}

        //public IActionResult UpdateInternInDuAn(InternInfo internInfo)
        //{
        //    var result = _duAnRepo.UpdateInternInDuAn(internInfo);
        //    if (result == 0)
        //    {
        //        return new BadRequestObjectResult("An error occurred while updating intern in DuAn");
        //    }
        //    return new OkResult();
        //}

        //public IActionResult RemoveInternFromDuAn(string internInfoId)
        //{
        //    var result = _duAnRepo.RemoveInternFromDuAn(internInfoId);
        //    if (result == 0)
        //    {
        //        return new BadRequestObjectResult("InternInfo does not exist");
        //    }
        //    else if (result == -1)
        //    {
        //        return new BadRequestObjectResult("An error occurred while removing intern from DuAn");
        //    }
        //    return new OkResult();
        //}
    }
}
