using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.DTO;
using AmazingTech.InternSystem.Repositories;
using AmazingTech.InternSystem.Repositories.AmazingTech.InternSystem.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace AmazingTech.InternSystem.Services
{
    public interface ITechService
    {

        Task<List<TechModel>> getAllTech();
        Task CreateTech(TechModel tech);

        Task UpdateTech(String id, TechModel tech);

        Task DeleteTech(String id, TechModel tech);

    }

    public class TechService : ITechService
    {
        private ITechRepo _congNgheRepo;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TechService(IServiceProvider serviceProvider, IMapper mapper) 
        {
            _congNgheRepo = serviceProvider.GetRequiredService<ITechRepo>();
            _mapper = mapper;
        }

        public async Task<List<TechModel>> getAllTech()
        {
            List<CongNghe> congNghe = await _congNgheRepo.GetAllCongNgheAsync();
            List<TechModel> tech = _mapper.Map<List<TechModel>>(congNghe);
            return tech;
        }

        public async Task CreateTech(TechModel tech)
        {
           CongNghe congNghe = _mapper.Map<CongNghe>(tech);
           await _congNgheRepo.CreateCongNgheAsync(congNghe);
        }

        public async Task UpdateTech(String id, TechModel tech)
        {
            CongNghe congNghe = _mapper.Map<CongNghe>(tech);
            await _congNgheRepo.UpdateCongNgheAsync(id, congNghe);
        }

        public async Task DeleteTech(String id, TechModel tech)
        {
            CongNghe congNghe = _mapper.Map<CongNghe>(tech);
            await _congNgheRepo.DeleteCongNgheAsync(id, congNghe);

        }


    }

    

}
