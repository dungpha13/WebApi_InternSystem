using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request.DuAn;
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

        public List<DuAn> GetAllDuAns()
        {
            var duAns = _dbContext.DuAns
                .Include(duAn => duAn.Leader)
                .ToList();

            return duAns;
        }

        public DuAn GetDuAnById(string id)
        {
            var duAn = _dbContext.DuAns
                .Include(duAn => duAn.Leader)
                .FirstOrDefault(c => c.Id == id);

            return duAn;
        }

        public DuAn GetDuAnByName(string projectName)
        {
            return _dbContext.DuAns.FirstOrDefault(d => d.Ten == projectName && d.DeletedBy == null)!;
        }

        public List<DuAnModel> SearchProject(string? ten, string? leaderName)
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
            //if (startDate != DateTime.MinValue)
            //{
            //    query = query.Where(d => d.ThoiGianBatDau >= startDate);
            //}
            //if (endDate != DateTime.MaxValue)
            //{
            //    query = query.Where(d => d.ThoiGianKetThuc <= endDate);
            //}

            // Sorting
            query = query.OrderBy(d => d.Ten);

            var result = query.Select(d => new DuAnModel
            {
                Ten = d.Ten,
                LeaderId = d.LeaderId,
                LeaderName = d.Leader.HoVaTen ?? "",
                ThoiGianBatDau = d.ThoiGianBatDau,
                ThoiGianKetThuc = d.ThoiGianKetThuc,
            });

            return result.ToList();

        }

        public int CreateDuAn(DuAn createDuAn)
        {
            try
            {
                var existingDuAn = GetDuAnByName(createDuAn.Ten);
                if (existingDuAn != null)
                {
                    return -1;
                }

                createDuAn.CreatedBy = "Admin";
                createDuAn.LastUpdatedBy = "Admin";
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

        public int UpdateDuAn(string duAnId, DuAn updatedDuAn)
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
            existingDuAn.LastUpdatedBy = "Admin";
            existingDuAn.LastUpdatedTime = DateTime.Now;

            return _dbContext.SaveChanges();
        }

        public int DeleteDuAn(string duAnId)
        {
            var duAnToDelete = _dbContext.DuAns.SingleOrDefault(d => d.Id == duAnId && d.DeletedBy == null);
            if (duAnToDelete == null)
            {
                return 0;
            }

            var currentTime = DateTime.Now;

            duAnToDelete.DeletedBy = "Admin";
            duAnToDelete.DeletedTime = currentTime;

            //_dbContext.Remove(duAnToDelete);
            return _dbContext.SaveChanges();
        }
    }
}
