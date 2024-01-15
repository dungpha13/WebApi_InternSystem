using AmazingTech.InternSystem.Data.Entity;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Service
{
    public interface IViTriService
    {
        Task<List<ViTri>> GetViTriList();

    }
}