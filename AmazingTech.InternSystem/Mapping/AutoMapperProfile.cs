using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.Request.InternInfo;
using AmazingTech.InternSystem.Models.Response.InternInfo;
using AmazingTech.InternSystem.Models.Response.User;
using AmazingTech.InternSystem.Repositories;
using AutoMapper;

namespace AmazingTech.InternSystem.Mapping
{
    public class AutoMapperProfile : Profile
    {
        private readonly UserRepository _userRepository;

        public AutoMapperProfile(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public AutoMapperProfile()
        {
            CreateMap<User, ProfileResponseDTO>().ReverseMap();

            CreateMap<InternInfo, InternInfoDTO>()
            .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime.HasValue ? src.CreatedTime.Value.ToString("dd/MM/yyyy - HH:mm:ss") : null))
            .ForMember(dest => dest.NgaySinh, opt => opt.MapFrom(src => src.NgaySinh.HasValue ? src.NgaySinh.Value.ToString("dd/MM/yyyy") : null))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.HasValue ? src.StartDate.Value.ToString("dd/MM/yyyy") : null))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.HasValue ? src.EndDate.Value.ToString("dd/MM/yyyy") : null))
            .ForMember(dest => dest.DeletedTime, opt => opt.MapFrom(src => src.DeletedTime.HasValue ? src.DeletedTime.Value.ToString("dd/MM/yyyy - HH:mm:ss") : null))
            .ForMember(dest => dest.ViTri, opt => opt.MapFrom(src => src.User!.UserViTris.Select(x => x.ViTri.Ten).ToArray()))
            .ForMember(dest => dest.NhomZalo, opt => opt.MapFrom(src => src.User!.UserNhomZalos.Select(x => x.NhomZalo.TenNhom).ToArray()))
            .ForMember(dest => dest.DuAn, opt => opt.MapFrom(src => src.User!.UserDuAns.Select(x => x.DuAn.Ten).ToArray()))
            .ForMember(dest => dest.GioiTinh, opt => opt.MapFrom(src => src.GioiTinh ? "Nam" : "Nữ"));

            CreateMap<InternInfo, AddInternInfoDTO>().ReverseMap();
            CreateMap<InternInfo, UpdateInternInfoDTO>().ReverseMap();

            //Add Comment
            CreateMap<Comment, CommentInternInfoDTO>().ReverseMap();
            
            //View Comment theo InternInfo
            CreateMap<InternInfo, InternCommentDTO>()
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src =>
                    src.Comments.Select(comment => new CommentDTO
                    {
                        NguoiComment =  comment.NguoiComment!.HoVaTen!,  
                        Content =  comment.Content!   
                    }).ToList()));

        }  
    }    
}
