﻿using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.DTO.DuAn;
using AmazingTech.InternSystem.Models.Request.KiThucTap;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmazingTech.InternSystem.Services
{
    public interface IDuAnService
    {
        //DuAn methods
        IActionResult SearchProject(string ten, string leaderName, DateTime? startDate, DateTime? endDate);
        IActionResult GetAllDuAns();
        IActionResult GetDuAnById(string id);
        IActionResult CreateDuAn(string user, CrudDuAnModel createDuAn);
        IActionResult UpdateDuAn(string id, string user, CrudDuAnModel updatedDuAn);
        IActionResult DeleteDuAn(string id, string user);

        //UserDuAn methods
        IActionResult GetAllUsersInDuAn(string duAnId);
        IActionResult GetAllDuAnsOfUser(string userId);
        IActionResult AddUserToDuAn(string duAnId, string user, UserDuAnModel addUserDuAn);
        IActionResult UpdateUserInDuAn(string duAnId, string user, UserDuAnModel updateUserDuAn);
        IActionResult DeleteUserFromDuAn(string duAnId, string user, string userId);
    }
}
