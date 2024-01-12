﻿using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace AmazingTech.InternSystem.Repositories
{
    public interface ILichPhongVanRepository
    {
        public void addNewLichPhongVan(LichPhongVan entity);

        
       

        public void DeleteLichPhongVanByIdNguoiDuocPhongVan(string id);
        public List<LichPhongVan> GetLichPhongVan();
        public List<LichPhongVan> GetLichPhongVanByIdNgPhongVan(string id);
        public List<LichPhongVan> GetLichPhongVanByIdNguoiDuocPhongVan(string id);
    }
    public class LichPhongVanRepository : ILichPhongVanRepository
    {
        private DbSet<LichPhongVan> _dbSet;
        public LichPhongVanRepository()
        {

        }
        
        public void addNewLichPhongVan(LichPhongVan entity)
        {
            using(var context = new AppDbContext())
            {
                context.Set<LichPhongVan>().Add(entity);
                context.SaveChanges();
            }
        }

       

        public List<LichPhongVan> GetLichPhongVanByIdNguoiDuocPhongVan(string id)
        {
            using ( var context = new AppDbContext())
            {
                var list = context.Set<LichPhongVan>().AsNoTracking().Where(x => x.IdNguoiDuocPhongVan == id).ToList();
                return list;
            }
        }

        public void DeleteLichPhongVanByIdNguoiDuocPhongVan(string id)
        {
            using (var context = new AppDbContext())
            {
                var lichPhongVan = context.Set<LichPhongVan>().FirstOrDefault(x => x.IdNguoiDuocPhongVan == id);
                if (lichPhongVan != null)
                {
                    context.Set<LichPhongVan>().Remove(lichPhongVan);
                    context.SaveChanges();
                }
            }
        }

        public List<LichPhongVan> GetLichPhongVan()
        {
            using (var context = new AppDbContext())
            {
                var result = context.Set<LichPhongVan>().ToList();
                return result;
            }
        }

        public List<LichPhongVan> GetLichPhongVanByIdNgPhongVan(string id)
        {
            using (var context = new AppDbContext())
            {
                var list = context.Set<LichPhongVan>().AsNoTracking().Where(x => x.IdNguoiPhongVan == id).ToList();
                return list;
            }
        }
    }
}
