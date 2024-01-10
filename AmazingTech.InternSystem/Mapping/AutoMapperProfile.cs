using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Response.User;
using AutoMapper;

namespace AmazingTech.InternSystem.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, ProfileResponseDTO>().ReverseMap();

        }
    }
}
