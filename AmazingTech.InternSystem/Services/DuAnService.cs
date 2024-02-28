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
            DuAn duAn = _duAnRepo.GetDuAnById(id);
            return new OkObjectResult(duAn);
        }

        public IActionResult CreateDuAn(string user, CrudDuAnModel createDuAn)
        {
            DuAn duAn = _mapper.Map<DuAn>(createDuAn);
            var result = _duAnRepo.CreateDuAn(user, duAn);
            return new OkObjectResult(result);
        }

        public IActionResult UpdateDuAn(string id, string user, CrudDuAnModel updatedDuAn)
        {
            DuAn duAn = _mapper.Map<DuAn>(updatedDuAn);
            var result = _duAnRepo.UpdateDuAn(id, user, duAn);
            return new OkObjectResult(result);
        }

        public IActionResult DeleteDuAn(string id, string user)
        {
            var result = _duAnRepo.DeleteDuAn(id, user);
            return new OkObjectResult(result);
        }

        // UserDuAn methods
        public IActionResult GetAllUsersInDuAn(string duAnId)
        {
            List<UserDuAn> userDuAns = _duAnRepo.GetAllUsersInDuAn(duAnId);
            return new OkObjectResult(userDuAns);
        }

        public IActionResult AddUserToDuAn(string duAnId, string user, UserDuAnModel addUserDuAn)
        {            
            UserDuAn userDuAn = _mapper.Map<UserDuAn>(addUserDuAn);
            var result = _duAnRepo.AddUserToDuAn(duAnId, user, userDuAn);
            return new OkObjectResult(result);
        }

        public IActionResult UpdateUserInDuAn(string duAnId, string user, UserDuAnModel updateUserDuAn)
        {
            UserDuAn userDuAn = _mapper.Map<UserDuAn>(updateUserDuAn);
            var result = _duAnRepo.UpdateUserInDuAn(duAnId, user, userDuAn);
            return new OkObjectResult(result);
        }

        public IActionResult DeleteUserFromDuAn(string duAnId, string user, string userId)
        {
            var result = _duAnRepo.DeleteUserFromDuAn(duAnId, user, userId);
            return new OkObjectResult(result);
        }
    }
}
