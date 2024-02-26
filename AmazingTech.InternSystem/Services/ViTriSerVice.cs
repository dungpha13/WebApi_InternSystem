using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Response.InternInfo;
using AmazingTech.InternSystem.Models.VItri;
using AmazingTech.InternSystem.Repositories;
using AmazingTech.InternSystem.Service;
using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AmazingTech.InternSystem.Services
{
    public class ViTriService : IViTriService
    {
        private IViTriRepository _VitriRepo;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ViTriService(IServiceProvider serviceProvider, AppDbContext context, IMapper mapper)
        {
            _context = context;
            _VitriRepo = serviceProvider.GetRequiredService<IViTriRepository>();
            _mapper = mapper;
        }


        public async Task<List<Vitrinew>> GetViTriList()
        {
            List<ViTri> viTris = await _VitriRepo.GetAllVitri();
            List<Vitrinew> vitrinews = _mapper.Map<List<Vitrinew>>(viTris);
            return vitrinews;
        }
        public async Task<int> AddVitri(VitriModel vitriModel, string user)
        {

            ViTri viTri = _mapper.Map<ViTri>(vitriModel);
            viTri.CreatedBy = user;
            viTri.LastUpdatedBy = user;
            return await _VitriRepo.CreateViTri(viTri);
        }
        public async Task<int> UpdateVitri(VitriModel updatedVitri, string vitriId, string user)
        {
            ViTri viTri = _mapper.Map<ViTri>(updatedVitri);
            viTri.LastUpdatedBy = user;
            return await _VitriRepo.UpdateViTri(vitriId, viTri);
        }
        public async Task<int> DeleteVitri(string id, string user)
        {
            return await _VitriRepo.DeleteViTri(id, user);
        }

        public async Task<List<VitriUserViewModel>> UserViTriView(string id)
        {
            List<InternInfo> interinfor = await _VitriRepo.UserViTriView(id);
            List<InternInfoDTO> viewModels = _mapper.Map<List<InternInfoDTO>>(interinfor);
            List<VitriUserViewModel> viewModel = _mapper.Map<List<VitriUserViewModel>>(viewModels);
            return viewModel;
        }
    }
}
