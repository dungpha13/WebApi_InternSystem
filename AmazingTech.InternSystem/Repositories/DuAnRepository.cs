using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.DTO;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AmazingTech.InternSystem.Data.Enum.Enums;

namespace AmazingTech.InternSystem.Repositories
{
    public class DuAnRepository : IDuAnRepo
    {
        private readonly AppDbContext _dbContext;

        public DuAnRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //DuAn methods
        public List<DuAn> GetAllDuAns()
        {
            var duAns = _dbContext.DuAns
                .Where(duAn => duAn.DeletedBy == null)
                .OrderByDescending(intern => intern.CreatedTime)
                .Include(duAn => duAn.Leader)
                .ToList();

            return duAns;
        }

        public DuAn GetDuAnById(string id)
        {
            var duAn = _dbContext.DuAns
                .Where(duAn => duAn.DeletedBy == null)
                .Include(duAn => duAn.Leader)
                .FirstOrDefault(c => c.Id == id);

            return duAn;
        }

        public DuAn GetDuAnByName(string projectName)
        {
            return _dbContext.DuAns.FirstOrDefault(d => d.Ten == projectName && d.DeletedBy == null)!;
        }

        public List<DuAnModel> SearchProject(string? ten, string? leaderName, DateTime? startDate, DateTime? endDate)
        {
            var query = _dbContext.DuAns.AsQueryable();

            if (!string.IsNullOrEmpty(ten))
            {
                query = query.Where(d => d.Ten.Contains(ten));
            }
            if (!string.IsNullOrEmpty(leaderName))
            {
                query = query.Where(d => d.Leader.HoVaTen != null && d.Leader.HoVaTen.Contains(leaderName));
            }
            if (startDate.HasValue)
            {
                query = query.Where(d => d.ThoiGianBatDau >= startDate);
            }
            if (endDate.HasValue)
            {
                query = query.Where(d => d.ThoiGianKetThuc <= endDate);
            }

            // Sorting
            query = query.OrderBy(d => d.Ten);

            var result = query.Select(d => new DuAnModel
            {
                Ten = d.Ten,
                LeaderId = d.LeaderId,
                LeaderName = d.Leader.HoVaTen,
                ThoiGianBatDau = d.ThoiGianBatDau,
                ThoiGianKetThuc = d.ThoiGianKetThuc,
            });

            return result.ToList();

        }

        public int CreateDuAn(string user, DuAn createDuAn)
        {
            try
            {
                var existingDuAn = GetDuAnByName(createDuAn.Ten);
                if (existingDuAn != null)
                {
                    return -1;
                }

                createDuAn.CreatedBy = user;
                createDuAn.LastUpdatedBy = user;
                createDuAn.LastUpdatedTime = DateTime.Now;

                _dbContext.DuAns.Add(createDuAn);
                _dbContext.SaveChanges();

                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating project: {ex.Message}");
                return 0;
            }
        }

        public int UpdateDuAn(string duAnId, string user, DuAn updatedDuAn)
        {
            var existingDuAn = _dbContext.DuAns.SingleOrDefault(d => d.Id == duAnId && d.DeletedBy == null);
            if (existingDuAn == null)
            {
                return 0;
            }

            if (updatedDuAn.Ten != null)
            {
                existingDuAn.Ten = updatedDuAn.Ten;
                var check = _dbContext.DuAns.SingleOrDefault(x => x.Ten == existingDuAn.Ten && x.DeletedBy == null && x.Id != existingDuAn.Id);
                if (check != null)
                {
                    return 0;
                }
            }

            existingDuAn.LeaderId = updatedDuAn.LeaderId;
            existingDuAn.ThoiGianBatDau = updatedDuAn.ThoiGianBatDau;
            existingDuAn.ThoiGianKetThuc = updatedDuAn.ThoiGianKetThuc;
            existingDuAn.LastUpdatedBy = user;
            existingDuAn.LastUpdatedTime = DateTime.Now;

            return _dbContext.SaveChanges();
        }

        public int DeleteDuAn(string duAnId, string user)
        {
            var duAnToDelete = _dbContext.DuAns.SingleOrDefault(d => d.Id == duAnId && d.DeletedBy == null);
            if (duAnToDelete == null)
            {
                return 0;
            }

            duAnToDelete.DeletedBy = user;
            duAnToDelete.DeletedTime = DateTime.Now;

            //_dbContext.Remove(duAnToDelete);
            return _dbContext.SaveChanges();
        }

        // UserDuAn methods
        public List<UserDuAn> GetAllUsersInDuAn(string duAnId)
        {
            return _dbContext.UserDuAns.Where(x => x.IdDuAn == duAnId && x.DeletedBy == null)
                                                    .Include(nz => nz.DuAn)
                                                    .ToList();
        }

        public int AddUserToDuAn(string duAnId, string user, UserDuAn addUserDuAn)
        {
            var duAn = _dbContext.DuAns.Find(duAnId);

            if (duAn == null)
            {
                throw new Exception($"Project with ID {duAnId} not found.");
            }

            addUserDuAn.IdDuAn = duAnId;

            addUserDuAn.CreatedBy = user;
            addUserDuAn.LastUpdatedBy = user;
            addUserDuAn.LastUpdatedTime = DateTime.Now;

            _dbContext.UserDuAns.Add(addUserDuAn);
            return _dbContext.SaveChanges();
        }

        public int UpdateUserInDuAn(string duAnId, string user, UserDuAn updateUserDuAn)
        {
            var userDuAn = _dbContext.UserDuAns.FirstOrDefault(x => x.UserId == updateUserDuAn.UserId && x.IdDuAn == duAnId && x.DeletedBy == null);

            if (userDuAn == null)
            {
                throw new Exception($"UserDuAn with ID {updateUserDuAn.UserId} in Project with ID {updateUserDuAn.IdDuAn} not found.");
            }

            userDuAn.UserId = updateUserDuAn.UserId;
            userDuAn.IdDuAn = updateUserDuAn.IdDuAn;

            userDuAn.LastUpdatedBy = user;
            userDuAn.LastUpdatedTime = DateTime.Now;

            return _dbContext.SaveChanges();
        }

        public int DeleteUserFromDuAn(string user, string userId, string duAnId)
        {
            var userDuAn = _dbContext.UserDuAns.FirstOrDefault(x => x.UserId == userId && x.IdDuAn == duAnId && x.DeletedBy == null);

            if (userDuAn == null)
            {
                throw new Exception($"UserDuAn with ID {userId} in DuAn with ID {duAnId} not found.");
            }

            userDuAn.DeletedBy = user;
            userDuAn.DeletedTime = DateTime.Now;

            return _dbContext.SaveChanges();
        }

        //Intern DuAn methods
        //public int AddInternToDuAn(string duAnId, string mssv, InternInfo internInfo)
        //{
        //    try
        //    {
        //        var duAn = _dbContext.DuAns.Find(duAnId);
        //        if (duAn != null)
        //        {
        //            duAn.InternInfos.Add(internInfo);
        //            _dbContext.SaveChanges();
        //            return 1;
        //        }
        //        return 0;
        //    }
        //    catch (Exception)
        //    {
        //        return -1;
        //    }
        //}

        //public int UpdateInternInDuAn(InternInfo internInfo)
        //{
        //    try
        //    {
        //        _dbContext.InternInfos.Update(internInfo);
        //        _dbContext.SaveChanges();
        //        return 1;
        //    }
        //    catch (Exception)
        //    {
        //        return 0;
        //    }
        //}

        //public int RemoveInternFromDuAn(string internInfoId)
        //{
        //    try
        //    {
        //        var internInfo = _dbContext.InternInfos.FirstOrDefault(i => i.Id == internInfoId);
        //        if (internInfo != null)
        //        {
        //            _dbContext.InternInfos.Remove(internInfo);
        //            _dbContext.SaveChanges();
        //            return 1;
        //        }
        //        return 0;
        //    }
        //    catch (Exception)
        //    {
        //        return -1;
        //    }
        //}
    }
}
