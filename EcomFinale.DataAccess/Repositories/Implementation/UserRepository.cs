using EcomFinale.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcomFinale.DataAccess.Repositories.Implementation;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext context;

    public UserRepository(AppDbContext context)
    {
        this.context = context;
    }

    public IQueryable<User> GetAllUsers()
    {
        return context.Users;
    }

    public async Task<User> Create(User user)
    {
        var result = await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<User?> GetById(int id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    // public async Task<User> Update(User user, int id)
    // {
    //     var existingUser = await context.Users.FindAsync(id);
    //     if (existingUser == null)
    //     {
    //         throw new Exception($"User with id {id} not found");
    //     }

    //     existingUser.Username = user.Username;
    //     existingUser.Email = user.Email;
    //     // existingUser.PasswordHash = user.PasswordHash;

    //     context.Users.Update(existingUser);
    //     await context.SaveChangesAsync();
    //     return existingUser;
    // }

    public async Task<bool> Delete(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException("User with matching identifier is not found");
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task SaveChanges()
    {
        await this.context.SaveChangesAsync();
    }
}
