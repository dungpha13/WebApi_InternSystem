﻿using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using Microsoft.EntityFrameworkCore;
using static AmazingTech.InternSystem.Data.Enum.Enums;



namespace AmazingTech.InternSystem.Repositories
{
     public interface ITechRepo
     {
         Task<List<CongNghe>> GetAllCongNgheAsync(string id);
         Task<int> CreateCongNgheAsync(string idvitri, string user, CongNghe CongNgheModel);
         Task<int> UpdateCongNgheAsync(string idvitri, string user, string congNgheId, CongNghe updatedCongNghe);
         Task<int> DeleteCongNgheAsync(string idvitri, string user, string congNgheId);
         Task<CongNghe> GetCongNgheByIdAsync(string id, string congNgheId);

     }
    public class TechRepository : ITechRepo
    {
        private readonly AppDbContext _context;

        public TechRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<CongNghe>> GetAllCongNgheAsync(string id)
        {                      
            var checkidvitri = _context.ViTris.Where(x => x.Id == id && x.DeletedBy == null).FirstOrDefault();
            if (checkidvitri == null) { throw new Exception(); }                     
            return await _context.CongNghes.Where(x => x.IdViTri == id && x.DeletedBy == null).ToListAsync();
        }

        public async Task<CongNghe> GetCongNgheByIdAsync(string id, string congNgheId)
        {
            var checkidvitri = _context.ViTris.Where(x => x.Id == id && x.DeletedBy == null).FirstOrDefault();
            if (checkidvitri == null) { throw new Exception(); }
            var congNghe = _context.CongNghes.Where(x => x.IdViTri == id && x.DeletedBy == null && x.Id == congNgheId).FirstOrDefault();
            return congNghe;           
        }

        public async Task<int> CreateCongNgheAsync(string idvitri, string user, CongNghe CongNgheModel)
        {
            
                var checkidvitri = _context.ViTris.Where(x => x.Id == idvitri && x.DeletedBy == null).FirstOrDefault();
                if (checkidvitri == null) { throw new Exception(); }
                var check = _context.CongNghes.Where(x => x.Ten == CongNgheModel.Ten && x.IdViTri == idvitri && x.DeletedBy == null).FirstOrDefault();
                if (check != null) { return 0; }
                CongNgheModel.IdViTri = idvitri;
                CongNgheModel.CreatedBy = user;
                CongNgheModel.LastUpdatedBy = user;
                CongNgheModel.LastUpdatedTime = DateTime.Now;
               _context.CongNghes.Add(CongNgheModel);                           
               return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateCongNgheAsync(string idvitri, string user, string congNgheId, CongNghe updatedCongNghe)
        {
           
                var checkidvitri = _context.ViTris.Where(x => x.Id == idvitri && x.DeletedBy == null).FirstOrDefault();
                if (checkidvitri == null) { throw new Exception (); }
                var existingCongNghe = await _context.CongNghes.FirstOrDefaultAsync(c => c.Id == congNgheId  && c.DeletedBy == null);
                if (existingCongNghe == null) { return 0; }
                if (updatedCongNghe.Ten != null) existingCongNghe.Ten = updatedCongNghe.Ten;
                var check = _context.CongNghes.Where(x => x.Ten == existingCongNghe.Ten && x.IdViTri == idvitri && x.DeletedBy == null).FirstOrDefault();
                if (check != null) { return 0; }
                existingCongNghe.IdViTri = idvitri;
                existingCongNghe.ImgUrl = updatedCongNghe.ImgUrl;
                existingCongNghe.LastUpdatedBy = user;
                existingCongNghe.LastUpdatedTime = DateTime.Now;           
                return await _context.SaveChangesAsync();
            
        }

        public async Task<int> DeleteCongNgheAsync(string idvitri, string user,  string congNgheId )
        {
            
                var checkidvitri = _context.ViTris.Where(x => x.Id == idvitri && x.DeletedBy == null).FirstOrDefault();
                if (checkidvitri == null) { throw new Exception(); }
                var congNgheToDelete = await _context.CongNghes.FirstOrDefaultAsync(c => c.Id == congNgheId && c.IdViTri == idvitri &&  c.DeletedBy == null);
                if (congNgheToDelete == null) { return 0; }
                congNgheToDelete.DeletedBy = user;
                congNgheToDelete.DeletedTime = DateTime.Now;
                var cauhoiCongnghe = _context.cauhoiCongnghes.Where(x => x.IdCongNghe == congNgheId).ToList();
                foreach (CauhoiCongnghe obj in cauhoiCongnghe)
                {
                    obj.DeletedBy = user;
                    obj.DeletedTime = DateTime.Now;
                }
            
            await _context.SaveChangesAsync();
            return 1;
        }
    }

}