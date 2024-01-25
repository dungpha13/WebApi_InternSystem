using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request.DuAn;
using AutoMapper;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return _dbContext.DuAns.FirstOrDefault(d => d.Ten == projectName && d.DeletedBy == null);
        }

        public List<DuAn> SearchProject(DuAnFilterCriteria criteria)
        {
            var query = _dbContext.DuAns
                .Include(d => d.Leader)
                //.Include(d => d.UserDuAns)
                //    .ThenInclude(uda => uda.User)
                //.Include(d => d.CongNgheDuAns)
                //    .ThenInclude(cnda => cnda.CongNghe)
                .AsQueryable();

            if (!string.IsNullOrEmpty(criteria.Ten))
                query = query.Where(d => d.Ten.Contains(criteria.Ten));

            if (!string.IsNullOrEmpty(criteria.LeaderId))
                query = query.Where(d => d.LeaderId.Contains(criteria.LeaderId));

            if (criteria.ThoiGianBatDau != null)
                query = query.Where(d => d.ThoiGianBatDau >= criteria.ThoiGianBatDau);

            if (criteria.ThoiGianKetThuc != null)
                query = query.Where(d => d.ThoiGianKetThuc <= criteria.ThoiGianKetThuc);

            // Sorting
            query = query.OrderBy(d => d.Ten);

            return query.ToList();
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

        public int UpdateDuAn(string user, string duAnId, DuAn updatedDuAn)
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

        public int DeleteDuAn(string user, string duAnId)
        {
            var duAnToDelete = _dbContext.DuAns.SingleOrDefault(d => d.Id == duAnId && d.DeletedBy == null);
            if (duAnToDelete == null)
            {
                return 0;
            }

            duAnToDelete.DeletedBy = user;
            duAnToDelete.DeletedTime = DateTime.Now;

            _dbContext.Remove(duAnToDelete);
            _dbContext.SaveChanges();
            return 1;
        }
    }
}
