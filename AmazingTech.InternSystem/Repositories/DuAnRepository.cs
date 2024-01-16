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

        public async Task<List<DuAn>> GetAllDuAnsAsync()
        {
            try
            {
                return await _dbContext.DuAns.ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in GetAllDuAnsAsync");
                throw;
            }


        }

        public async Task<DuAn> GetDuAnByIdAsync(string id)
        {
            var duAn = await _dbContext.DuAns
                .Include(d => d.Leader)
                .Include(d => d.UserDuAns)
                    .ThenInclude(uda => uda.User)
                .Include(d => d.CongNgheDuAns)
                .FirstOrDefaultAsync(c => c.Id == id);

            return duAn;
        }

        public async Task<List<DuAn>> SearchProjectsAsync(DuAnFilterCriteria criteria)
        {
            try
            {
                var query = _dbContext.DuAns
                    .Include(d => d.Leader)
                    .Include(d => d.UserDuAns)
                        .ThenInclude(uda => uda.User)
                    .Include(d => d.CongNgheDuAns)
                        .ThenInclude(cnda => cnda.CongNghe)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(criteria.Ten))
                    query = query.Where(d => d.Ten.Contains(criteria.Ten));

                if (criteria.ThoiGianBatDau != null)
                    query = query.Where(d => d.ThoiGianBatDau >= criteria.ThoiGianBatDau);

                if (criteria.ThoiGianKetThuc != null)
                    query = query.Where(d => d.ThoiGianKetThuc <= criteria.ThoiGianKetThuc);

                // Sorting
                query = query.OrderBy(d => d.Ten);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in SearchProjectsAsync");
                throw;
            }
        }

        public async Task CreateDuAnAsync(DuAn createDuAn)
        {
            try
            {
                var duAn = new DuAn
                {
                    Id = createDuAn.Id,
                    Ten = createDuAn.Ten,
                    LeaderId = createDuAn.LeaderId,
                    ThoiGianBatDau = createDuAn.ThoiGianBatDau,
                    ThoiGianKetThuc = createDuAn.ThoiGianKetThuc,
                    CreatedBy = createDuAn.CreatedBy,
                    LastUpdatedBy = createDuAn.CreatedBy,
                };
                _dbContext.DuAns.Add(duAn);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in CreateDuAnAsync");
                throw;
            }
        }

        public async Task UpdateDuAnAsync(string id, DuAn updatedDuAn)
        {
            try
            {
                var existDuAn = await _dbContext.DuAns.FirstOrDefaultAsync(c => c.Id == id);


                if (existDuAn != null)
                {
                    if (updatedDuAn.Ten != null) existDuAn.Ten = updatedDuAn.Ten;
                    if (updatedDuAn.LeaderId != null) existDuAn.LeaderId = updatedDuAn.LeaderId;
                    if (updatedDuAn.ThoiGianBatDau != null) existDuAn.ThoiGianBatDau = updatedDuAn.ThoiGianBatDau;
                    if (updatedDuAn.ThoiGianKetThuc != null) existDuAn.ThoiGianKetThuc = updatedDuAn.ThoiGianKetThuc;
                    existDuAn.LastUpdatedBy = updatedDuAn.LastUpdatedBy;
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in UpdateDuAnAsync (outer catch)");
                throw;
            }
        }

        public async Task DeleteDuAnAsync(string id, DuAn deleteDuAn)
        {
            try
            {
                var DuAnToDelete = await _dbContext.CongNghes.FirstOrDefaultAsync(c => c.Id == id);

                if (DuAnToDelete != null)
                {
                    DuAnToDelete.DeletedBy = deleteDuAn.DeletedBy;
                    DuAnToDelete.DeletedTime = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in DeleteDuAnAsync (outer catch)");
                throw;
            }
        }
    }
}
