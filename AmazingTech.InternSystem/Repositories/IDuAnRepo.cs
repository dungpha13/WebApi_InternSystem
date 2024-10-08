﻿using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.DTO.DuAn;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmazingTech.InternSystem.Repositories
{
    public interface IDuAnRepo
    {
        // DuAn methods
        List<DuAn> GetAllDuAns();
        DuAn GetDuAnById(string id);
        List<DuAnModel> SearchProject(string ten, string leaderName, DateTime? startDate, DateTime? endDate);
        DuAn GetDuAnByName(string projectName);
        int CreateDuAn(string user, DuAn createDuAn);
        int UpdateDuAn(string duAnId, string user, DuAn updatedDuAn);
        int DeleteDuAn(string duAnId, string user);

        // UserDuAn methods
        List<UserDuAn> GetAllUsersInDuAn(string duAnId);
        List<UserDuAn> GetAllDuAnsOfUser(string userId);
        int AddUserToDuAn(string duAnId, string user, UserDuAn addUserDuAn);
        int UpdateUserInDuAn(string duAnId,string user, UserDuAn updateUserDuAn);
        int DeleteUserFromDuAn(string duAnId, string user, string userId);
    }
}
