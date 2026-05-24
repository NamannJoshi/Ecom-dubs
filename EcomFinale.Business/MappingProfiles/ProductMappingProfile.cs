using AutoMapper;
using EcomFinale.DataAccess.Dtos;
using EcomFinale.DataAccess.Entities;

namespace EcomFinale.Business.MappingProfiles;

public class ProductMappingProfile: Profile {
    public ProductMappingProfile() {
        CreateMap<Product, ProductDto>();
        CreateMap<ProductDto, Product>();
    }
}