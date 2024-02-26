using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.VItri;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Service
{
    public interface IViTriService
    {
        Task<List<Vitrinew>> GetViTriList();
        Task<int> AddVitri(VitriModel vitriModel, string user);
        Task<int> DeleteVitri(string id, string user);
        Task<int> UpdateVitri(VitriModel updatedVitri, string vitriId, string user);
        Task<List<VitriUserViewModel>> UserViTriView(string id);

    }
}