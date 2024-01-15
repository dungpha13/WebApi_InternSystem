using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Repositories;
using AmazingTech.InternSystem.Service;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Services
{
    public class ViTriService : IViTriService
    {
        private IViTriRepository _VitriRepo;
        private readonly AppDbContext _context;
        public ViTriService(IServiceProvider serviceProvider, AppDbContext context) 
        { 
            _context = context;
            _VitriRepo = serviceProvider.GetRequiredService<IViTriRepository>();
        }

        public async Task<List<ViTri>> GetViTriList()
        {
            List<ViTri> viTris = await _VitriRepo.GetAllViTriAsync();
            return viTris;
        }
    }

   
}
