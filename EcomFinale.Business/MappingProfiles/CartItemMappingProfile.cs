using AutoMapper;
using EcomFinale.DataAccess.Dtos;
using EcomFinale.DataAccess.Entities;

namespace EcomFinale.Business.MappingProfiles;

public class CartItemMappingProfile : Profile
{
    public CartItemMappingProfile()
    {
        CreateMap<CartItem, CartItemDto>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));

        CreateMap<CreateCartItemDto, CartItem>();
    }
}