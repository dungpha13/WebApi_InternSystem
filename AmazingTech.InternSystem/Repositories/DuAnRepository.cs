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
            //using (var context = new AppDbContext())
            //{
            //    var duAns = context.Set<DuAn>().Include(duAn => duAn.Leader).ToList();
            //    return duAns;
            //}
            var duAns = _dbContext.DuAns
                .Include(duAn => duAn.Leader)
                .ToList();

            return duAns;
        }

        public DuAn? GetDuAnById(string id)
        {
            //var duAn = await _dbContext.DuAns
                //.Include(d => d.Leader)
                //.Include(d => d.UserDuAns)
                //    .ThenInclude(uda => uda.User)
                //.Include(d => d.CongNgheDuAns)
                //    .ThenInclude(cnda => cnda.CongNghe)
                //.FirstOrDefaultAsync(c => c.Id == id);

            //return duAn;
            using (var context = new AppDbContext())
            {
                //return context.DuAns.FirstOrDefault(duan => duan.Id == id);
                //return context.DuAns.Include(d => d.Leader)
                //                    .Include(d => d.UserDuAns)
                //                        .ThenInclude(uda => uda.User)
                //                    .Include(d => d.CongNgheDuAns)
                //                        .ThenInclude(cnda => cnda.CongNghe)
                //                    .FirstOrDefault(c => c.Id == id);
                var duAn = _dbContext.DuAns
                .Include(d => d.Leader)
                .Include(d => d.UserDuAns)
                    .ThenInclude(uda => uda.User)
                .Include(d => d.CongNgheDuAns)
                    .ThenInclude(cnda => cnda.CongNghe)
                .FirstOrDefault(c => c.Id == id);

                return duAn;
            }
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

        public int CreateDuAn(DuAn createDuAn)
        {
            //_dbContext.Set<DuAn>().Add(createDuAn);
            //return _dbContext.SaveChanges();
            var existingDuAn = _dbContext.DuAns.FirstOrDefault(d => d.Ten == createDuAn.Ten);

            if (existingDuAn != null)
            {
                return -1;
            }

            _dbContext.Set<DuAn>().Add(createDuAn);
            return _dbContext.SaveChanges();
        }

        public int UpdateDuAn(DuAn updatedDuAn)
        {
            //_dbContext.DuAns.Update(updatedDuAn);
            //return _dbContext.SaveChanges();
            var existDuAn = _dbContext.DuAns.FirstOrDefault(c => c.Id == updatedDuAn.Id);

            if (existDuAn != null)
            {
                existDuAn.Ten = updatedDuAn.Ten;
                existDuAn.LeaderId = updatedDuAn.LeaderId;
                existDuAn.ThoiGianBatDau = updatedDuAn.ThoiGianBatDau;
                existDuAn.ThoiGianKetThuc = updatedDuAn.ThoiGianKetThuc;

                return _dbContext.SaveChanges();
            }

            return 0;
        }

        public int DeleteDuAn(DuAn deleteDuAn)
        {
            //_dbContext.DuAns.Remove(deleteDuAn);
            //return _dbContext.SaveChanges();
            var duAnToDelete = _dbContext.DuAns.FirstOrDefault(c => c.Id == deleteDuAn.Id);

            if (duAnToDelete != null)
            {
                duAnToDelete.DeletedTime = DateTime.Now;
                duAnToDelete.DeletedBy = deleteDuAn.DeletedBy;
                return _dbContext.SaveChanges();
            }
            return 0;
        }
    }
}
