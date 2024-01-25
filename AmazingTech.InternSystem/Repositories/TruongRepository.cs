﻿using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace AmazingTech.InternSystem.Repositories
{
    public class TruongRepository : ITruongRepository
    {
        private readonly AppDbContext _context;

        public TruongRepository(AppDbContext context)
        {
            _context = context;
        }

        public int AddTruong(TruongHoc truong)
        {
            using (var context = new AppDbContext())
            {
                context.Set<TruongHoc>().Add(truong);
                return context.SaveChanges();
            }
        }

        public int DeleteTruong(TruongHoc truong)
        {
            _context.TruongHocs.Remove(truong);
            return _context.SaveChanges();
        }

        public List<TruongHoc> GetAllTruongs()
        {
            using (var context = new AppDbContext())
            {
                var truongs = context.Set<TruongHoc>().ToList();
                Console.WriteLine("Truong: " + truongs);
                return truongs;
            }
        }

        public TruongHoc? GetTruong(string id)
        {
            return _context.TruongHocs.FirstOrDefault(t => t.Id == id);
        }

        public int UpdateTruong(TruongHoc truong)
        {
            _context.TruongHocs.Update(truong);
            return _context.SaveChanges();
        }
    }
}
