using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.DTO;
using AutoMapper;

namespace AmazingTech.InternSystem.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<TechModel, CongNghe>().ReverseMap();
        }
    }
}
