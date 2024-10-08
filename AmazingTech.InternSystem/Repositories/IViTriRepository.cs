﻿using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Response.InternInfo;
using AmazingTech.InternSystem.Models.VItri;

namespace AmazingTech.InternSystem.Repositories
{
    public interface IViTriRepository
    {
        Task<List<ViTri>> GetAllVitri();

        Task<int> CreateViTri(ViTri viTri, string user);

        Task<int> UpdateViTri(string viTriId, ViTri updatedViTri, string user);

        Task<int> DeleteViTri(string viTriId, string user);
        Task<List<InternInfo>> UserViTriView(string id);
        public ViTri GetViTriByName(string name);
        
    }
}
