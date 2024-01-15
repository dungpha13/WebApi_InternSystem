using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models;
using AmazingTech.InternSystem.Models.Request.DuAn;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Serilog;
using System.Data;
using System.Drawing;

namespace AmazingTech.InternSystem.Repositories
{
    public class DuAnRepository : IDuAnRepo
    {
        private readonly AppDbContext _dbContext;
        private DuAnFilterCriteria _currentFilterCriteria = new DuAnFilterCriteria();

        public DuAnRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<DuAn>> SearchProjectsAsync(DuAnFilterCriteria criteria)
        {
            try
            {
                var query = _dbContext.DuAns.AsQueryable();

                if (!string.IsNullOrEmpty(criteria.Ten))
                    query = query.Where(d => d.Ten.Contains(criteria.Ten));

                if (criteria.ThoiGianBatDauFrom != null)
                    query = query.Where(d => d.ThoiGianBatDau >= criteria.ThoiGianBatDauFrom);

                if (criteria.ThoiGianBatDauTo != null)
                    query = query.Where(d => d.ThoiGianBatDau <= criteria.ThoiGianBatDauTo);

                if (criteria.ThoiGianKetThucFrom != null)
                    query = query.Where(d => d.ThoiGianKetThuc >= criteria.ThoiGianKetThucFrom);

                if (criteria.ThoiGianKetThucTo != null)
                    query = query.Where(d => d.ThoiGianKetThuc <= criteria.ThoiGianKetThucTo);

                if (!string.IsNullOrEmpty(criteria.LeaderId))
                    query = query.Where(d => d.LeaderId == criteria.LeaderId);

                //if (!string.IsNullOrEmpty(criteria.IdCongNghe))
                //    query = query.Where(d => d.CongNgheIds.Contains(criteria.IdCongNghe));

                if (!string.IsNullOrEmpty(criteria.IdDuAn))
                    query = query.Where(d => d.Id == criteria.IdDuAn);

                if (!string.IsNullOrEmpty(criteria.UserId))
                {
                    query = query.Where(d => d.UserDuAns.Any(uda => uda.UserId == criteria.UserId));
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in SearchProjectsAsync");
                throw;
            }
        }

        public async Task CleanFiltersAsync()
        {
            try
            {
                var defaultCriteria = new DuAnFilterCriteria();
                _currentFilterCriteria = defaultCriteria;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in CleanFiltersAsync");
                throw;
            }
        }


        public async Task<string> CreateDuAnAsync(DuAn duAn, List<string> viTriIds, List<string> congNgheIds, List<string> leaderUserIds)
        {
            try
            {
                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        _dbContext.DuAns.Add(duAn);

                        foreach (var viTriId in viTriIds)
                        {
                            var viTri = new ViTri { Id = viTriId };
                            _dbContext.Attach(viTri);
                            duAn.ViTris.Add(viTri);
                        }

                        foreach (var congNgheId in congNgheIds)
                        {
                            var congNghe = new CongNghe { Id = congNgheId };
                            _dbContext.Attach(congNghe);
                            duAn.CongNghes.Add(congNghe);
                        }

                        foreach (var leaderUserId in leaderUserIds)
                        {
                            var leaderUser = new User { Id = leaderUserId };
                            _dbContext.Attach(leaderUser);
                            duAn.Leader = leaderUser;
                        }

                        await _dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return duAn.Id;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in CreateDuAnAsync (outer catch)");
                throw;
            }
        }

        public async Task UpdateDuAnAsync(string id, DuAn updatedDuAn, List<string> viTriIds, List<string> congNgheIds, List<string> leaderUserIds)
        {
            try
            {
                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var existingDuAn = await _dbContext.DuAns
                            .Include(d => d.ViTris)
                            .Include(d => d.CongNghes)
                            .Include(d => d.Leader)
                            .FirstOrDefaultAsync(d => d.Id == id);

                        if (existingDuAn == null)
                        {
                            return;
                        }

                        existingDuAn.Ten = updatedDuAn.Ten;
                        existingDuAn.ThoiGianBatDau = updatedDuAn.ThoiGianBatDau;
                        existingDuAn.ThoiGianKetThuc = updatedDuAn.ThoiGianKetThuc;

                        existingDuAn.ViTris.Clear();
                        existingDuAn.CongNghes.Clear();
                        existingDuAn.Leader = null;

                        foreach (var viTriId in viTriIds)
                        {
                            var viTri = new ViTri { Id = viTriId };
                            _dbContext.Attach(viTri);
                            existingDuAn.ViTris.Add(viTri);
                        }

                        foreach (var congNgheId in congNgheIds)
                        {
                            var congNghe = new CongNghe { Id = congNgheId };
                            _dbContext.Attach(congNghe);
                            existingDuAn.CongNghes.Add(congNghe);
                        }

                        foreach (var leaderUserId in leaderUserIds)
                        {
                            var leaderUser = new User { Id = leaderUserId };
                            _dbContext.Attach(leaderUser);
                            existingDuAn.Leader = leaderUser;
                        }

                        await _dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in UpdateDuAnAsync (outer catch)");
                throw;
            }
        }

        public async Task DeleteDuAnAsync(string id)
        {
            try
            {
                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var existingDuAn = await _dbContext.DuAns
                            .Include(d => d.ViTris)
                            .Include(d => d.CongNghes)
                            .Include(d => d.Leader)
                            .FirstOrDefaultAsync(d => d.Id == id);

                        if (existingDuAn == null)
                        {
                            return;
                        }

                        _dbContext.DuAns.Remove(existingDuAn);
                        await _dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in DeleteDuAnAsync (outer catch)");
                throw;
            }
        }

        public async Task<DuAn?> GetDuAnForEditAsync(string id)
        {
            try
            {
                var duAn = await _dbContext.DuAns
                    .Include(d => d.ViTris)
                    .Include(d => d.CongNghes)
                    .Include(d => d.Leader)
                    .FirstOrDefaultAsync(d => d.Id == id);

                return duAn;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in GetDuAnForEditAsync");
                throw;
            }
        }

        //public async Task<byte[]> ExportProjectsToExcelAsync(List<string> duAnIds)
        //{
        //    try
        //    {
        //        var projects = await _dbContext.DuAns
        //            .Include(d => d.ViTris)
        //            .Include(d => d.CongNghes)
        //            .Include(d => d.Leader)
        //            .Where(d => duAnIds.Contains(d.Id))
        //            .ToListAsync();

        //        using (var package = new ExcelPackage())
        //        {
        //            foreach (var project in projects)
        //            {
        //                AddProjectSheet(package, project);
        //            }

        //            using (var stream = new MemoryStream())
        //            {
        //                package.SaveAs(stream);
        //                return stream.ToArray();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return new byte[0];
        //    }
        //}

        //// Helper method to add a sheet to the Excel package
        ////private void AddProjectSheet(ExcelPackage package, DuAn project)
        ////{
        ////    var worksheet = package.Workbook.Worksheets.Add(project.Ten); // Use project name as sheet name

        ////    worksheet.Cells["A1"].Value = "Project ID";
        ////    worksheet.Cells["B1"].Value = "Project Name";
        ////    worksheet.Cells["C1"].Value = "Start Date";
        ////    worksheet.Cells["D1"].Value = "End Date";
        ////    worksheet.Cells["E1"].Value = "Leader";

        ////    worksheet.Cells["A2"].Value = project.Id;
        ////    worksheet.Cells["B2"].Value = project.Ten;
        ////    worksheet.Cells["C2"].Value = project.ThoiGianBatDau?.ToShortDateString();
        ////    worksheet.Cells["D2"].Value = project.ThoiGianKetThuc?.ToShortDateString();
        ////    worksheet.Cells["E2"].Value = project.Leader?.UserName;
        ////}
    }
}
