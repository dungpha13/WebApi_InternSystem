using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.DTO;
using AmazingTech.InternSystem.Repositories.AmazingTech.InternSystem.Repositories;
using AutoMapper;

namespace AmazingTech.InternSystem.Services
{
    public interface ITechService
    {

        Task<List<CongNghe>> getAllTech();

        Task<int> CreateTech(TechModel tech, string user);

        Task<int> UpdateTech(string user, string id, TechModel tech);

        Task<int> DeleteTech(string user, string id);


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

        public async Task<List<CongNghe>> getAllTech()
        {
            List<CongNghe> congNghe = await _congNgheRepo.GetAllCongNgheAsync();
            
            return congNghe;
        }

        public async Task<int> CreateTech(TechModel tech, string user)
        {
           CongNghe congNghe = _mapper.Map<CongNghe>(tech);
           return  await _congNgheRepo.CreateCongNgheAsync(user, congNghe);          
        }

        

        public async Task<int> UpdateTech(string user, string id, TechModel tech)
        {
            CongNghe congNghe = _mapper.Map<CongNghe>(tech);
            return await _congNgheRepo.UpdateCongNgheAsync(user,id, congNghe);
        }

        public async Task<int> DeleteTech(string user, string id)
        {            
            return   await _congNgheRepo.DeleteCongNgheAsync(user, id);
        }


    }

    

}
