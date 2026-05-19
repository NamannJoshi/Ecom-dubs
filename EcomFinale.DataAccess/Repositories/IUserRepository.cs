using System;
using EcomFinale.DataAccess.Entities;

namespace EcomFinale.DataAccess.Repositories;

public interface IUserRepository
{
    public IQueryable<User> GetAllUsers();

    public Task<User> Create(User user);

    public Task<User?> GetById(int id);

    public Task<User?> GetByEmail(string email);

    // public Task<User> Update(User user, int id);

    public Task<bool> Delete(int id);

    Task SaveChanges();
}
