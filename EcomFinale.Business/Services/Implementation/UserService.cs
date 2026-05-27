using AutoMapper;
using EcomFinale.DataAccess.Dtos;
using EcomFinale.DataAccess.Dtos.Requests;
using EcomFinale.DataAccess.Entities;
using EcomFinale.DataAccess.Repositories;

namespace EcomFinale.Business.Services.Implementation;

public class UserService : IUserService
{
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        this.userRepository = userRepository;
        this.mapper = mapper;
    }

    public IQueryable<UserDto> GetAllUsers()
    {
        return this.userRepository.GetAllUsers()
            .Select(user => new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            });
    }

    public async Task<UserDto> Create(CreateUserDto user)
    {
        var entity = this.mapper.Map<User>(user);
        var created = await this.userRepository.Create(entity);

        return this.mapper.Map<UserDto>(created);
    }

    public async Task<UserDto?> GetById(int id)
    {
        var user = await this.userRepository.GetById(id);
        if (user == null)
        {
            throw new KeyNotFoundException("User with matching identifier is not found");
        }

        return this.mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> GetByEmail(string email)
    {
        var user = await this.userRepository.GetByEmail(email);

        return this.mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> Update(CreateUserDto userDto, int id)
    {
        var user = await this.userRepository.GetById(id);
        if (user == null)
        {
            throw new KeyNotFoundException("User with matching identifier is not found");
        }

        this.mapper.Map(userDto, user);

        await this.userRepository.SaveChanges();
        return this.mapper.Map<UserDto>(user);
    }

    public async Task<bool> Delete(int id)
    {
        return await this.userRepository.Delete(id);
    }
}
