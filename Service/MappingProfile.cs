using AutoMapper;
using Entities.Domains;
using Service.DTOs;

namespace Service;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserGetDto>().ReverseMap();
        CreateMap<User, UserAddEditDto>().ReverseMap();
        CreateMap<User, UserWithProductsDto>().ReverseMap();
        CreateMap<Product, ProductGetDto>().ReverseMap();
        CreateMap<Product, ProductAddEditDto>().ReverseMap();
        CreateMap<Product, ProductWithUserDto>().ReverseMap();
    }
}
