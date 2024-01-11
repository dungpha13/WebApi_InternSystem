using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Services
{
    public class TruongService : ITruongService
    {
        private ITruongRepository _truongRepository;

        public TruongService(ITruongRepository truongRepository) 
        {
            _truongRepository = truongRepository;
        }

        public IActionResult GetAllTruongs()
        {
            List<TruongHoc> truongs = _truongRepository.GetAllTruongs();
            return new OkObjectResult(truongs);
        }
    }
}
