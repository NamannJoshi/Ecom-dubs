using AutoMapper;
using EcomFinale.DataAccess.Dtos;
using EcomFinale.DataAccess.Entities;

namespace EcomFinale.Business.MappingProfiles;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<Order, OrderDto>().ReverseMap();
    }
}