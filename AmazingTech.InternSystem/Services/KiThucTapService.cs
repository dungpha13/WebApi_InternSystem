using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Services
{
    public class KiThucTapService : IKiThucTapService
    {
        private IKiThucTapRepository _kiRepository;

        public KiThucTapService(IKiThucTapRepository kiThucTapRepository) 
        {
            _kiRepository = kiThucTapRepository;
        }

        public IActionResult GetAllKiThucTaps()
        {
            List<KiThucTap> kis = _kiRepository.GetAllKiThucTaps();
            return new OkObjectResult(kis);
        }

        public IActionResult GetKiThucTapsByTruong(string idTruong) 
        {
            List<KiThucTap> kis = _kiRepository.GetAllKiThucTaps().Where(_ => _.IdTruong.Equals(idTruong)).ToList();
            return new OkObjectResult(kis);
        }
    }
}
