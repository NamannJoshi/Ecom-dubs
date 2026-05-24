using System;
using AutoMapper;
using EcomFinale.DataAccess.Dtos;
using EcomFinale.DataAccess.Dtos.Requests;
using EcomFinale.DataAccess.Entities;
using Npgsql.Replication;

namespace EcomFinale.Business.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ReverseMap();

        CreateMap<CreateUserDto, User>();
    }
}
