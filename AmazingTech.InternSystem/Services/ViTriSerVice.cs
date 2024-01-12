using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Repo;
using AmazingTech.InternSystem.Service;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Services
{
    public class ViTriSerVice : IViTriService
    {
        private IViTriRepo _VitriRepo;
        private readonly AppDbContext _context;
        public ViTriSerVice(IServiceProvider serviceProvider, AppDbContext context) 
        { 
            _context = context;
            _VitriRepo = serviceProvider.GetRequiredService<IViTriRepo>();
        }

        public List<ViTri> GetViTriList()
        {
            List<ViTri> viTris = _VitriRepo.GetViTriList();
            return viTris;
        }
    }

   
}
