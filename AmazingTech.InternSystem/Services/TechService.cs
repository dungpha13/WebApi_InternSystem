using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.DTO;
using AmazingTech.InternSystem.Repositories;
using AutoMapper;

namespace AmazingTech.InternSystem.Services
{
    public interface ITechService
    {

        Task<List<TechView>> getAllTech(string id);

        Task<int> CreateTech(string id, TechModel tech, string user);

        Task<int> UpdateTech(string vitriId, string user, string id, TechModel tech);

        Task<int> DeleteTech(string idVitri, string user, string id);


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

        public async Task<List<TechView>> getAllTech(string id)
        {
            List<CongNghe> congNghe = await _congNgheRepo.GetAllCongNgheAsync(id);       
            List<TechView> tech = _mapper.Map<List<TechView>>(congNghe);
            return tech;
        }

        public async Task<int> CreateTech(string id, TechModel tech, string user)
        {
           CongNghe congNghe = _mapper.Map<CongNghe>(tech);
           return  await _congNgheRepo.CreateCongNgheAsync(id, user, congNghe);          
        }

        public async Task<int> UpdateTech(string vitriId, string user, string id, TechModel tech)
        {
            CongNghe congNghe = _mapper.Map<CongNghe>(tech);
            return await _congNgheRepo.UpdateCongNgheAsync(vitriId, user ,id, congNghe);
        }

        public async Task<int> DeleteTech(string idVitri, string user, string id)
        {            
            return await _congNgheRepo.DeleteCongNgheAsync(idVitri, user, id);
        }


    }

    

}
