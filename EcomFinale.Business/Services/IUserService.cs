using EcomFinale.DataAccess.Dtos;
using EcomFinale.DataAccess.Dtos.Requests;

namespace EcomFinale.Business.Services;

public interface IUserService
{
    public IQueryable<UserDto> GetAllUsers();

    public Task<UserDto> Create(CreateUserDto user);

    public Task<UserDto?> GetById(int id);

    public Task<UserDto?> GetByEmail(string email);

    public Task<UserDto> Update(CreateUserDto user, int id);

    public Task<bool> Delete(int id);
}
