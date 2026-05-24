using AutoMapper;
using EcomFinale.DataAccess.Dtos;
using EcomFinale.DataAccess.Entities;

namespace EcomFinale.Business.MappingProfiles;

public class ProductCategoryProfile : Profile
{
    public ProductCategoryProfile()
    {
        CreateMap<ProductCategoryDto, ProductCategory>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ReverseMap();
    }
}