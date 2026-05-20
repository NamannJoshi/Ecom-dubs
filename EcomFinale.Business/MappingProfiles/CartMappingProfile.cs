namespace EcomFinale.Business.MappingProfiles;
using AutoMapper;
using EcomFinale.DataAccess.Dtos;
using EcomFinale.DataAccess.Entities;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<Cart, CartDto>()
            .ReverseMap();
    }
}