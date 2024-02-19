using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Data;
using Microsoft.EntityFrameworkCore;
using AmazingTech.InternSystem.Models.DTO;
using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;

namespace AmazingTech.InternSystem.Repositories
{
    public interface IInterviewRepo
    {
        Task<List<Cauhoi>> GetAllCauHoiAsync(string CauhoiCongNgheid);
        Task<int> AwserQuestion(string user, List<PhongVan> phongVan);

    }
    public class InterviewRepository : IInterviewRepo
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public InterviewRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<Cauhoi>> GetAllCauHoiAsync(string CauhoiCongNgheid)
        {
            return await _context.cauhois.Where(x => x.CauhoiCongnghe.Where(c => c.Id == CauhoiCongNgheid && x.DeletedBy == null).Any()).ToListAsync();
        }



        public async Task<int> AwserQuestion(string user, List<PhongVan> phongVan)
        {                      
                foreach (var phongvan in phongVan)
                {
                  var check = _context.cauhoiCongnghes.Where(x => x.Id == phongvan.IdCauHoiCongNghe && x.DeletedBy == null).FirstOrDefault();
                  if ( check == null )
                  {
                    throw new Exception();
                  }
                  phongvan.CreatedBy = user;
                  phongvan.LastUpdatedBy = user;
                  _context.phongVans.Add(phongvan);
                }              
            
            return await _context.SaveChangesAsync();
        }

        /*        public async Task<int> DeleteQuestionAsync(string user, string congNgheId, string cauhoiId)
                {


                }*/

        /*       public async Task<int> AddListQuestionAsync(List<Cauhoi> cauhois, string user, string id)
               {
                   var existingKi = await _context.CongNghes
                       .Where(x => x.Id == id && x.DeletedBy == null)
                       .FirstOrDefaultAsync();

                   if (existingKi == null)
                   {
                       return 0;
                   }

                   foreach (var cauhoi in cauhois)
                   {
                       cauhoi.CreatedBy = user;
                       cauhoi.LastUpdatedBy = user;
                       // Add the question to the context
                       _context.cauhois.Add(cauhoi);

                       // Create a relationship between the question and the technology
                       CauhoiCongnghe cauhoiconghe = new CauhoiCongnghe()
                       {
                           IdCauhoi = cauhoi.Id,
                           IdCongNghe = existingKi.Id,
                           CreatedBy = user,
                           LastUpdatedBy = user,
                       };
                       _context.cauhoiCongnghes.Add(cauhoiconghe);

                   }

                   return await _context.SaveChangesAsync();
               }*/
    }
}
