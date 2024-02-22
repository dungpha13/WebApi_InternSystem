using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.DTO.DuAn;
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
            var existingDuAn = _duAnRepo.GetDuAnById(id);
            if (existingDuAn == null)
            {
                return new BadRequestObjectResult($"DuAn with ID {id} not found.");
            }

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
            var existingDuAn = _duAnRepo.GetDuAnById(id);
            if (existingDuAn == null)
            {
                return new BadRequestObjectResult($"DuAn with ID {id} not found.");
            }

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

        public IActionResult AddUserToDuAn(string duAnId, string user, UserDuAnModel addUserDuAn)
        {
            if (string.IsNullOrEmpty(duAnId))
            {
                return new BadRequestObjectResult("DuAnId is required");
            }

            var existingUserDuAn = _duAnRepo.GetAllUsersInDuAn(duAnId).FirstOrDefault(u => u.UserId == addUserDuAn.UserId);
            if (existingUserDuAn != null)
            {
                return new BadRequestObjectResult("User already exists in the project.");
            }

            UserDuAn userDuAn = _mapper.Map<UserDuAn>(addUserDuAn);
            var result = _duAnRepo.AddUserToDuAn(duAnId, user, userDuAn);

            if (result == 0)
            {
                return new BadRequestObjectResult("Failed to add user to the project.");
            }

            return new OkResult();
        }

        public IActionResult UpdateUserInDuAn(string duAnId, string user, UserDuAnModel updateUserDuAn)
        {
            if (string.IsNullOrEmpty(duAnId))
            {
                return new BadRequestObjectResult("DuAnId is required");
            }

            var existingUserDuAn = _duAnRepo.GetAllUsersInDuAn(duAnId).FirstOrDefault(u => u.UserId == updateUserDuAn.UserId);
            if (existingUserDuAn == null)
            {
                return new BadRequestObjectResult("User does not exist in the project.");
            }

            UserDuAn userDuAn = _mapper.Map<UserDuAn>(updateUserDuAn);
            var result = _duAnRepo.UpdateUserInDuAn(duAnId, user, userDuAn);

            if (result == 0)
            {
                return new BadRequestObjectResult("Failed to update user in the project.");
            }

            return new OkResult();
        }

        public IActionResult DeleteUserFromDuAn(string user, string userId, string duAnId)
        {
            if (string.IsNullOrEmpty(duAnId))
            {
                return new BadRequestObjectResult("DuAnId is required");
            }

            var existingUserDuAn = _duAnRepo.GetAllUsersInDuAn(duAnId).FirstOrDefault(u => u.UserId == userId);
            if (existingUserDuAn == null)
            {
                return new BadRequestObjectResult("User does not exist in the project.");
            }

            var result = _duAnRepo.DeleteUserFromDuAn(user, userId, duAnId);

            if (result == 0)
            {
                return new BadRequestObjectResult("Failed to delete user from the project.");
            }

            return new OkResult();
        }
    }
}
