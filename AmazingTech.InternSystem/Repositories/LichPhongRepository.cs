﻿using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AmazingTech.InternSystem.Repositories
{
    public interface ILichPhongRepository
    {
        LichPhongVan GetLichPhong(string id);
        List<LichPhongVan> GetAllLichPhong();

        int AddLichPhong(LichPhongVan lich);
        int UpdateLichPhong(LichPhongVan lich);
        int DeleteLichPhong(string id);
        
    }
    public class LichPhongRepository :ILichPhongRepository
    {
        private DbSet<LichPhongVan> _DbSet;

        public LichPhongRepository() { }

        public int AddLichPhong(LichPhongVan lich) 
        {   
            using(var context = new AppDbContext())
            {
                context.Set<LichPhongVan>().Add(lich);
                return context.SaveChanges();
            }
        }

        public int DeleteLichPhong(string id)
        {
            throw new NotImplementedException();
        }

        public List<LichPhongVan> GetAllLichPhong()
        {
            using (var context = new AppDbContext())
            {
                var lis = context.Set<LichPhongVan>().ToList();
                return lis;
            }
        }

        public LichPhongVan GetLichPhong(string id)
        {
            throw new NotImplementedException();
        }

        public int UpdateLichPhong(LichPhongVan lich)
        {
            throw new NotImplementedException();
        }
         
    
    }
}
