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
            using (var context = new AppDbContext())
            {
                var duAns = context.Set<DuAn>().Include(duAn => duAn.Leader).ToList();
                return duAns;
            }
        }

        public DuAn? GetDuAnById(string id)
        {
            //var duAn = await _dbContext.DuAns
            //    //.Include(d => d.Leader)
            //    //.Include(d => d.UserDuAns)
            //    //    .ThenInclude(uda => uda.User)
            //    //.Include(d => d.CongNgheDuAns)
            //    //    .ThenInclude(cnda => cnda.CongNghe)
            //    .FirstOrDefaultAsync(c => c.Id == id);

            //return duAn;
            using (var context = new AppDbContext())
            {
                return context.DuAns.FirstOrDefault(duan => duan.Id == id);
            }
        }

        public List<DuAn> SearchProject(DuAnFilterCriteria criteria)
        {
            var query = _dbContext.DuAns
                .Include(d => d.Leader)
                .Include(d => d.UserDuAns)
                    .ThenInclude(uda => uda.User)
                .Include(d => d.CongNgheDuAns)
                    .ThenInclude(cnda => cnda.CongNghe)
                .AsQueryable();

            if (!string.IsNullOrEmpty(criteria.Ten))
                query = query.Where(d => d.LeaderId.Contains(criteria.Ten));

            if (criteria.ThoiGianBatDau != null)
                query = query.Where(d => d.ThoiGianBatDau >= criteria.ThoiGianBatDau);

            if (criteria.ThoiGianKetThuc != null)
                query = query.Where(d => d.ThoiGianKetThuc <= criteria.ThoiGianKetThuc);

            // Sorting
            query = query.OrderBy(d => d.Ten);

            return query.ToList();
        }

        public int CreateDuAn(DuAn createDuAn)
        {
            //var duAn = new DuAn
            //{
            //    Id = createDuAn.Id,
            //    Ten = createDuAn.Ten,
            //    LeaderId = createDuAn.LeaderId,
            //    ThoiGianBatDau = createDuAn.ThoiGianBatDau,
            //    ThoiGianKetThuc = createDuAn.ThoiGianKetThuc,
            //    CreatedBy = createDuAn.CreatedBy,
            //    LastUpdatedBy = createDuAn.CreatedBy,
            //};
            //_dbContext.DuAns.Add(duAn);
            //await _dbContext.SaveChangesAsync();
            using (var context = new AppDbContext())
            {
                context.Set<DuAn>().Add(createDuAn);
                return context.SaveChanges();
            }
        }

        public int UpdateDuAn(DuAn updatedDuAn)
        {

            //var existDuAn = await _dbContext.DuAns.FirstOrDefaultAsync(c => c.Id == id);


            //if (existDuAn != null)
            //{
            //    if (updatedDuAn.Ten != null) existDuAn.Ten = updatedDuAn.Ten;
            //    if (updatedDuAn.LeaderId != null) existDuAn.LeaderId = updatedDuAn.LeaderId;
            //    if (updatedDuAn.ThoiGianBatDau != null) existDuAn.ThoiGianBatDau = updatedDuAn.ThoiGianBatDau;
            //    if (updatedDuAn.ThoiGianKetThuc != null) existDuAn.ThoiGianKetThuc = updatedDuAn.ThoiGianKetThuc;
            //    existDuAn.LastUpdatedBy = updatedDuAn.LastUpdatedBy;
            //    await _dbContext.SaveChangesAsync();
            //}
            _dbContext.DuAns.Update(updatedDuAn);
            return _dbContext.SaveChanges();

        }

        public int DeleteDuAn(DuAn deleteDuAn)
        {

            //var DuAnToDelete = await _dbContext.CongNghes.FirstOrDefaultAsync(c => c.Id == id);

            //if (DuAnToDelete != null)
            //{
            //    DuAnToDelete.DeletedBy = deleteDuAn.DeletedBy;
            //    DuAnToDelete.DeletedTime = DateTime.Now;
            //    await _dbContext.SaveChangesAsync();
            //}
            _dbContext.DuAns.Remove(deleteDuAn);
            return _dbContext.SaveChanges();
        }
    }
}
